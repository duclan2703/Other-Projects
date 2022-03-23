namespace Parse.Forms
{
    partial class frmInvocieView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmInvocieView));
            this.groupInvoiceDetail = new DevExpress.XtraEditors.GroupControl();
            this.invBrowser = new System.Windows.Forms.WebBrowser();
            this.btnPrint = new DevExpress.XtraEditors.SimpleButton();
            this.btnPublishInvoice = new DevExpress.XtraEditors.SimpleButton();
            this.btnEditInvocie = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.groupInvoiceDetail)).BeginInit();
            this.groupInvoiceDetail.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupInvoiceDetail
            // 
            this.groupInvoiceDetail.Controls.Add(this.invBrowser);
            this.groupInvoiceDetail.Location = new System.Drawing.Point(0, 1);
            this.groupInvoiceDetail.Name = "groupInvoiceDetail";
            this.groupInvoiceDetail.Size = new System.Drawing.Size(836, 619);
            this.groupInvoiceDetail.TabIndex = 6;
            // 
            // invBrowser
            // 
            this.invBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.invBrowser.Location = new System.Drawing.Point(2, 20);
            this.invBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.invBrowser.Name = "invBrowser";
            this.invBrowser.Size = new System.Drawing.Size(832, 597);
            this.invBrowser.TabIndex = 0;
            this.invBrowser.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.invBrowser_DocumentCompleted);
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrint.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPrint.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnPrint.ImageOptions.Image")));
            this.btnPrint.Location = new System.Drawing.Point(446, 626);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(95, 23);
            this.btnPrint.TabIndex = 19;
            this.btnPrint.Text = "In hóa đơn";
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnPublishInvoice
            // 
            this.btnPublishInvoice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPublishInvoice.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPublishInvoice.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnPublishInvoice.ImageOptions.Image")));
            this.btnPublishInvoice.Location = new System.Drawing.Point(211, 626);
            this.btnPublishInvoice.Name = "btnPublishInvoice";
            this.btnPublishInvoice.Size = new System.Drawing.Size(128, 23);
            this.btnPublishInvoice.TabIndex = 20;
            this.btnPublishInvoice.Text = "Phát hành hóa đơn";
            this.btnPublishInvoice.Visible = false;
            this.btnPublishInvoice.Click += new System.EventHandler(this.btnPublishInvoice_Click);
            // 
            // btnEditInvocie
            // 
            this.btnEditInvocie.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEditInvocie.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnEditInvocie.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnEditInvocie.ImageOptions.Image")));
            this.btnEditInvocie.Location = new System.Drawing.Point(345, 626);
            this.btnEditInvocie.Name = "btnEditInvocie";
            this.btnEditInvocie.Size = new System.Drawing.Size(95, 23);
            this.btnEditInvocie.TabIndex = 21;
            this.btnEditInvocie.Text = "Sửa hóa đơn";
            this.btnEditInvocie.Click += new System.EventHandler(this.btnEditInvocie_Click);
            // 
            // frmInvocieView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(836, 655);
            this.Controls.Add(this.btnEditInvocie);
            this.Controls.Add(this.btnPublishInvoice);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.groupInvoiceDetail);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmInvocieView";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Thông tin chi tiết hóa đơn";
            ((System.ComponentModel.ISupportInitialize)(this.groupInvoiceDetail)).EndInit();
            this.groupInvoiceDetail.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupInvoiceDetail;
        private System.Windows.Forms.WebBrowser invBrowser;
        private DevExpress.XtraEditors.SimpleButton btnPrint;
        private DevExpress.XtraEditors.SimpleButton btnPublishInvoice;
        private DevExpress.XtraEditors.SimpleButton btnEditInvocie;
    }
}