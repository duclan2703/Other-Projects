using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Parse.Core.Domain;
using System.IO;
using FX.Core;
using Parse.Core;
using Parse.Core.IService;
using DevExpress.XtraBars;
using System.IO.Compression;
using Parse.Forms.CustomUC;
using log4net;
using System.Threading.Tasks;
using DevExpress.XtraSplashScreen;
using OfficeOpenXml;
using System.Globalization;
using Newtonsoft.Json;
using Parse.Forms.Common;
using System.Configuration;
using System.Collections.Concurrent;

namespace Parse.Forms
{
    public delegate void VoidMethod();
    public partial class frmMain : XtraForm, IMainForm
    {
        private readonly ILog log = LogManager.GetLogger(typeof(frmMain));
        private string comCode = !string.IsNullOrEmpty(ConfigurationManager.AppSettings["COM_CODE"]) ? ConfigurationManager.AppSettings["COM_CODE"].ToLower() : null;
        private List<CustomWatcher> Watchers = new List<CustomWatcher>();
        public frmMain()
        {
            InitializeComponent();
            InternetExplorerBrowserEmulation.SetBrowserEmulationVersion();
            SetMenuUpload();
            var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            this.Text = String.Format("Hóa đơn điện tử v{0}", version);
        }

        public void AddUC(UserControl uc)
        {
            panelParent.Controls.Clear();
            panelParent.AutoScroll = true;
            panelParent.Controls.Add(uc);
            uc.Dock = DockStyle.Fill;
        }

        public void frmMain_Load(object sender, EventArgs e)
        {
            IconTaskBar.Visible = false;
            if (AppContext.Current.company == null)
            {
                if (XtraMessageBox.Show("Thông tin đơn vị chưa được khởi tạo. Thêm ngay bây giờ?", "Thông Báo", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    //popup frmCompanyInfo
                    frmCompany frm = new frmCompany();
                    frm.ShowDialog();
                }
            }
            ucBussinessLog uc = new ucBussinessLog();
            AddUC(uc);

            // create watchers
            InitWatcher();
        }

        public void InitWatcher()
        {
            // stop and clear watchers
            //while (Watchers.Count > 0)
            //{
            //    Watchers[0].EnableRaisingEvents = false;
            //    Watchers[0].Changed -= new FileSystemEventHandler(WatcherOnChanged);
            //    Watchers[0].Dispose();
            //    Watchers.RemoveAt(0);
            //}

            // add and start watchers

            ISetupService setupService = IoC.Resolve<ISetupService>();
            var list = setupService.GetAll();
            if (list.Count == 0)
                return;

            foreach (var menu in MenuModel.MenuItems)
            {
                var config = list.FirstOrDefault(p => p.Code == menu.Code);
                if (config != null && Directory.Exists(config.FilePath))
                {
                    CustomWatcher watcher = new CustomWatcher();
                    watcher.Menu = menu;
                    watcher.Path = config.FilePath;
                    watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
                       | NotifyFilters.FileName | NotifyFilters.DirectoryName;
                    //watcher.Filter = filter;
                    watcher.Created += new FileSystemEventHandler(WatcherOnChanged);
                    watcher.EnableRaisingEvents = true; // Begin watching.         
                    Watchers.Add(watcher);
                }
            }
        }

        // Load menu upload file
        private void SetMenuUpload()
        {
            foreach (var item in MenuModel.MenuItems)
            {
                BarButtonItem barButton = new BarButtonItem();
                barButton.Id = item.Id;
                barButton.Caption = item.Caption;
                barButton.ImageUri = item.ImageUri;
                barButton.Name = item.Code;
                barButton.ItemClick += new ItemClickEventHandler(btnParser_ItemClick);
                barSubItem2.AddItem(barButton);
            }
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            //this.ShowInTaskbar = false;
            //this.Hide();
            //IconTaskBar.Visible = true;
            //IconTaskBar.ShowBalloonTip(1000, "Invoice", "Pin to taskbar. Click to open.", ToolTipIcon.Info);
            Application.Exit();
        }

        private void IconTaskBar_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.ShowInTaskbar = true;
            IconTaskBar.Visible = false;
            this.Show();
        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ShowInTaskbar = true;
            IconTaskBar.Visible = false;
            this.Show();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnCompanyInfo_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmCompany frm = new frmCompany();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                AppContext.InitContext(frm.COM);
            }
        }

