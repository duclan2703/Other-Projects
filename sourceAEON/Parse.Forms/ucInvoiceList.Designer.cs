namespace Parse.Forms
{
    partial class ucInvoiceList
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DevExpress.XtraEditors.Controls.EditorButtonImageOptions editorButtonImageOptions1 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucInvoiceList));
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject2 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject3 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject4 = new DevExpress.Utils.SerializableAppearanceObject();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.groupListInvoice = new DevExpress.XtraEditors.GroupControl();
            this.ucPaging = new Parse.Forms.CustomUC.UCPaging();
            this.gridListInv = new DevExpress.XtraGrid.GridControl();
            this.viewListInv = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.ColId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ColSerial = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ColPattern = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ColFKey = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colBuyer = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCusName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCusCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCusAddress = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ColCusTaxCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ColTotal = new DevExpress.XtraGrid.Columns.GridColumn();
            this.txtTotal = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.ColVATRate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ColVATAmount = new DevExpress.XtraGrid.Columns.GridColumn();
            this.txtVATAmount = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.ColAmount = new DevExpress.XtraGrid.Columns.GridColumn();
            this.txtAmount = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.ColArisingDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dtArisingDate = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            this.ColConverted = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ColConvert = new DevExpress.XtraGrid.Columns.GridColumn();
            this.btnConvert = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupListInvoice)).BeginInit();
            this.groupListInvoice.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridListInv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewListInv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVATAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtArisingDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtArisingDate.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnConvert)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.labelControl1);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(753, 30);
            this.panelControl1.TabIndex = 0;
            // 
            // labelControl1
            // 
            this.labelControl1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(304, 8);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(135, 15);
            this.labelControl1.TabIndex = 1;
            this.labelControl1.Text = "DANH SÁCH HÓA ĐƠN";
            // 
            // groupListInvoice
            // 
            this.groupListInvoice.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupListInvoice.Controls.Add(this.ucPaging);
            this.groupListInvoice.Controls.Add(this.gridListInv);
            this.groupListInvoice.Location = new System.Drawing.Point(0, 30);
            this.groupListInvoice.Name = "groupListInvoice";
            this.groupListInvoice.Size = new System.Drawing.Size(753, 566);
            this.groupListInvoice.TabIndex = 21;
            this.groupListInvoice.Text = "Chi tiết";
            // 
            // ucPaging
            // 
            this.ucPaging.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ucPaging.Location = new System.Drawing.Point(3, 517);
            this.ucPaging.Name = "ucPaging";
            this.ucPaging.PageIndex = 1;
            this.ucPaging.PageSize = 30;
            this.ucPaging.Size = new System.Drawing.Size(200, 28);
            this.ucPaging.TabIndex = 1;
            this.ucPaging.Total = null;
            this.ucPaging.NextClick += new Parse.Forms.CustomUC.PagingEventHandler(this.UCPaging_Click);
            this.ucPaging.PrevClick += new Parse.Forms.CustomUC.PagingEventHandler(this.UCPaging_Click);
            this.ucPaging.FirstClick += new Parse.Forms.CustomUC.PagingEventHandler(this.UCPaging_Click);
            this.ucPaging.LastClick += new Parse.Forms.CustomUC.PagingEventHandler(this.UCPaging_Click);
            // 
            // gridListInv
            // 
            this.gridListInv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridListInv.Location = new System.Drawing.Point(2, 20);
            this.gridListInv.MainView = this.viewListInv;
            this.gridListInv.Name = "gridListInv";
            this.gridListInv.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.btnConvert,
            this.txtTotal,
            this.txtVATAmount,
            this.txtAmount,
            this.dtArisingDate});
            this.gridListInv.Size = new System.Drawing.Size(749, 544);
            this.gridListInv.TabIndex = 0;
            this.gridListInv.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.viewListInv});
            // 
            // viewListInv
            // 
            this.viewListInv.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.ColId,
            this.ColSerial,
            this.ColPattern,
            this.ColFKey,
            this.colNo,
            this.colName,
            this.colBuyer,
            this.colCusName,
            this.colCusCode,
            this.colCusAddress,
            this.ColCusTaxCode,
            this.ColTotal,
            this.ColVATRate,
            this.ColVATAmount,
            this.ColAmount,
            this.ColArisingDate,
            this.ColConverted,
            this.ColConvert});
            this.viewListInv.GridControl = this.gridListInv;
            this.viewListInv.GroupPanelText = "Kéo một cột vào đây để xem dữ liệu theo nhóm";
            this.viewListInv.Name = "viewListInv";
            this.viewListInv.OptionsFind.FindNullPrompt = "Nhập dữ liệu cần tìm...";
            this.viewListInv.OptionsView.ColumnAutoWidth = false;
            this.viewListInv.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.True;
            this.viewListInv.OptionsView.ShowFooter = true;
            // 
            // ColId
            // 
            this.ColId.AppearanceHeader.Options.UseTextOptions = true;
            this.ColId.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.ColId.Caption = "Id";
            this.ColId.FieldName = "Id";
            this.ColId.Name = "ColId";
            this.ColId.OptionsColumn.AllowEdit = false;
            this.ColId.OptionsColumn.ReadOnly = true;
            // 
            // ColSerial
            // 
            this.ColSerial.AppearanceHeader.Options.UseTextOptions = true;
            this.ColSerial.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.ColSerial.Caption = "Ký hiệu";
            this.ColSerial.FieldName = "Serial";
            this.ColSerial.Name = "ColSerial";
            this.ColSerial.OptionsColumn.AllowEdit = false;
            this.ColSerial.OptionsColumn.ReadOnly = true;
            this.ColSerial.Visible = true;
            this.ColSerial.VisibleIndex = 0;
            this.ColSerial.Width = 100;
            // 
            // ColPattern
            // 
            this.ColPattern.AppearanceHeader.Options.UseTextOptions = true;
            this.ColPattern.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.ColPattern.Caption = "Mẫu số";
            this.ColPattern.FieldName = "Pattern";
            this.ColPattern.Name = "ColPattern";
            this.ColPattern.OptionsColumn.AllowEdit = false;
            this.ColPattern.OptionsColumn.ReadOnly = true;
            this.ColPattern.Visible = true;
            this.ColPattern.VisibleIndex = 1;
            this.ColPattern.Width = 100;
            // 
            // ColFKey
            // 
            this.ColFKey.AppearanceHeader.Options.UseTextOptions = true;
            this.ColFKey.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.ColFKey.Caption = "Key";
            this.ColFKey.FieldName = "FKey";
            this.ColFKey.Name = "ColFKey";
            this.ColFKey.OptionsColumn.AllowEdit = false;
            this.ColFKey.OptionsColumn.ReadOnly = true;
            this.ColFKey.Width = 100;
            // 
            // colNo
            // 
            this.colNo.AppearanceHeader.Options.UseTextOptions = true;
            this.colNo.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colNo.Caption = "Số hóa đơn";
            this.colNo.FieldName = "No";
            this.colNo.Name = "colNo";
            this.colNo.OptionsColumn.AllowEdit = false;
            this.colNo.OptionsColumn.ReadOnly = true;
            this.colNo.Visible = true;
            this.colNo.VisibleIndex = 2;
            this.colNo.Width = 100;
            // 
            // colName
            // 
            this.colName.AppearanceHeader.Options.UseTextOptions = true;
            this.colName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colName.Caption = "Phần mềm";
            this.colName.FieldName = "Name";
            this.colName.Name = "colName";
            this.colName.OptionsColumn.AllowEdit = false;
            this.colName.OptionsColumn.ReadOnly = true;
            this.colName.Width = 100;
            // 
            // colBuyer
            // 
            this.colBuyer.AppearanceHeader.Options.UseTextOptions = true;
            this.colBuyer.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colBuyer.Caption = "Người mua hàng";
            this.colBuyer.FieldName = "Buyer";
            this.colBuyer.Name = "colBuyer";
            this.colBuyer.OptionsColumn.AllowEdit = false;
            this.colBuyer.OptionsColumn.ReadOnly = true;
            this.colBuyer.Visible = true;
            this.colBuyer.VisibleIndex = 3;
            this.colBuyer.Width = 100;
            // 
            // colCusName
            // 
            this.colCusName.AppearanceHeader.Options.UseTextOptions = true;
            this.colCusName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colCusName.Caption = "Tên đơn vị";
            this.colCusName.FieldName = "CusName";
            this.colCusName.Name = "colCusName";
            this.colCusName.OptionsColumn.AllowEdit = false;
            this.colCusName.OptionsColumn.ReadOnly = true;
            this.colCusName.Visible = true;
            this.colCusName.VisibleIndex = 4;
            this.colCusName.Width = 150;
            // 
            // colCusCode
            // 
            this.colCusCode.AppearanceHeader.Options.UseTextOptions = true;
            this.colCusCode.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colCusCode.Caption = "Mã khách hàng";
            this.colCusCode.FieldName = "CusCode";
            this.colCusCode.Name = "colCusCode";
            this.colCusCode.OptionsColumn.AllowEdit = false;
            this.colCusCode.OptionsColumn.ReadOnly = true;
            this.colCusCode.Width = 100;
            // 
            // colCusAddress
            // 
            this.colCusAddress.AppearanceHeader.Options.UseTextOptions = true;
            this.colCusAddress.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colCusAddress.Caption = "Địa chỉ";
            this.colCusAddress.FieldName = "CusAddress";
            this.colCusAddress.Name = "colCusAddress";
            this.colCusAddress.OptionsColumn.AllowEdit = false;
            this.colCusAddress.OptionsColumn.ReadOnly = true;
            this.colCusAddress.Visible = true;
            this.colCusAddress.VisibleIndex = 5;
            this.colCusAddress.Width = 250;
            // 
            // ColCusTaxCode
            // 
            this.ColCusTaxCode.AppearanceHeader.Options.UseTextOptions = true;
            this.ColCusTaxCode.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.ColCusTaxCode.Caption = "Mã số thuế";
            this.ColCusTaxCode.FieldName = "CusTaxCode";
            this.ColCusTaxCode.Name = "ColCusTaxCode";
            this.ColCusTaxCode.OptionsColumn.AllowEdit = false;
            this.ColCusTaxCode.OptionsColumn.ReadOnly = true;
            this.ColCusTaxCode.Visible = true;
            this.ColCusTaxCode.VisibleIndex = 6;
            this.ColCusTaxCode.Width = 130;
            // 
            // ColTotal
            // 
            this.ColTotal.AppearanceHeader.Options.UseTextOptions = true;
            this.ColTotal.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.ColTotal.Caption = "Cộng tiền";
            this.ColTotal.ColumnEdit = this.txtTotal;
            this.ColTotal.FieldName = "Total";
            this.ColTotal.Name = "ColTotal";
            this.ColTotal.OptionsColumn.AllowEdit = false;
            this.ColTotal.OptionsColumn.ReadOnly = true;
            this.ColTotal.Visible = true;
            this.ColTotal.VisibleIndex = 7;
            this.ColTotal.Width = 130;
            // 
            // txtTotal
            // 
            this.txtTotal.AutoHeight = false;
            this.txtTotal.DisplayFormat.FormatString = "n0";
            this.txtTotal.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtTotal.EditFormat.FormatString = "n0";
            this.txtTotal.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtTotal.Mask.EditMask = "n0";
            this.txtTotal.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtTotal.Name = "txtTotal";
            // 
            // ColVATRate
            // 
            this.ColVATRate.AppearanceHeader.Options.UseTextOptions = true;
            this.ColVATRate.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.ColVATRate.Caption = "Thuế GTGT";
            this.ColVATRate.FieldName = "VATRate";
            this.ColVATRate.Name = "ColVATRate";
            this.ColVATRate.OptionsColumn.AllowEdit = false;
            this.ColVATRate.OptionsColumn.ReadOnly = true;
            this.ColVATRate.Visible = true;
            this.ColVATRate.VisibleIndex = 8;
            this.ColVATRate.Width = 100;
            // 
            // ColVATAmount
            // 
            this.ColVATAmount.AppearanceHeader.Options.UseTextOptions = true;
            this.ColVATAmount.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.ColVATAmount.Caption = "Tiền thuế GTGT";
            this.ColVATAmount.ColumnEdit = this.txtVATAmount;
            this.ColVATAmount.FieldName = "VATAmount";
            this.ColVATAmount.Name = "ColVATAmount";
            this.ColVATAmount.OptionsColumn.AllowEdit = false;
            this.ColVATAmount.OptionsColumn.ReadOnly = true;
            this.ColVATAmount.Visible = true;
            this.ColVATAmount.VisibleIndex = 9;
            this.ColVATAmount.Width = 130;
            // 
            // txtVATAmount
            // 
            this.txtVATAmount.AutoHeight = false;
            this.txtVATAmount.DisplayFormat.FormatString = "n0";
            this.txtVATAmount.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtVATAmount.EditFormat.FormatString = "n0";
            this.txtVATAmount.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtVATAmount.Mask.EditMask = "n0";
            this.txtVATAmount.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtVATAmount.Name = "txtVATAmount";
            // 
            // ColAmount
            // 
            this.ColAmount.AppearanceHeader.Options.UseTextOptions = true;
            this.ColAmount.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.ColAmount.Caption = "Tổng tiền";
            this.ColAmount.ColumnEdit = this.txtAmount;
            this.ColAmount.FieldName = "Amount";
            this.ColAmount.Name = "ColAmount";
            this.ColAmount.OptionsColumn.AllowEdit = false;
            this.ColAmount.OptionsColumn.ReadOnly = true;
            this.ColAmount.Visible = true;
            this.ColAmount.VisibleIndex = 10;
            this.ColAmount.Width = 130;
            // 
            // txtAmount
            // 
            this.txtAmount.AutoHeight = false;
            this.txtAmount.DisplayFormat.FormatString = "n0";
            this.txtAmount.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtAmount.EditFormat.FormatString = "n0";
            this.txtAmount.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtAmount.Mask.EditMask = "n0";
            this.txtAmount.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtAmount.Name = "txtAmount";
            // 
            // ColArisingDate
            // 
            this.ColArisingDate.AppearanceHeader.Options.UseTextOptions = true;
            this.ColArisingDate.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.ColArisingDate.Caption = "Ngày hóa đơn";
            this.ColArisingDate.ColumnEdit = this.dtArisingDate;
            this.ColArisingDate.FieldName = "ArisingDate";
            this.ColArisingDate.Name = "ColArisingDate";
            this.ColArisingDate.OptionsColumn.AllowEdit = false;
            this.ColArisingDate.OptionsColumn.ReadOnly = true;
            this.ColArisingDate.Visible = true;
            this.ColArisingDate.VisibleIndex = 11;
            this.ColArisingDate.Width = 100;
            // 
            // dtArisingDate
            // 
            this.dtArisingDate.AutoHeight = false;
            this.dtArisingDate.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtArisingDate.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtArisingDate.DisplayFormat.FormatString = "dd\\/MM\\/yyyy";
            this.dtArisingDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dtArisingDate.EditFormat.FormatString = "dd\\/MM\\/yyyy";
            this.dtArisingDate.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dtArisingDate.Mask.EditMask = "dd\\/MM\\/yyyy";
            this.dtArisingDate.Mask.UseMaskAsDisplayFormat = true;
            this.dtArisingDate.Name = "dtArisingDate";
            this.dtArisingDate.NullDate = "";
            // 
            // ColConverted
            // 
            this.ColConverted.AppearanceHeader.Options.UseTextOptions = true;
            this.ColConverted.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.ColConverted.Caption = "Đã chuyển đổi";
            this.ColConverted.FieldName = "Converted";
            this.ColConverted.Name = "ColConverted";
            this.ColConverted.OptionsColumn.AllowEdit = false;
            this.ColConverted.OptionsColumn.ReadOnly = true;
            this.ColConverted.Width = 80;
            // 
            // ColConvert
            // 
            this.ColConvert.AppearanceHeader.Options.UseTextOptions = true;
            this.ColConvert.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.ColConvert.Caption = "In hóa đơn";
            this.ColConvert.ColumnEdit = this.btnConvert;
            this.ColConvert.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Right;
            this.ColConvert.Name = "ColConvert";
            this.ColConvert.Visible = true;
            this.ColConvert.VisibleIndex = 12;
            // 
            // btnConvert
            // 
            this.btnConvert.AutoHeight = false;
            editorButtonImageOptions1.Image = ((System.Drawing.Image)(resources.GetObject("editorButtonImageOptions1.Image")));
            this.btnConvert.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, editorButtonImageOptions1, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, serializableAppearanceObject2, serializableAppearanceObject3, serializableAppearanceObject4, "", null, null, DevExpress.Utils.ToolTipAnchor.Default)});
            this.btnConvert.Name = "btnConvert";
            this.btnConvert.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            this.btnConvert.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.btnConvert_ButtonClick);
            // 
            // ucInvoiceList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupListInvoice);
            this.Controls.Add(this.panelControl1);
            this.Name = "ucInvoiceList";
            this.Size = new System.Drawing.Size(753, 596);
            this.Load += new System.EventHandler(this.ucInvoiceList_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupListInvoice)).EndInit();
            this.groupListInvoice.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridListInv)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewListInv)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVATAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtArisingDate.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtArisingDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnConvert)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.GroupControl groupListInvoice;
        private CustomUC.UCPaging ucPaging;
        private DevExpress.XtraGrid.GridControl gridListInv;
        private DevExpress.XtraGrid.Views.Grid.GridView viewListInv;
        private DevExpress.XtraGrid.Columns.GridColumn ColId;
        private DevExpress.XtraGrid.Columns.GridColumn colNo;
        private DevExpress.XtraGrid.Columns.GridColumn ColSerial;
        private DevExpress.XtraGrid.Columns.GridColumn ColPattern;
        private DevExpress.XtraGrid.Columns.GridColumn ColFKey;
        private DevExpress.XtraGrid.Columns.GridColumn colName;
        private DevExpress.XtraGrid.Columns.GridColumn colCusName;
        private DevExpress.XtraGrid.Columns.GridColumn colCusCode;
        private DevExpress.XtraGrid.Columns.GridColumn colCusAddress;
        private DevExpress.XtraGrid.Columns.GridColumn ColCusTaxCode;
        private DevExpress.XtraGrid.Columns.GridColumn ColTotal;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit txtTotal;
        private DevExpress.XtraGrid.Columns.GridColumn ColVATRate;
        private DevExpress.XtraGrid.Columns.GridColumn ColVATAmount;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit txtVATAmount;
        private DevExpress.XtraGrid.Columns.GridColumn ColAmount;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit txtAmount;
        private DevExpress.XtraGrid.Columns.GridColumn ColArisingDate;
        private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit dtArisingDate;
        private DevExpress.XtraGrid.Columns.GridColumn ColConverted;
        private DevExpress.XtraGrid.Columns.GridColumn ColConvert;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit btnConvert;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraGrid.Columns.GridColumn colBuyer;
    }
}
