using FX.Core;
using log4net;
using Parse.Core;
using Parse.Core.Domain;
using Parse.Core.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ViettelAPI;
using ViettelAPI.Models;

namespace Parse.Forms.Common
{
    public class AutoPublishTask
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AutoPublishTask));
        private static Task PublishTask;
        private static int Duration = 5;
        private static bool AllowRun = true;

        /// <summary>
        /// khoi chay task
        /// </summary>
        /// <param name="duration">khoang thoi gian giua 2 lan thuc hien. Nho nhat la 5s</param>
        public static void Start(int duration)
        {
            if (duration >= 5)
                Duration = duration;

            PublishTask = Task.Factory.StartNew(() => PublishInv());
        }

        public static void Stop()
        {
            AllowRun = false;
            PublishTask = null;
        }

        /// <summary>
        /// phat hanh invoice
        /// </summary>
        private static void PublishInv()
        {
            while (AllowRun)
            {
                IBussinessLogService logService = IoC.Resolve<IBussinessLogService>();
                try
                {
                    IInvoiceVATService InvSrc = IoC.Resolve<IInvoiceVATService>();
                    var lstInvoice = InvSrc.GetUnPublish();
                    if (lstInvoice.Count() > 0)
                    {
                        foreach (var invoice in lstInvoice)
                        {
                            IProductInvService prodSrc = IoC.Resolve<IProductInvService>();
                            var lstprod = prodSrc.GetByInvoiceID(invoice.Id);
                            invoice.Products = lstprod;
                        }
                        //Lấy danh sách hóa đơn phát hành
                        string strlstFolio = "Danh sách hóa đơn: ";
                        for (int i = 0; i < lstInvoice.Count; i++)
                        {
                            strlstFolio += " - " + lstInvoice[i].FolioNo;
                        }
                        try
                        {

                            IApiParserService apiParserSer = IoC.Resolve<IApiParserService>();
                            var lstInvoiceApi = apiParserSer.ConvertToAPIModel(lstInvoice, AppContext.Current.company);
                            APIResults results = ViettelAPI.APIHelper.SendInvoices(lstInvoiceApi);
                            //Lưu vào db InvoiceConvert
                            InvoiceHelper.UpdatePublishResult(lstInvoice, results);
                            //Lấy danh sách result
                            var successResults = results.createInvoiceOutputs.Where(p => string.IsNullOrEmpty(p.errorCode)).ToList();
                            var failResults = results.createInvoiceOutputs.Where(p => !string.IsNullOrEmpty(p.errorCode)).ToList();
                            string lstFail = "";
                            if (failResults.Count > 0)
                            {
                                foreach (var item in failResults)
                                {
                                    lstFail += " - " + item.transactionUuid.Split('-').ToList()[item.transactionUuid.Split('-').ToList().Count() - 2];
                                }
                            }
                            // Ghi log hóa đơn
                            BussinessLog Bussinesslog = new BussinessLog();
                            Bussinesslog.FileName = "Phát hành tự động hóa đơn";
                            Bussinesslog.CreateDate = DateTime.Now;
                            if (failResults.Count() == 0)
                                Bussinesslog.Error = string.Format("Phát hành tự động hóa đơn thành công - {0}", strlstFolio);
                            else
                                Bussinesslog.Error = string.Format("Phát hành tự động hóa đơn thành công {0}/{1} - {2} - Hóa đơn lỗi: {3}", successResults.Count(), lstInvoice.Count, strlstFolio, lstFail);
                            logService.CreateNew(Bussinesslog);
                            logService.CommitChanges();
                        }
                        catch (Exception ex)
                        {
                            // Ghi log hóa đơn
                            BussinessLog Bussinesslog = new BussinessLog();
                            Bussinesslog.FileName = "Phát hành hóa đơn";
                            Bussinesslog.CreateDate = DateTime.Now;
                            Bussinesslog.Error = strlstFolio + " + Lỗi: " + ex.Message;
                            logService.CreateNew(Bussinesslog);
                            logService.CommitChanges();
                            log.Error(ex);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Ghi log hóa đơn
                    BussinessLog Bussinesslog = new BussinessLog();
                    Bussinesslog.FileName = "Phát hành hóa đơn";
                    Bussinesslog.CreateDate = DateTime.Now;
                    Bussinesslog.Error = "Lỗi: " + ex.Message;
                    logService.CreateNew(Bussinesslog);
                    logService.CommitChanges();
                    log.Error(ex);
                }

                Thread.Sleep(1000 * Duration);
            }
        }
    }
}
