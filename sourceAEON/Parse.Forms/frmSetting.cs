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
using Parse.Core.IService;
using FX.Core;
using Parse.Core.Domain;
using log4net;
using Parse.Forms.CustomUC;
using Parse.Core;

namespace Parse.Forms
{
    public partial class frmSetting : DevExpress.XtraEditors.XtraForm
    {
        Setup entity = new Setup();
        private readonly ILog log = LogManager.GetLogger(typeof(frmSetting));
        BindingSource sourse = new BindingSource();
        frmMain Main;
        public frmSetting(frmMain main)
        {
            InitializeComponent();
            Main = main;
        }

        private void frmSetting_Load(object sender, EventArgs e)
        {
            ISetupService service = IoC.Resolve<ISetupService>();
            List<Setup> lstSetup = service.GetAll();
            if (lstSetup.Count() == 0)
            {
                CreateDefaultSetting();
                sourse.DataSource = service.GetAll();
            }
            LoadData();
        }

        public void LoadData()
        {
            ISetupService service = IoC.Resolve<ISetupService>();
            List<Setup> lstSetup = service.GetAll();
            sourse.DataSource = lstSetup;
            gridSetup.DataSource = sourse;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            viewSetup.FocusedRowHandle = -1;
            txtId.Text = "0";
            txtCode.Text = "";
            txtFilePath.Text = "";
        }

        private bool Validation()
        {
            if (txtCode.Text == "" || txtFilePath.Text == "")
                return false;
            return true;
        }
        private bool CheckExit()
        {
            ISetupService service = IoC.Resolve<ISetupService>();
            var obj = service.GetbyCode(txtCode.Text);
            if (obj != null && int.Parse(txtId.Text) == 0)
                return false;
            return true;
        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (Validation() == false)
                {
                    XtraMessageBox.Show("Trường đánh dấu (*) bắt buộc nhập", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (CheckExit() == false)
                {
                    XtraMessageBox.Show("Mã " + txtCode.Text.ToUpper() + " đã tồn tại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                ISetupService service = IoC.Resolve<ISetupService>();
                if (int.Parse(txtId.Text) > 0)
                {
                    this.entity.Code = txtCode.Text;
                    this.entity.FilePath = txtFilePath.Text;
                    service.Update(this.entity);
                }
                else
                {
                    Setup obj = new Setup();
                    obj.Code = txtCode.Text;
                    obj.FilePath = txtFilePath.Text;
                    service.CreateNew(obj);
                }

                service.CommitChanges();
                XtraMessageBox.Show("Cập nhật thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnChooseFolder_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                txtFilePath.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void viewSetup_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            this.entity = (Setup)viewSetup.GetRow(viewSetup.FocusedRowHandle);
            if (entity != null)
            {
                txtId.Text = entity.Id.ToString();
                txtCode.Text = entity.Code;
                txtFilePath.Text = entity.FilePath;
            }
        }

        private void btnDelete_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                Setup entity = (Setup)viewSetup.GetRow(viewSetup.FocusedRowHandle);
                if (entity != null)
                {
                    if (XtraMessageBox.Show("Bạn có chắc chắn muốn xóa dữ liệu này?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        ISetupService service = IoC.Resolve<ISetupService>();
                        service.Delete(entity.Id);
                        service.CommitChanges();
                        LoadData();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }

        private void CreateDefaultSetting()
        {
            ISetupService service = IoC.Resolve<ISetupService>();
            try
            {
                service.BeginTran();
                foreach (var menu in MenuModel.MenuItems)
                {
                    Setup setup = new Setup();
                    setup.Code = menu.Code;
                    setup.FilePath = @"C:\" + menu.Code;
                    service.CreateNew(setup);
                }
                service.CommitTran();
            }
            catch (Exception ex)
            {
                service.RolbackTran();
                log.Error(ex.Message);
            }
        }
    }
}