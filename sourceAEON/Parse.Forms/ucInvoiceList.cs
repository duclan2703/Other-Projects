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
using Parse.Core;
using DevExpress.XtraEditors;
using Parse.Forms.CustomUC;
using log4net;
using Parse.Core.Domain;
using Newtonsoft.Json;
using System.Configuration;
using System.IO;
using DevExpress.XtraSplashScreen;

namespace Parse.Forms
{
    public partial class ucInvoiceList : UserControl
    {
        private readonly IInvoiceVATService invSrc;
        private readonly ILog log = LogManager.GetLogger(typeof(ucInvoiceList));
        IMainForm Main;
        public ucInvoiceList(IMainForm main)
        {
            InitializeComponent();
            Main = main;
            invSrc = IoC.Resolve<IInvoiceVATService>();
            ucPaging.PageSize = 30;
        }

        private void ucInvoiceList_Load(object sender, EventArgs e)
        {
            LoadData(ucPaging.PageIndex);
        }

        public void LoadData(int PageIndex)
        {
            int total = 0;
            gridListInv.DataSource = invSrc.GetPublishSuccess(PageIndex, ucPaging.PageSize, out total);
            ucPaging.PageIndex = PageIndex;
            ucPaging.Total = total;
            ucPaging.UpdatePagingState();
        }

        private void btnConvert_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                SplashScreenManager.ShowForm(typeof(ProcessIndicator));
                InvoiceVAT entity = (InvoiceVAT)viewListInv.GetRow(viewListInv.FocusedRowHandle);
                string folder = AppDomain.CurrentDomain.BaseDirectory + "PRINT_INVOICE";
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);
                var fileName = string.Format("{0}_{1}_{2}", entity.Pattern.Replace("/", ""), entity.Serial.Replace("/", ""), entity.No.Replace("/", ""));
                string path = string.Format("{0}\\{1}.pdf", folder, fileName);
                if (!File.Exists(path))
                {
                    var data = ViettelAPI.APIHelper.GetInvoicePdf(entity.No, entity.Fkey);
                    //var invoiceInfo = JsonConvert.DeserializeObject<ViettelAPI.Models.InvoicePdfInfo>(data);
                    if (!string.IsNullOrEmpty(data.errorCode))
                        throw new Exception(data.errorCode + ":" + data.description);

                    File.WriteAllBytes(path, data.fileToBytes);
                }
                frmInvoicePdf frmPdf = new frmInvoicePdf(path, this.Main);
                frmPdf.ShowDialog();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                XtraMessageBox.Show("Có lỗi xảy ra, vui lòng thực hiện lại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SplashScreenManager.CloseForm();
            }
        }

        private void btnESCConvert_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            InvoiceVAT entity = (InvoiceVAT)viewListInv.GetRow(viewListInv.FocusedRowHandle);

            try
            {
                bool useViettelAPI = ConfigurationManager.AppSettings["POPULAR_API"] == "1";
                //view hóa đơn
                if (useViettelAPI)
                {
                    IProductInvService prodSrc = IoC.Resolve<IProductInvService>();
                    var lstprod = prodSrc.GetByInvoiceID(entity.Id);
                    entity.Products = lstprod;
                    string xmldata = entity.GetXMLData(AppContext.Current.company);
                    frmInvocieView frm = new frmInvocieView(xmldata, Main, entity);
                    frm.ShowDialog();
                }
                else
                {
                    // in chuyển đổi
                    if (entity != null)
                    {
                        string xmlResult = APIHelper.ConvertInv(entity.Fkey, entity.Pattern, entity.Serial, entity.No);
                        if (xmlResult.Contains("ERR:"))
                        {
                            //cập nhật lại trạng thái vào CSDL
                            XtraMessageBox.Show("", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        entity.Converted = true;
                        invSrc.Update(entity);
                        invSrc.CommitChanges();

                        //View hóa đơn
                        XtraMessageBox.Show("In chuyển đổi hóa đơn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData(1);
                        frmInvocieView frm = new frmInvocieView(JsonConvert.DeserializeObject<string>(xmlResult), this.Main);
                        frm.ShowDialog();

                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                XtraMessageBox.Show("Có lỗi xảy ra, vui lòng thực hiện lại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

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
    }
}
