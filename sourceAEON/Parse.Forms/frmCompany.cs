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
using Parse.Core.Domain;
using log4net;
using Parse.Core.IService;
using FX.Core;
using Parse.Core;
using System.Configuration;

namespace Parse.Forms
{
    public partial class frmCompany : DevExpress.XtraEditors.XtraForm
    {
        public Company COM = new Company();
        private readonly ILog log = LogManager.GetLogger(typeof(frmCompany));
        public frmCompany()
        {
            InitializeComponent();
        }

        private void frmCompany_Load(object sender, EventArgs e)
        {
            LoadData();
        }
        public void LoadData()
        {
            this.COM = AppContext.Current.company;
            if (this.COM != null)
            {
                txtName.Text = this.COM.Name;
                txtBankName.Text = this.COM.BankName;
                txtBankNumber.Text = this.COM.BankNumber;
                txtEmail.Text = this.COM.Email;
                txtTaxCode.Text = this.COM.TaxCode;
                txtPhone.Text = this.COM.Phone;
                txtAddress.Text = this.COM.Address;
                txtFax.Text = this.COM.Fax;
                txtDomain.Text = this.COM.Domain;
                txtUserName.Text = this.COM.UserName;
                txtPassWord.Text = this.COM.PassWord;
                txtInvPattern.Text = this.COM.InvPattern;
                txtInvSerial.Text = this.COM.InvSerial;
                chkPubInv.Checked = this.COM.Config.ContainsKey("PUBLISH_TASK_AUTO_START") ? (Convert.ToInt32(this.COM.Config["PUBLISH_TASK_AUTO_START"]) == 1 ? true : false) : false;
                txtPublishTime.Text = this.COM.Config.ContainsKey("PUBLISH_TASK_DURATION") ? this.COM.Config["PUBLISH_TASK_DURATION"].ToString() : "";
            }
        }

        public Company InnitData()
        {
            if (this.COM == null)
                this.COM = new Company();
            this.COM.InvPattern = txtInvPattern.Text;
            this.COM.InvSerial = txtInvSerial.Text;
            this.COM.Name = txtName.Text;
            this.COM.BankName = txtBankName.Text;
            this.COM.BankNumber = txtBankNumber.Text;
            this.COM.Email = txtEmail.Text;
            this.COM.TaxCode = txtTaxCode.Text;
            this.COM.Phone = txtPhone.Text;
            this.COM.Address = txtAddress.Text;
            this.COM.Fax = txtFax.Text;
            this.COM.Domain = txtDomain.Text;
            this.COM.UserName = txtUserName.Text;
            this.COM.PassWord = txtPassWord.Text;
            this.COM.Code = !string.IsNullOrEmpty(ConfigurationManager.AppSettings["COM_CODE"]) ? ConfigurationManager.AppSettings["COM_CODE"].ToLower() : null;
            this.COM.Config["PUBLISH_TASK_AUTO_START"] = chkPubInv.Checked == true ? "1" : "0";
            this.COM.Config["PUBLISH_TASK_DURATION"] = txtPublishTime.Text;
            return this.COM;
        }


        private void btnUpdate_Click(object sender, EventArgs e)
        {
            ICompanyService service = IoC.Resolve<ICompanyService>();

            service.BeginTran();
            try
            {
                this.COM = InnitData();
                if (this.COM.Id > 0)
                    service.Update(this.COM);
                else
                    service.CreateNew(this.COM);

                service.CommitTran();
                AppContext.InitContext(this.COM);
                this.Close();
            }
            catch (Exception ex)
            {
                service.RolbackTran();
                log.Error(ex);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}