        private void btnSetup_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmSetting frm = new frmSetting(this);
            frm.ShowDialog();
        }

        private void btnListInvoice_ItemClick(object sender, ItemClickEventArgs e)
        {
            ucInvoiceList uc = new ucInvoiceList(this);
            AddUC(uc);
        }

        private void btnHome_ItemClick(object sender, ItemClickEventArgs e)
        {
            ucInvoiceVAT uc = new ucInvoiceVAT(this);
            AddUC(uc);
        }

        private void btnBussinessLog_ItemClick(object sender, ItemClickEventArgs e)
        {
            ucBussinessLog uc = new ucBussinessLog();
            AddUC(uc);
        }

        private void btnProxy_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmProxy frm = new frmProxy();
            frm.ShowDialog();
        }

        /// <summary>
        ///  Xử lý dữ liệu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnParser_ItemClick(object sender, ItemClickEventArgs e)
        {
            BarButtonItem button = e.Item as BarButtonItem;
            MenuModel menu = MenuModel.GetById(button.Id);
            if (menu == null)
            {
                XtraMessageBox.Show("Không tìm thấy menu: " + button.Caption, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            IBussinessLogService logService = IoC.Resolve<IBussinessLogService>();
            var filter = (menu.FileExtensions == null || menu.FileExtensions.Length == 0) ? "*.*" : string.Join(";", menu.FileExtensions.Select(x => "*" + x));
            openFileDialog.Filter = "Files (" + filter + ") | " + filter;
            openFileDialog.Multiselect = false;
            openFileDialog.Title = "Upload file " + button.Caption;
            DialogResult result = openFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                string message = "";
                SplashScreenManager.ShowForm(typeof(ProcessIndicator));
                try
                {
                    var invSuccess = 0;
                    var invTotal = 0;
                    var mesError = "";
                    if (menu.FileExtensions.Contains(Path.GetExtension(openFileDialog.FileName).ToLower()))
                    {
                        IParserService service;
                        try
                        {
                            service = ParserResolveHelper.Resolve(menu.ServiceName);
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                            XtraMessageBox.Show("Lỗi:" + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        //service.FileProcessing(openFileDialog.FileName, ref invSuccess, ref invTotal, ref mesError);
                    }
                    // Load lại form
                    ucBussinessLog uc = new ucBussinessLog();
                    AddUC(uc);
                    // Ghi log hóa đơn
                    //BussinessLog Bussinesslog = new BussinessLog();
                    //Bussinesslog.FileName = openFileDialog.FileName;
                    //Bussinesslog.CreateDate = DateTime.Now;
                    //Bussinesslog.Error = string.Format("{0}{1}/{2}: {3}", "Upload hóa đơn thành công ", invSuccess, invTotal, mesError);
                    //logService.CreateNew(Bussinesslog);
                    //logService.CommitChanges();
                    XtraMessageBox.Show(mesError, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    // Ghi log hóa đơn
                    BussinessLog Bussinesslog = new BussinessLog();
                    Bussinesslog.FileName = openFileDialog.FileName;
                    Bussinesslog.CreateDate = DateTime.Now;
                    Bussinesslog.Error = ex.Message;
                    logService.CreateNew(Bussinesslog);
                    logService.CommitChanges();
                    log.Error(ex);
                    ucBussinessLog uc = new ucBussinessLog();
                    AddUC(uc);
                    XtraMessageBox.Show("Có lỗi xảy ra, vui lòng thực hiện lại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    SplashScreenManager.CloseForm();
                }
            }
        }

        private void WatcherOnChanged(object source, FileSystemEventArgs e)
        {
            IBussinessLogService logService = IoC.Resolve<IBussinessLogService>();
            ISetupService setupSrc = IoC.Resolve<ISetupService>();
            var currentWatcher = ((CustomWatcher)source);
            string[] arr_filter = currentWatcher.Menu.FileExtensions;
            var fileName = e.FullPath;
            FileInfo file = new FileInfo(fileName);
            if (!arr_filter.Contains(file.Extension))
                return;
            string message = "";
            var folderPath = ((FileSystemWatcher)source).Path;
            var company = AppContext.Current.company;
            try
            {
                IAEONService service = IoC.Resolve<IAEONService>();

                var invSuccess = 0;
                var invTotal = 0;
                var mesError = "";
                service.FileProcessing(file.FullName, company.InvPattern, company.InvSerial, ref invSuccess, ref invTotal, ref mesError);
                //Di chuyển file
                if(invSuccess== invTotal)
                {
                    string successDir = Path.Combine(folderPath, "Success", DateTime.Now.ToString("yyyyMMdd"));
                    if (!Directory.Exists(successDir))
                        Directory.CreateDirectory(successDir);
                File.Move(file.FullName, Path.Combine(successDir, file.Name));
                }
                else
                {
                    string failDir = Path.Combine(folderPath, "Error", DateTime.Now.ToString("yyyyMMdd"));
                    if (!Directory.Exists(failDir))
                        Directory.CreateDirectory(failDir);
                    File.Move(file.FullName, Path.Combine(failDir, file.Name));
                }

                // Messagess success
                IconTaskBar.Visible = true;
                IconTaskBar.ShowBalloonTip(5000, "Thông báo", "Đã thực hiện phát hành hóa đơn mới!", ToolTipIcon.Info);
                IconTaskBar.Visible = false;
            }
            catch (Exception ex)
            {
                // write log
                BussinessLog Bussinesslog = new BussinessLog();
                Bussinesslog.FileName = file.FullName;
                Bussinesslog.CreateDate = DateTime.Now;
                Bussinesslog.Error = ex.Message;
                logService.CreateNew(Bussinesslog);
                logService.CommitChanges();

                // move file to error folder
                string errorDir = Path.Combine(folderPath, "ERROR", DateTime.Now.ToString("yyyyMMdd"));
                if (!Directory.Exists(errorDir))
                    Directory.CreateDirectory(errorDir);
                File.Move(file.FullName, Path.Combine(errorDir, file.Name));
                // Messagess error
                IconTaskBar.Visible = true;
                IconTaskBar.ShowBalloonTip(5000, "Thông báo", "Hóa đơn lỗi!", ToolTipIcon.Warning);
                IconTaskBar.Visible = false;
            }
        }

        private void btnUpload_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmUpload frm = new frmUpload();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                IBussinessLogService logService = IoC.Resolve<IBussinessLogService>();
                SplashScreenManager.ShowForm(typeof(ProcessIndicator));
                try
                {
                    int invSuccess = 0;
                    int invTotal = 0;
                    string mesError = "";
                    IAEONService service = IoC.Resolve<IAEONService>();
                    service.FileProcessing(frm.path, frm.pattern, frm.serial, ref invSuccess, ref invTotal, ref mesError);
                    // Load lại form
                    ucBussinessLog uc = new ucBussinessLog();
                    AddUC(uc);
                    XtraMessageBox.Show(mesError, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    // Ghi log hóa đơn
                    BussinessLog Bussinesslog = new BussinessLog();
                    Bussinesslog.FileName = openFileDialog.FileName;
                    Bussinesslog.CreateDate = DateTime.Now;
                    Bussinesslog.Error = ex.Message;
                    logService.CreateNew(Bussinesslog);
                    logService.CommitChanges();
                    log.Error(ex);
                    ucBussinessLog uc = new ucBussinessLog();
                    AddUC(uc);
                    XtraMessageBox.Show("Có lỗi xảy ra, vui lòng thực hiện lại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    SplashScreenManager.CloseForm();
                }
            }
        }
    }
}