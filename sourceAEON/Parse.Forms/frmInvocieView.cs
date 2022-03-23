using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Microsoft.Win32;
using Parse.Core.Domain;
using log4net;
using Parse.Core;
using Newtonsoft.Json;
using Parse.Forms.CustomUC;
using System.Configuration;
using ViettelAPI.Models;
using ViettelAPI;
using Parse.Core.IService;
using FX.Core;

namespace Parse.Forms
{
    public partial class frmInvocieView : DevExpress.XtraEditors.XtraForm
    {
        private readonly ILog log = LogManager.GetLogger(typeof(frmInvocieView));
        string XMLData = "";
        InvoiceVAT Invoice;
        IMainForm Main;
        public frmInvocieView(string xmldata, IMainForm main, InvoiceVAT invoice = null)
        {
            InitializeComponent();
            Main = main;
            if (invoice != null)
            {
                Invoice = invoice;
                this.XMLData = xmldata;
                LoadInvoice(this.XMLData);

                //  btnPublishInvoice.Visible = true;
                btnEditInvocie.Visible = string.IsNullOrEmpty(invoice.No);
                btnPrint.Visible = false;
            }
            else
            {
                invBrowser.DocumentText = xmldata;
                invBrowser.ScriptErrorsSuppressed = true;

                //   btnPublishInvoice.Visible = false;
                btnEditInvocie.Visible = false;
                btnPrint.Visible = true;
                btnPrint.Location = new Point(367, 626);
            }
        }

        public void LoadInvoice(string xmlData)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(xmlData))
                {
                    MessageBox.Show("Lỗi dữ liệu, vui lòng thực hiện lại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                int products = 1;
                string html = DsigViewer.GetHtml(xmlData, out products);
                if (string.IsNullOrWhiteSpace(html))
                {
                    MessageBox.Show("Lỗi dữ liệu, vui lòng thực hiện lại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                string java = "<script src=\"" + path + "\\Content\\viewer\\jquery.min.js\"></script>";
                java += "<script src=\"" + path + "\\Content\\viewer\\main.js\"></script>";
                java += "<link href=\"" + path + "\\Content\\viewer\\styles.css\" rel=\"stylesheet\" type=\"text/css\">";
                html = html.Replace("<head>", "<head>" + java);
                if (products > 1)
                {
                    StringBuilder replace = new StringBuilder();
                    replace.Append("<div class='pagination'><a href='#' class='number' id='ap0' onclick='showPageContent(1); return false;'><<</a><a href='#' id='prev' class='prevnext' onclick='previewPage()'><</a>");
                    for (int i = 1; i <= products; i++)
                    {
                        replace.Append("<a href='#' class='number' id='ap" + i + "' onclick='showPageContent(" + i + "); return false;'>" + i + "</a>");
                    }
                    replace.Append("<a href='#' id='next' class='prevnext' onclick='nextPage()'>></a></div>");
                    replace.Append("<script type='text/javascript'>$(document).ready(function () {$('.VATTEMP .invtable .prds').ProductNumberPagination({ number: 10 });});</script></body>");
                    html = html.Replace("</body>", replace.ToString());
                }
                invBrowser.DocumentText = html;
                invBrowser.ScriptErrorsSuppressed = true;
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show("Bạn cần sử dụng với quyền Administartor, vui lòng thực hiện lại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Application.Exit();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra, vui lòng thực hiện lại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void invBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            RegistryKey r;
            try
            {
                r = RegistryKey.OpenRemoteBaseKey(RegistryHive.CurrentUser, "");
                var key = r.OpenSubKey(@"Software\Microsoft\Internet Explorer\PageSetup", RegistryKeyPermissionCheck.ReadWriteSubTree);
                key.SetValue("(Default)", "");
                key.SetValue("font", "");
                key.SetValue("footer", "");
                key.SetValue("header", "");
                key.SetValue("margin_bottom", "0.750000");
                key.SetValue("margin_left", "0.750000");
                key.SetValue("margin_right", "0.750000");
                key.SetValue("margin_top", "0.750000");
                key.SetValue("Print_Background", "yes");
                key.SetValue("Shrink_To_Fit", "yes");
            }
            catch (Exception)
            {
                MessageBox.Show("Có lỗi xảy ra, vui lòng thực hiện lại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            invBrowser.ShowPrintDialog();
        }

        private void btnPublishInvoice_Click(object sender, EventArgs e)
        {
            try
            {
                List<InvoiceVAT> lstInvoice = new List<InvoiceVAT>();
                lstInvoice.Add(this.Invoice);
                IApiParserService apiParserSer = IoC.Resolve<IApiParserService>();
                var lstInvoiceApi = apiParserSer.ConvertToAPIModel(lstInvoice, AppContext.Current.company);
                //phát hành hóa đơn
                // VIETTEL API
                APIResults results = ViettelAPI.APIHelper.SendInvoices(lstInvoiceApi);
                //Lưu vào db InvoiceConvert
                InvoiceHelper.UpdatePublishResult(lstInvoice, results);
                XtraMessageBox.Show("Đã thực hiện phát hành hóa đơn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        private void btnEditInvocie_Click(object sender, EventArgs e)
        {
            frmInvoice frm = new frmInvoice(this.Main, this.Invoice, true);
            this.Hide();
            frm.ShowDialog();
            this.Close();
        }
    }
}