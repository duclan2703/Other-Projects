using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Parse.Core.IService;
using FX.Core;
using DevExpress.XtraEditors;
using Parse.Core.Domain;
using Parse.Core;
using Newtonsoft.Json;
using log4net;
using Parse.Forms.CustomUC;
using System.Configuration;
using ViettelAPI.Models;
using ViettelAPI;
using DevExpress.XtraSplashScreen;

namespace Parse.Forms
{
    public partial class ucInvoiceVAT : UserControl
    {
        private readonly ILog log = LogManager.GetLogger(typeof(ucInvoiceVAT));
        IMainForm Main;
        public ucInvoiceVAT(IMainForm main)
        {
            InitializeComponent();
            Main = main;
            ucPaging.PageSize = 35;
        }

        private void ucInvoiceVAT_Load(object sender, EventArgs e)
        {
            LoadData(ucPaging.PageIndex);
        }

        public void LoadData(int PageIndex)
        {
            int total = 0;
            IInvoiceVATService service = IoC.Resolve<IInvoiceVATService>();
            var list = service.GetUnPublish(PageIndex, ucPaging.PageSize, out total);
            gridInvoiceVAT.DataSource = list;
            gridInvoiceVAT.Focus();
            lblInvoicesNumber.Text = total.ToString();
            ucPaging.PageIndex = PageIndex;
            ucPaging.Total = total;
            ucPaging.UpdatePagingState();
        }

        private void UCPaging_Click(object sender, CustomUC.PagingEventArgs e)
        {
            var button = (SimpleButton)sender;

            if (button.Name == "btnFirst")
                LoadData(1);

            else if (button.Name == "btnPrev")
                LoadData(e.NextPageIndex);

            else if (button.Name == "btnNext")
                LoadData(e.NextPageIndex);

            else if (button.Name == "btnLast")
                LoadData(e.NextPageIndex);
        }

