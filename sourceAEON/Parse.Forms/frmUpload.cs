using DevExpress.XtraEditors;
using Parse.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Parse.Forms
{
    public partial class frmUpload : XtraForm
    {
        public string path = "";
        public string pattern = "";
        public string serial = "";
        public frmUpload()
        {
            InitializeComponent();
        }

        private void frmUpload_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        public void LoadData()
        {
            var company = AppContext.Current.company;
            if (company != null)
            {
                txtPattern.Text = company.InvPattern;
                txtSerial.Text = company.InvSerial;
            }
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            string filter = "*.csv";
            openFileDialog.Filter = "Files (" + filter + ") | " + filter;
            openFileDialog.Multiselect = false;
            openFileDialog.Title = "Upload file CSV";
            DialogResult result = openFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                txtFilePath.Text = openFileDialog.FileName;
            }
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSerial.Text) || string.IsNullOrEmpty(txtPattern.Text) || string.IsNullOrEmpty(txtFilePath.Text))
            {
                XtraMessageBox.Show("Vui lòng điền đầy đủ thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                path = txtFilePath.Text;
                pattern = txtPattern.Text;
                serial = txtSerial.Text;

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
