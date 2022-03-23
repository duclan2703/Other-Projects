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
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using Parse.Core.Domain;

namespace Parse.Forms
{
    public partial class ucBussinessLog : UserControl
    {
        IBussinessLogService service = IoC.Resolve<IBussinessLogService>();
        ILogDetailService detailService = IoC.Resolve<ILogDetailService>();
        public ucBussinessLog()
        {
            InitializeComponent();
            ucPaging.PageSize = 30;
        }

        private void ucBussinessLog_Load(object sender, EventArgs e)
        {
            LoadData(ucPaging.PageIndex);
        }

        public void LoadData(int PageIndex)
        {
            var lstBussinessLog = service.GetByPaging(ucPaging.PageIndex, ucPaging.PageSize);
            gridBussinessLog.DataSource = lstBussinessLog;
            if (lstBussinessLog.Count > 0)
                gridLogDetail.DataSource = detailService.GetByLogFile(lstBussinessLog.FirstOrDefault().Id);
            ucPaging.PageIndex = PageIndex;
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

        private void viewBussinessLog_MouseDown(object sender, MouseEventArgs e)
        {
            GridHitInfo hitInfo = viewBussinessLog.CalcHitInfo(e.Location);
            if (viewBussinessLog.IsEditFormVisible && hitInfo.InRowCell && e.Clicks == 1)
            {
                viewBussinessLog.CloseEditForm();
            }
        }

        //private void btnDetails_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        //{
        //    var id = (int)(viewBussinessLog.GetRowCellValue(viewBussinessLog.FocusedRowHandle, "Id"));
        //    lstDetailLog = detailService.GetByLogFile(id);
        //    if (lstDetailLog.Count > 0)
        //    gridLogDetail.DataSource = lstDetailLog;
        //}

        private void viewBussinessLog_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            if (e.Column.FieldName == "Details")
            {
                var id = (int)(viewBussinessLog.GetRowCellValue(viewBussinessLog.FocusedRowHandle, "Id"));
                gridLogDetail.DataSource = detailService.GetByLogFile(id);
            }
            if(e.Column.FieldName == "Excel")
            {
                var id = (int)(viewBussinessLog.GetRowCellValue(viewBussinessLog.FocusedRowHandle, "Id"));

                string filter = "*.xlsx";
                SaveFileDialog.Filter = "Files (" + filter + ") | " + filter;
                SaveFileDialog.Title = "Lưu file excel mapping số hóa đơn";
                DialogResult result = SaveFileDialog.ShowDialog();
                if(result == DialogResult.OK)
                {
                    try
                    {
                        if (string.IsNullOrEmpty(SaveFileDialog.FileName))
                            XtraMessageBox.Show("Vui lòng điền tên file hoặc chọn file!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        else
                        {
                            IAEONService aeonService = IoC.Resolve<IAEONService>();
                            aeonService.PrintFileMapping(id, SaveFileDialog.FileName);
                            XtraMessageBox.Show("Xuất file thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            System.Diagnostics.Process.Start("explorer.exe", "/select," + SaveFileDialog.FileName);
                        }
                    }
                    catch(Exception ex)
                    {

                    }
                }
            }
        }
    }
}