        private void btnPublicInvoice_Click(object sender, EventArgs e)
        {
            if (viewInvoiceVAT.SelectedRowsCount > 0)
            {
                SplashScreenManager.ShowForm(typeof(ProcessIndicator));
                IBussinessLogService logService = IoC.Resolve<IBussinessLogService>();
                try
                {
                    List<InvoiceVAT> lstInvoice = new List<InvoiceVAT>();
                    foreach (var rowHandle in viewInvoiceVAT.GetSelectedRows())
                    {
                        InvoiceVAT entity = (InvoiceVAT)viewInvoiceVAT.GetRow(rowHandle);
                        if (entity != null)
                        {
                            IProductInvService prodSrc = IoC.Resolve<IProductInvService>();
                            var lstprod = prodSrc.GetByInvoiceID(entity.Id);
                            entity.Products = lstprod;
                            lstInvoice.Add(entity);
                        }
                    }
                    //Lấy danh sách hóa đơn phát hành
                    var strlstFolio = "Danh sách hóa đơn: " + lstInvoice[0].FolioNo;
                    for (int i = 1; i < lstInvoice.Count; i++)
                    {
                        strlstFolio += " - " + lstInvoice[i].FolioNo;
                    }
                    try
                    {
                        IApiParserService apiParserSer = IoC.Resolve<IApiParserService>();
                        var lstInvoiceApi = apiParserSer.ConvertToAPIModel(lstInvoice, AppContext.Current.company);
                        //phát hành hóa đơn VIETTEL API
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
                        Bussinesslog.FileName = "Phát hành hóa đơn";
                        Bussinesslog.CreateDate = DateTime.Now;
                        if (failResults.Count() == 0)
                            Bussinesslog.Error = string.Format("Phát hành hóa đơn thành công - {0}", strlstFolio);
                        else
                            Bussinesslog.Error = string.Format("Phát hành hóa đơn thành công {0}/{1} - {2} - Hóa đơn lỗi: {3}", successResults.Count(), lstInvoice.Count, strlstFolio, lstFail);
                        logService.CreateNew(Bussinesslog);
                        logService.CommitChanges();
                        SplashScreenManager.CloseForm();
                        XtraMessageBox.Show("Đã thực hiện phát hành hóa đơn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData(1);
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
                        SplashScreenManager.CloseForm();
                        XtraMessageBox.Show("Phát hành hóa đơn thất bại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    SplashScreenManager.CloseForm();
                    XtraMessageBox.Show("Phát hành hóa đơn thất bại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
                XtraMessageBox.Show("Chưa chọn hóa đơn phát hành", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void cmdTaoLap_Click(object sender, EventArgs e)
        {
            InvoiceVAT invoice = new InvoiceVAT();
            invoice.Products = new List<ProductInv>();
            invoice.VATRate = 10;
            frmInvoice frm = new frmInvoice(this.Main, invoice, false);
            frm.ShowDialog();
        }

        private void viewInvoiceVAT_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            //view hóa đơn
            if (e.Column.FieldName == "ViewInv")
            {
                try
                {
                    InvoiceVAT entity = (InvoiceVAT)viewInvoiceVAT.GetRow(viewInvoiceVAT.FocusedRowHandle);
                    if (entity != null)
                    {
                        IProductInvService prodSrc = IoC.Resolve<IProductInvService>();
                        var lstprod = prodSrc.GetByInvoiceID(entity.Id);
                        entity.Products = lstprod;
                        string xmldata = entity.GetXMLData(AppContext.Current.company);
                        frmInvocieView frm = new frmInvocieView(xmldata, Main, entity);
                        frm.ShowDialog();
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
            }

            //sửa hóa đơn
            if (e.Column.FieldName == "EditInv")
            {
                try
                {
                    InvoiceVAT entity = (InvoiceVAT)viewInvoiceVAT.GetRow(viewInvoiceVAT.FocusedRowHandle);
                    if (entity != null)
                    {
                        IProductInvService prodSrc = IoC.Resolve<IProductInvService>();
                        var lstprod = prodSrc.GetByInvoiceID(entity.Id);
                        entity.Products = lstprod;
                        frmInvoice frm = new frmInvoice(this.Main, entity, true);
                        frm.ShowDialog();
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
            }

            //xóa hóa đơn
            //if (e.Column.FieldName == "DeleteInv")
            //{
            //    try
            //    {
            //        InvoiceVAT entity = (InvoiceVAT)viewInvoiceVAT.GetRow(viewInvoiceVAT.FocusedRowHandle);
            //        if (entity == null)
            //            return;
            //        IProductInvService prodSrc = IoC.Resolve<IProductInvService>();
            //        var lstprod = prodSrc.GetByInvoiceID(entity.Id);
            //        entity.Products = lstprod;

            //        if (XtraMessageBox.Show("Bạn có chắc chắn muốn xóa hóa đơn này?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            //        {
            //            string message = "";
            //            IInvoiceVATService service = IoC.Resolve<IInvoiceVATService>();
            //            if (!service.DeleteInvoice(entity, out message))
            //            {
            //                XtraMessageBox.Show("Có lỗi xảy ra, vui lòng thực hiện lại", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            //                return;
            //            }
            //            ucInvoiceVAT uc = new ucInvoiceVAT(this.Main);
            //            this.Main.AddUC(uc);
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        log.Error(ex);
            //    }
            //}
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadData(1);
        }

        private void btnDeleteAll_Click(object sender, EventArgs e)
        {
            if (XtraMessageBox.Show("Bạn có chắc chắn muốn xóa tất cả hóa đơn?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                SplashScreenManager.ShowForm(typeof(ProcessIndicator));
                IBussinessLogService logService = IoC.Resolve<IBussinessLogService>();
                IInvoiceVATService invService = IoC.Resolve<IInvoiceVATService>();
                try
                {
                    List<InvoiceVAT> lstInvoice = invService.GetUnPublish();
                    foreach (var invoice in lstInvoice)
                    {
                        IProductInvService prodSrc = IoC.Resolve<IProductInvService>();
                        var lstprod = prodSrc.GetByInvoiceID(invoice.Id);
                        invoice.Products = lstprod;
                    }
                    if (lstInvoice.Count > 0)
                    {
                        string message = "";
                        IInvoiceVATService service = IoC.Resolve<IInvoiceVATService>();
                        if (!service.DeleteInvoices(lstInvoice, out message))
                        {
                            SplashScreenManager.CloseForm();
                            XtraMessageBox.Show("Có lỗi xảy ra, vui lòng thực hiện lại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        SplashScreenManager.CloseForm();
                        XtraMessageBox.Show("Đã thực hiện xóa hóa đơn", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ucInvoiceVAT uc = new ucInvoiceVAT(this.Main);
                        this.Main.AddUC(uc);
                    }
                    else
                    {
                        SplashScreenManager.CloseForm();
                        XtraMessageBox.Show("Không tồn tại hóa đơn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception ex)
                {
                    SplashScreenManager.CloseForm();
                    XtraMessageBox.Show("Có lỗi xảy ra, vui lòng thực hiện lại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    log.Error(ex);
                }
            }
        }

        private void btnDeleteSelected_Click(object sender, EventArgs e)
        {
            if (viewInvoiceVAT.SelectedRowsCount > 0)
            {
                if (XtraMessageBox.Show("Bạn có chắc chắn muốn xóa các hóa đơn này?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    SplashScreenManager.ShowForm(typeof(ProcessIndicator));
                    try
                    {
                        List<InvoiceVAT> lstInvoice = new List<InvoiceVAT>();
                        foreach (var rowHandle in viewInvoiceVAT.GetSelectedRows())
                        {
                            InvoiceVAT entity = (InvoiceVAT)viewInvoiceVAT.GetRow(rowHandle);
                            if (entity != null)
                            {
                                IProductInvService prodSrc = IoC.Resolve<IProductInvService>();
                                var lstprod = prodSrc.GetByInvoiceID(entity.Id);
                                entity.Products = lstprod;
                                lstInvoice.Add(entity);
                            }
                        }
                        string message = "";
                        IInvoiceVATService service = IoC.Resolve<IInvoiceVATService>();
                        if (!service.DeleteInvoices(lstInvoice, out message))
                        {
                            SplashScreenManager.CloseForm();
                            XtraMessageBox.Show("Có lỗi xảy ra, vui lòng thực hiện lại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        SplashScreenManager.CloseForm();
                        XtraMessageBox.Show("Đã thực hiện xóa hóa đơn", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ucInvoiceVAT uc = new ucInvoiceVAT(this.Main);
                        this.Main.AddUC(uc);
                    }
                    catch (Exception ex)
                    {
                        SplashScreenManager.CloseForm();
                        XtraMessageBox.Show("Có lỗi xảy ra, vui lòng thực hiện lại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        log.Error(ex);
                    }
                }
            }
            else
                XtraMessageBox.Show("Chưa chọn hóa đơn xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void btnPublicAll_Click(object sender, EventArgs e)
        {
            SplashScreenManager.ShowForm(typeof(ProcessIndicator));
            IBussinessLogService logService = IoC.Resolve<IBussinessLogService>();
            IInvoiceVATService invService = IoC.Resolve<IInvoiceVATService>();
            try
            {
                List<InvoiceVAT> lstInvoice = invService.GetUnPublish();
                foreach (var invoice in lstInvoice)
                {
                    IProductInvService prodSrc = IoC.Resolve<IProductInvService>();
                    var lstprod = prodSrc.GetByInvoiceID(invoice.Id);
                    invoice.Products = lstprod;
                }
                if (lstInvoice.Count > 0)
                {

                    //Lấy danh sách hóa đơn phát hành
                    var strlstFolio = "Danh sách hóa đơn: " + lstInvoice[0].FolioNo;
                    for (int i = 1; i < lstInvoice.Count; i++)
                    {
                        strlstFolio += " - " + lstInvoice[i].FolioNo;
                    }
                    try
                    {

                        IApiParserService apiParserSer = IoC.Resolve<IApiParserService>();
                        var a = AppContext.Current.company.InvPattern;
                        var lstInvoiceApi = apiParserSer.ConvertToAPIModel(lstInvoice, AppContext.Current.company);
                        //phát hành hóa đơn VIETTEL API
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
                        Bussinesslog.FileName = "Phát hành hóa đơn";
                        Bussinesslog.CreateDate = DateTime.Now;
                        if (failResults.Count() == 0)
                            Bussinesslog.Error = string.Format("Phát hành hóa đơn thành công - {0}", strlstFolio);
                        else
                            Bussinesslog.Error = string.Format("Phát hành hóa đơn thành công {0}/{1} - {2} - Hóa đơn lỗi: {3}", successResults.Count(), lstInvoice.Count, strlstFolio, lstFail);
                        logService.CreateNew(Bussinesslog);
                        logService.CommitChanges();
                        SplashScreenManager.CloseForm();
                        XtraMessageBox.Show("Đã thực hiện phát hành hóa đơn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData(1);
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
                        SplashScreenManager.CloseForm();
                        XtraMessageBox.Show("Phát hành hóa đơn thất bại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        log.Error(ex);
                    }
                }
                else
                {
                    SplashScreenManager.CloseForm();
                    XtraMessageBox.Show("Không tồn tại hóa đơn chưa phát hành", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                SplashScreenManager.CloseForm();
                XtraMessageBox.Show("Phát hành hóa đơn thất bại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
