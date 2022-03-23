namespace Parse.Forms
{
    partial class frmProxy
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
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmProxy));
            this.proxySetting = new DevExpress.XtraEditors.RadioGroup();
            this.pnlProfile = new DevExpress.XtraEditors.GroupControl();
            this.txtProxyPass = new DevExpress.XtraEditors.TextEdit();
            this.txtProxyUser = new DevExpress.XtraEditors.TextEdit();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.chkAuthen = new DevExpress.XtraEditors.CheckEdit();
            this.txtProxyPort = new DevExpress.XtraEditors.TextEdit();
            this.txtProxyHost = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.btnApply = new DevExpress.XtraEditors.SimpleButton();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.proxySetting.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlProfile)).BeginInit();
            this.pnlProfile.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtProxyPass.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtProxyUser.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkAuthen.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtProxyPort.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtProxyHost.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // proxySetting
            // 
            this.proxySetting.EditValue = "0";
            this.proxySetting.Location = new System.Drawing.Point(12, 3);
            this.proxySetting.Name = "proxySetting";
            this.proxySetting.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.proxySetting.Properties.Appearance.Options.UseBackColor = true;
            this.proxySetting.Properties.Columns = 3;
            this.proxySetting.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem("0", "Không sử dụng"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("1", "Hệ thống"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("2", "Cấu hình proxy")});
            this.proxySetting.Size = new System.Drawing.Size(361, 35);
            this.proxySetting.TabIndex = 1;
            this.proxySetting.EditValueChanged += new System.EventHandler(this.proxySetting_EditValueChanged);
            // 
            // pnlProfile
            // 
            this.pnlProfile.Controls.Add(this.txtProxyPass);
            this.pnlProfile.Controls.Add(this.txtProxyUser);
            this.pnlProfile.Controls.Add(this.labelControl5);
            this.pnlProfile.Controls.Add(this.labelControl4);
            this.pnlProfile.Controls.Add(this.labelControl3);
            this.pnlProfile.Controls.Add(this.chkAuthen);
            this.pnlProfile.Controls.Add(this.txtProxyPort);
            this.pnlProfile.Controls.Add(this.txtProxyHost);
            this.pnlProfile.Controls.Add(this.labelControl2);
            this.pnlProfile.Controls.Add(this.labelControl1);
            this.pnlProfile.Enabled = false;
            this.pnlProfile.Location = new System.Drawing.Point(12, 55);
            this.pnlProfile.Name = "pnlProfile";
            this.pnlProfile.Size = new System.Drawing.Size(361, 164);
            this.pnlProfile.TabIndex = 2;
            this.pnlProfile.Text = "Cấu hình proxy server";
            // 
            // txtProxyPass
            // 
            this.txtProxyPass.Location = new System.Drawing.Point(89, 126);
            this.txtProxyPass.Name = "txtProxyPass";
            this.txtProxyPass.Properties.PasswordChar = '*';
            this.txtProxyPass.Properties.UseSystemPasswordChar = true;
            this.txtProxyPass.Size = new System.Drawing.Size(251, 20);
            this.txtProxyPass.TabIndex = 9;
            // 
            // txtProxyUser
            // 
            this.txtProxyUser.Location = new System.Drawing.Point(89, 91);
            this.txtProxyUser.Name = "txtProxyUser";
            this.txtProxyUser.Size = new System.Drawing.Size(251, 20);
            this.txtProxyUser.TabIndex = 8;
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(38, 129);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(44, 13);
            this.labelControl5.TabIndex = 7;
            this.labelControl5.Text = "Mật khẩu";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(10, 93);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(72, 13);
            this.labelControl4.TabIndex = 6;
            this.labelControl4.Text = "Tên đăng nhập";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(112, 72);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(91, 13);
            this.labelControl3.TabIndex = 5;
            this.labelControl3.Text = "Cấu hình Tài khoản";
            // 
            // chkAuthen
            // 
            this.chkAuthen.Location = new System.Drawing.Point(89, 68);
            this.chkAuthen.Name = "chkAuthen";
            this.chkAuthen.Properties.Caption = "";
            this.chkAuthen.Size = new System.Drawing.Size(17, 19);
            this.chkAuthen.TabIndex = 4;
            // 
            // txtProxyPort
            // 
            this.txtProxyPort.Location = new System.Drawing.Point(274, 32);
            this.txtProxyPort.Name = "txtProxyPort";
            this.txtProxyPort.Properties.DisplayFormat.FormatString = "d";
            this.txtProxyPort.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtProxyPort.Properties.EditFormat.FormatString = "d";
            this.txtProxyPort.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtProxyPort.Properties.Mask.EditMask = "d";
            this.txtProxyPort.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtProxyPort.Properties.MaxLength = 4;
            this.txtProxyPort.Size = new System.Drawing.Size(66, 20);
            this.txtProxyPort.TabIndex = 3;
            // 
            // txtProxyHost
            // 
            this.txtProxyHost.Location = new System.Drawing.Point(88, 32);
            this.txtProxyHost.Name = "txtProxyHost";
            this.txtProxyHost.Size = new System.Drawing.Size(144, 20);
            this.txtProxyHost.TabIndex = 2;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(243, 35);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(25, 13);
            this.labelControl2.TabIndex = 1;
            this.labelControl2.Text = "Cổng";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(50, 35);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(32, 13);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "Địa chỉ";
            // 
            // btnApply
            // 
            this.btnApply.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnApply.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnApply.ImageOptions.Image")));
            this.btnApply.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.btnApply.Location = new System.Drawing.Point(115, 235);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(75, 23);
            this.btnApply.TabIndex = 14;
            this.btnApply.Text = "Áp dụng";
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancel.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.ImageOptions.Image")));
            this.btnCancel.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(202, 235);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 15;
            this.btnCancel.Text = "Thoát";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // frmProxy
            // 
            this.ClientSize = new System.Drawing.Size(385, 271);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.pnlProfile);
            this.Controls.Add(this.proxySetting);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmProxy";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Cấu hình Proxy";
            this.Load += new System.EventHandler(this.frmProxy_Load);
            ((System.ComponentModel.ISupportInitialize)(this.proxySetting.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlProfile)).EndInit();
            this.pnlProfile.ResumeLayout(false);
            this.pnlProfile.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtProxyPass.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtProxyUser.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkAuthen.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtProxyPort.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtProxyHost.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraBars.Ribbon.RibbonStatusBar ribbonStatusBar1;
        private DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl1;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
        private DevExpress.XtraEditors.RadioGroup proxySetting;
        private DevExpress.XtraEditors.GroupControl pnlProfile;
        private DevExpress.XtraEditors.TextEdit txtProxyPort;
        private DevExpress.XtraEditors.TextEdit txtProxyHost;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit txtProxyPass;
        private DevExpress.XtraEditors.TextEdit txtProxyUser;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.CheckEdit chkAuthen;
        private DevExpress.XtraEditors.SimpleButton btnApply;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
    }
}