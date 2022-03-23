namespace Parse.Forms
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            //if (disposing && (components != null))
            //{
            //    components.Dispose();
            //}
            //base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar2 = new DevExpress.XtraBars.Bar();
            this.btnUpload = new DevExpress.XtraBars.BarButtonItem();
            this.barSubItem1 = new DevExpress.XtraBars.BarSubItem();
            this.btnCompanyInfo = new DevExpress.XtraBars.BarButtonItem();
            this.btnSetup = new DevExpress.XtraBars.BarButtonItem();
            this.btnProxy = new DevExpress.XtraBars.BarButtonItem();
            this.barSubItem2 = new DevExpress.XtraBars.BarSubItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem5 = new DevExpress.XtraBars.BarButtonItem();
            this.popupMenu1 = new DevExpress.XtraBars.PopupMenu(this.components);
            this.btnListInvoice = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem7 = new DevExpress.XtraBars.BarButtonItem();
            this.btnInvoiceUnPublish = new DevExpress.XtraBars.BarButtonItem();
            this.btnHome = new DevExpress.XtraBars.BarButtonItem();
            this.btnBussinessLog = new DevExpress.XtraBars.BarButtonItem();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.IconTaskBar = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panelParent = new DevExpress.XtraEditors.PanelControl();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu1)).BeginInit();
            this.contextMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelParent)).BeginInit();
            this.SuspendLayout();
            // 
            // barManager1
            // 
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar2});
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barButtonItem1,
            this.barSubItem1,
            this.barButtonItem2,
            this.btnCompanyInfo,
            this.btnSetup,
            this.barButtonItem5,
            this.btnListInvoice,
            this.barButtonItem7,
            this.barSubItem2,
            this.btnInvoiceUnPublish,
            this.btnHome,
            this.btnBussinessLog,
            this.btnProxy,
            this.btnUpload});
            this.barManager1.MainMenu = this.bar2;
            this.barManager1.MaxItemId = 22;
            // 
            // bar2
            // 
            this.bar2.BarName = "Main menu";
            this.bar2.DockCol = 0;
            this.bar2.DockRow = 0;
            this.bar2.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar2.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnUpload, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barSubItem1, "", true, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barSubItem2, "", true, true, false, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)});
            this.bar2.OptionsBar.MultiLine = true;
            this.bar2.OptionsBar.UseWholeRow = true;
            this.bar2.Text = "Main menu";
            // 
            // btnUpload
            // 
            this.btnUpload.Caption = "Upload";
            this.btnUpload.Id = 21;
            this.btnUpload.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnUpload.ImageOptions.Image")));
            this.btnUpload.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("btnUpload.ImageOptions.LargeImage")));
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnUpload_ItemClick);
            // 
            // barSubItem1
            // 
            this.barSubItem1.Caption = "Cấu Hình";
            this.barSubItem1.Id = 1;
            this.barSubItem1.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("barSubItem1.ImageOptions.Image")));
            this.barSubItem1.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("barSubItem1.ImageOptions.LargeImage")));
            this.barSubItem1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnCompanyInfo, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnSetup),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.Caption, this.btnProxy, "Cấu hình proxy")});
            this.barSubItem1.Name = "barSubItem1";
            // 
            // btnCompanyInfo
            // 
            this.btnCompanyInfo.Caption = "Thông tin đơn vị";
            this.btnCompanyInfo.Id = 3;
            this.btnCompanyInfo.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnCompanyInfo.ImageOptions.Image")));
            this.btnCompanyInfo.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("btnCompanyInfo.ImageOptions.LargeImage")));
            this.btnCompanyInfo.Name = "btnCompanyInfo";
            this.btnCompanyInfo.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnCompanyInfo_ItemClick);
            // 
            // btnSetup
            // 
            this.btnSetup.Caption = "Cấu hình thư mục";
            this.btnSetup.Id = 4;
            this.btnSetup.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnSetup.ImageOptions.Image")));
            this.btnSetup.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("btnSetup.ImageOptions.LargeImage")));
            this.btnSetup.Name = "btnSetup";
            this.btnSetup.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnSetup_ItemClick);
            // 
            // btnProxy
            // 
            this.btnProxy.Caption = "Cấu hình proxy";
            this.btnProxy.Id = 17;
            this.btnProxy.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnProxy.ImageOptions.Image")));
            this.btnProxy.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("btnProxy.ImageOptions.LargeImage")));
            this.btnProxy.Name = "btnProxy";
            this.btnProxy.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnProxy_ItemClick);
            // 
            // barSubItem2
            // 
            this.barSubItem2.Caption = "Upload";
            this.barSubItem2.Id = 8;
            this.barSubItem2.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("barSubItem2.ImageOptions.Image")));
            this.barSubItem2.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("barSubItem2.ImageOptions.LargeImage")));
            this.barSubItem2.Name = "barSubItem2";
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.barManager1;
            this.barDockControlTop.Size = new System.Drawing.Size(843, 24);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 662);
            this.barDockControlBottom.Manager = this.barManager1;
            this.barDockControlBottom.Size = new System.Drawing.Size(843, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 24);
            this.barDockControlLeft.Manager = this.barManager1;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 638);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(843, 24);
            this.barDockControlRight.Manager = this.barManager1;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 638);
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "barButtonItem1";
            this.barButtonItem1.Id = 0;
            this.barButtonItem1.Name = "barButtonItem1";
            // 
            // barButtonItem2
            // 
            this.barButtonItem2.Caption = "barButtonItem2";
            this.barButtonItem2.Id = 2;
            this.barButtonItem2.Name = "barButtonItem2";
            // 
            // barButtonItem5
            // 
            this.barButtonItem5.ActAsDropDown = true;
            this.barButtonItem5.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.DropDown;
            this.barButtonItem5.Caption = "Upload hóa đơn";
            this.barButtonItem5.DropDownControl = this.popupMenu1;
            this.barButtonItem5.Id = 5;
            this.barButtonItem5.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("barButtonItem5.ImageOptions.Image")));
            this.barButtonItem5.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("barButtonItem5.ImageOptions.LargeImage")));
            this.barButtonItem5.Name = "barButtonItem5";
            // 
            // popupMenu1
            // 
            this.popupMenu1.Manager = this.barManager1;
            this.popupMenu1.Name = "popupMenu1";
            // 
            // btnListInvoice
            // 
            this.btnListInvoice.Caption = "Đã phát hành";
            this.btnListInvoice.Id = 6;
            this.btnListInvoice.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnListInvoice.ImageOptions.Image")));
            this.btnListInvoice.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("btnListInvoice.ImageOptions.LargeImage")));
            this.btnListInvoice.Name = "btnListInvoice";
            this.btnListInvoice.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnListInvoice_ItemClick);
            // 
            // barButtonItem7
            // 
            this.barButtonItem7.Caption = "Phát hành hóa đơn";
            this.barButtonItem7.Id = 7;
            this.barButtonItem7.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("barButtonItem7.ImageOptions.Image")));
            this.barButtonItem7.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("barButtonItem7.ImageOptions.LargeImage")));
            this.barButtonItem7.Name = "barButtonItem7";
            // 
            // btnInvoiceUnPublish
            // 
            this.btnInvoiceUnPublish.Caption = "Hóa Đơn Chưa Phát Hành";
            this.btnInvoiceUnPublish.Id = 12;
            this.btnInvoiceUnPublish.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnInvoiceUnPublish.ImageOptions.Image")));
            this.btnInvoiceUnPublish.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("btnInvoiceUnPublish.ImageOptions.LargeImage")));
            this.btnInvoiceUnPublish.Name = "btnInvoiceUnPublish";
            // 
            // btnHome
            // 
            this.btnHome.Caption = "Chờ phát hành";
            this.btnHome.Id = 13;
            this.btnHome.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnHome.ImageOptions.Image")));
            this.btnHome.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("btnHome.ImageOptions.LargeImage")));
            this.btnHome.Name = "btnHome";
            this.btnHome.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnHome_ItemClick);
            // 
            // btnBussinessLog
            // 
            this.btnBussinessLog.Caption = "Log";
            this.btnBussinessLog.Id = 14;
            this.btnBussinessLog.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnBussinessLog.ImageOptions.Image")));
            this.btnBussinessLog.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("btnBussinessLog.ImageOptions.LargeImage")));
            this.btnBussinessLog.Name = "btnBussinessLog";
            this.btnBussinessLog.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnBussinessLog_ItemClick);
            // 
            // IconTaskBar
            // 
            this.IconTaskBar.ContextMenuStrip = this.contextMenuStrip;
            this.IconTaskBar.Icon = ((System.Drawing.Icon)(resources.GetObject("IconTaskBar.Icon")));
            this.IconTaskBar.Text = "Tools ParseData";
            this.IconTaskBar.Visible = true;
            this.IconTaskBar.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.IconTaskBar_MouseDoubleClick);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(104, 48);
            // 
            // showToolStripMenuItem
            // 
            this.showToolStripMenuItem.Name = "showToolStripMenuItem";
            this.showToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.showToolStripMenuItem.Text = "Show";
            this.showToolStripMenuItem.Click += new System.EventHandler(this.showToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // panelParent
            // 
            this.panelParent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelParent.Location = new System.Drawing.Point(0, 24);
            this.panelParent.Name = "panelParent";
            this.panelParent.Size = new System.Drawing.Size(843, 638);
            this.panelParent.TabIndex = 19;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(843, 662);
            this.Controls.Add(this.panelParent);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Thông tin hóa đơn";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu1)).EndInit();
            this.contextMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelParent)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar2;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarSubItem barSubItem1;
        private DevExpress.XtraBars.BarButtonItem btnCompanyInfo;
        private DevExpress.XtraBars.BarButtonItem btnSetup;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem2;
        private DevExpress.XtraBars.BarButtonItem barButtonItem5;
        private DevExpress.XtraBars.BarButtonItem btnListInvoice;
        private DevExpress.XtraBars.BarButtonItem barButtonItem7;
        private DevExpress.XtraBars.PopupMenu popupMenu1;
        private DevExpress.XtraBars.BarSubItem barSubItem2;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.NotifyIcon IconTaskBar;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem showToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private CustomUC.UCPaging ucPaging1;
        private DevExpress.XtraEditors.PanelControl panelParent;
        private DevExpress.XtraBars.BarButtonItem btnInvoiceUnPublish;
        private DevExpress.XtraBars.BarButtonItem btnHome;
        private DevExpress.XtraBars.BarButtonItem btnBussinessLog;
        private DevExpress.XtraBars.BarButtonItem btnProxy;
        private DevExpress.XtraBars.BarButtonItem btnUpload;
    }
}