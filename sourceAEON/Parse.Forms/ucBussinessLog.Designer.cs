namespace Parse.Forms
{
    partial class ucBussinessLog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucBussinessLog));
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject2 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject3 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject4 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.XtraEditors.Controls.EditorButtonImageOptions editorButtonImageOptions2 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject5 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject6 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject7 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject8 = new DevExpress.Utils.SerializableAppearanceObject();
            this.dtCreateDate = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            this.memoError = new DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit();
            this.btnDetails = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.gridBussinessLog = new DevExpress.XtraGrid.GridControl();
            this.viewBussinessLog = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colFileName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCount = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCreateDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colError = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDetails = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colExcel = new DevExpress.XtraGrid.Columns.GridColumn();
            this.btnExcel = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.ucPaging = new Parse.Forms.CustomUC.UCPaging();
            this.gridLogDetail = new DevExpress.XtraGrid.GridControl();
            this.viewLogDetail = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colFolio = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colInvNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSerial = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDetail = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.SaveFileDialog = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.dtCreateDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtCreateDate.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoError)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnDetails)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridBussinessLog)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewBussinessLog)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnExcel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridLogDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewLogDetail)).BeginInit();
            this.SuspendLayout();
            // 
            // dtCreateDate
            // 
            this.dtCreateDate.AutoHeight = false;
            this.dtCreateDate.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph)});
            this.dtCreateDate.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtCreateDate.DisplayFormat.FormatString = "dd\\/MM\\/yyyy";
            this.dtCreateDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dtCreateDate.EditFormat.FormatString = "dd\\/MM\\/yyyy";
            this.dtCreateDate.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dtCreateDate.Mask.EditMask = "dd\\/MM\\/yyyy";
            this.dtCreateDate.Mask.UseMaskAsDisplayFormat = true;
            this.dtCreateDate.Name = "dtCreateDate";
            this.dtCreateDate.NullDate = "";
            // 
            // memoError
            // 
            this.memoError.Name = "memoError";
            // 
            // btnDetails
            // 
            this.btnDetails.AutoHeight = false;
            editorButtonImageOptions1.Image = ((System.Drawing.Image)(resources.GetObject("editorButtonImageOptions1.Image")));
            this.btnDetails.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, editorButtonImageOptions1, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, serializableAppearanceObject2, serializableAppearanceObject3, serializableAppearanceObject4, "", null, null, DevExpress.Utils.ToolTipAnchor.Default)});
            this.btnDetails.Name = "btnDetails";
            this.btnDetails.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            // 
            // panelControl1
            // 
            this.panelControl1.AutoSize = true;
            this.panelControl1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelControl1.Controls.Add(this.labelControl1);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1016, 29);
            this.panelControl1.TabIndex = 0;
            // 
            // labelControl1
            // 
            this.labelControl1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(455, 8);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(87, 14);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "Lịch sử hóa đơn";
            // 
            // gridBussinessLog
            // 
            this.gridBussinessLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridBussinessLog.Location = new System.Drawing.Point(0, 30);
            this.gridBussinessLog.MainView = this.viewBussinessLog;
            this.gridBussinessLog.Name = "gridBussinessLog";
            this.gridBussinessLog.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.btnExcel});
            this.gridBussinessLog.Size = new System.Drawing.Size(550, 558);
            this.gridBussinessLog.TabIndex = 1;
            this.gridBussinessLog.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.viewBussinessLog});
            // 
            // viewBussinessLog
            // 
            this.viewBussinessLog.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colId,
            this.colFileName,
            this.colCount,
            this.colCreateDate,
            this.colError,
            this.colDetails,
            this.colExcel});
            this.viewBussinessLog.GridControl = this.gridBussinessLog;
            this.viewBussinessLog.GroupPanelText = "Kéo một cột vào đây để xem dữ liệu theo nhóm";
            this.viewBussinessLog.Name = "viewBussinessLog";
            this.viewBussinessLog.OptionsBehavior.EditingMode = DevExpress.XtraGrid.Views.Grid.GridEditingMode.EditFormInplace;
            this.viewBussinessLog.OptionsEditForm.ActionOnModifiedRowChange = DevExpress.XtraGrid.Views.Grid.EditFormModifiedAction.Nothing;
            this.viewBussinessLog.OptionsEditForm.EditFormColumnCount = 2;
            this.viewBussinessLog.OptionsEditForm.ShowOnDoubleClick = DevExpress.Utils.DefaultBoolean.False;
            this.viewBussinessLog.OptionsEditForm.ShowUpdateCancelPanel = DevExpress.Utils.DefaultBoolean.False;
            this.viewBussinessLog.OptionsFind.FindNullPrompt = "Nhập dữ liệu cần tìm...";
            this.viewBussinessLog.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.True;
            this.viewBussinessLog.OptionsView.ShowFooter = true;
            this.viewBussinessLog.RowCellClick += new DevExpress.XtraGrid.Views.Grid.RowCellClickEventHandler(this.viewBussinessLog_RowCellClick);
            this.viewBussinessLog.MouseDown += new System.Windows.Forms.MouseEventHandler(this.viewBussinessLog_MouseDown);
            // 
            // colId
            // 
            this.colId.AppearanceHeader.Options.UseTextOptions = true;
            this.colId.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colId.Caption = "Id";
            this.colId.FieldName = "Id";
            this.colId.Name = "colId";
            this.colId.OptionsColumn.AllowEdit = false;
            this.colId.OptionsColumn.ReadOnly = true;
            // 
            // colFileName
            // 
            this.colFileName.AppearanceHeader.Options.UseTextOptions = true;
            this.colFileName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colFileName.Caption = "Tên file";
            this.colFileName.FieldName = "FileName";
            this.colFileName.Name = "colFileName";
            this.colFileName.OptionsColumn.AllowEdit = false;
            this.colFileName.OptionsColumn.ReadOnly = true;
            this.colFileName.OptionsEditForm.ColumnSpan = 2;
            this.colFileName.OptionsEditForm.UseEditorColRowSpan = false;
            this.colFileName.Visible = true;
            this.colFileName.VisibleIndex = 0;
            this.colFileName.Width = 20;
            // 
            // colCount
            // 
            this.colCount.AppearanceCell.Options.UseTextOptions = true;
            this.colCount.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colCount.AppearanceHeader.Options.UseTextOptions = true;
            this.colCount.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colCount.Caption = "Số lượng hóa đơn";
            this.colCount.FieldName = "Count";
            this.colCount.Name = "colCount";
            this.colCount.OptionsColumn.AllowEdit = false;
            this.colCount.OptionsColumn.FixedWidth = true;
            this.colCount.OptionsColumn.ReadOnly = true;
            this.colCount.Visible = true;
            this.colCount.VisibleIndex = 2;
            this.colCount.Width = 120;
            // 
            // colCreateDate
            // 
            this.colCreateDate.AppearanceCell.Options.UseTextOptions = true;
            this.colCreateDate.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colCreateDate.AppearanceHeader.Options.UseTextOptions = true;
            this.colCreateDate.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colCreateDate.Caption = "Ngày tạo";
            this.colCreateDate.ColumnEdit = this.dtCreateDate;
            this.colCreateDate.FieldName = "CreateDate";
            this.colCreateDate.Name = "colCreateDate";
            this.colCreateDate.OptionsColumn.AllowEdit = false;
            this.colCreateDate.OptionsColumn.FixedWidth = true;
            this.colCreateDate.OptionsColumn.ReadOnly = true;
            this.colCreateDate.Visible = true;
            this.colCreateDate.VisibleIndex = 1;
            this.colCreateDate.Width = 100;
            // 
            // colError
            // 
            this.colError.AppearanceHeader.Options.UseTextOptions = true;
            this.colError.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colError.Caption = "Nội dung";
            this.colError.ColumnEdit = this.memoError;
            this.colError.FieldName = "Error";
            this.colError.Name = "colError";
            this.colError.OptionsColumn.AllowEdit = false;
            this.colError.OptionsColumn.ReadOnly = true;
            this.colError.OptionsEditForm.RowSpan = 4;
            this.colError.Visible = true;
            this.colError.VisibleIndex = 3;
            this.colError.Width = 200;
            // 
            // colDetails
            // 
            this.colDetails.AppearanceHeader.Options.UseTextOptions = true;
            this.colDetails.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colDetails.Caption = "Chi tiết";
            this.colDetails.ColumnEdit = this.btnDetails;
            this.colDetails.FieldName = "Details";
            this.colDetails.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Right;
            this.colDetails.Name = "colDetails";
            this.colDetails.OptionsColumn.FixedWidth = true;
            this.colDetails.Visible = true;
            this.colDetails.VisibleIndex = 5;
            this.colDetails.Width = 70;
            // 
            // colExcel
            // 
            this.colExcel.Caption = "In file số HĐ";
            this.colExcel.ColumnEdit = this.btnExcel;
            this.colExcel.FieldName = "Excel";
            this.colExcel.Name = "colExcel";
            this.colExcel.OptionsColumn.FixedWidth = true;
            this.colExcel.Visible = true;
            this.colExcel.VisibleIndex = 4;
            this.colExcel.Width = 50;
            // 
            // btnExcel
            // 
            this.btnExcel.AutoHeight = false;
            editorButtonImageOptions2.Image = ((System.Drawing.Image)(resources.GetObject("editorButtonImageOptions2.Image")));
            this.btnExcel.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, editorButtonImageOptions2, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject5, serializableAppearanceObject6, serializableAppearanceObject7, serializableAppearanceObject8, "", null, null, DevExpress.Utils.ToolTipAnchor.Default)});
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            // 
            // ucPaging
            // 
            this.ucPaging.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ucPaging.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.ucPaging.Location = new System.Drawing.Point(3, 557);
            this.ucPaging.Name = "ucPaging";
            this.ucPaging.PageIndex = 1;
            this.ucPaging.PageSize = 30;
            this.ucPaging.Size = new System.Drawing.Size(200, 28);
            this.ucPaging.TabIndex = 3;
            this.ucPaging.Total = null;
            this.ucPaging.NextClick += new Parse.Forms.CustomUC.PagingEventHandler(this.UCPaging_Click);
            this.ucPaging.PrevClick += new Parse.Forms.CustomUC.PagingEventHandler(this.UCPaging_Click);
            this.ucPaging.FirstClick += new Parse.Forms.CustomUC.PagingEventHandler(this.UCPaging_Click);
            this.ucPaging.LastClick += new Parse.Forms.CustomUC.PagingEventHandler(this.UCPaging_Click);
            // 
            // gridLogDetail
            // 
            this.gridLogDetail.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridLogDetail.Location = new System.Drawing.Point(556, 30);
            this.gridLogDetail.MainView = this.viewLogDetail;
            this.gridLogDetail.Name = "gridLogDetail";
            this.gridLogDetail.Size = new System.Drawing.Size(460, 559);
            this.gridLogDetail.TabIndex = 4;
            this.gridLogDetail.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.viewLogDetail});
            // 
            // viewLogDetail
            // 
            this.viewLogDetail.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colFolio,
            this.colInvNo,
            this.colSerial,
            this.colDetail,
            this.colType});
            this.viewLogDetail.GridControl = this.gridLogDetail;
            this.viewLogDetail.Name = "viewLogDetail";
            // 
            // colFolio
            // 
            this.colFolio.Caption = "Số hóa đơn";
            this.colFolio.FieldName = "FolioNo";
            this.colFolio.Name = "colFolio";
            this.colFolio.OptionsColumn.FixedWidth = true;
            this.colFolio.Visible = true;
            this.colFolio.VisibleIndex = 1;
            this.colFolio.Width = 60;
            // 
            // colInvNo
            // 
            this.colInvNo.Caption = "Số HĐ trên SInvoice";
            this.colInvNo.FieldName = "InvNo";
            this.colInvNo.Name = "colInvNo";
            this.colInvNo.OptionsColumn.FixedWidth = true;
            this.colInvNo.Visible = true;
            this.colInvNo.VisibleIndex = 2;
            this.colInvNo.Width = 100;
            // 
            // colSerial
            // 
            this.colSerial.Caption = "Serial";
            this.colSerial.FieldName = "Serial";
            this.colSerial.Name = "colSerial";
            this.colSerial.OptionsColumn.FixedWidth = true;
            this.colSerial.Visible = true;
            this.colSerial.VisibleIndex = 3;
            this.colSerial.Width = 58;
            // 
            // colDetail
            // 
            this.colDetail.Caption = "Nội dung";
            this.colDetail.FieldName = "Detail";
            this.colDetail.Name = "colDetail";
            this.colDetail.Visible = true;
            this.colDetail.VisibleIndex = 4;
            this.colDetail.Width = 194;
            // 
            // colType
            // 
            this.colType.Caption = "Loại";
            this.colType.FieldName = "Type";
            this.colType.Name = "colType";
            this.colType.OptionsColumn.FixedWidth = true;
            this.colType.Visible = true;
            this.colType.VisibleIndex = 0;
            this.colType.Width = 70;
            // 
            // ucBussinessLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gridLogDetail);
            this.Controls.Add(this.ucPaging);
            this.Controls.Add(this.gridBussinessLog);
            this.Controls.Add(this.panelControl1);
            this.Name = "ucBussinessLog";
            this.Size = new System.Drawing.Size(1016, 588);
            this.Load += new System.EventHandler(this.ucBussinessLog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dtCreateDate.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtCreateDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoError)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnDetails)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridBussinessLog)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewBussinessLog)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnExcel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridLogDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewLogDetail)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraGrid.GridControl gridBussinessLog;
        private DevExpress.XtraGrid.Views.Grid.GridView viewBussinessLog;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraGrid.Columns.GridColumn colId;
        private DevExpress.XtraGrid.Columns.GridColumn colFileName;
        private DevExpress.XtraGrid.Columns.GridColumn colCount;
        private DevExpress.XtraGrid.Columns.GridColumn colCreateDate;
        private DevExpress.XtraGrid.Columns.GridColumn colError;
        private CustomUC.UCPaging ucPaging;
        private DevExpress.XtraGrid.GridControl gridLogDetail;
        private DevExpress.XtraGrid.Views.Grid.GridView viewLogDetail;
        private DevExpress.XtraGrid.Columns.GridColumn colFolio;
        private DevExpress.XtraGrid.Columns.GridColumn colDetail;
        private DevExpress.XtraGrid.Columns.GridColumn colDetails;
        private DevExpress.XtraGrid.Columns.GridColumn colType;
        private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit dtCreateDate;
        private DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit memoError;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit btnDetails;
        private DevExpress.XtraGrid.Columns.GridColumn colInvNo;
        private DevExpress.XtraGrid.Columns.GridColumn colSerial;
        private DevExpress.XtraGrid.Columns.GridColumn colExcel;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit btnExcel;
        private System.Windows.Forms.SaveFileDialog SaveFileDialog;
    }
}
