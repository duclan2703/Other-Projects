using DevExpress.XtraEditors;
using log4net;
using Parse.Forms.CustomUC;
using System;
using System.Windows.Forms;

namespace Parse.Forms
{
    public partial class frmProxy : XtraForm
    {
        private readonly ILog log = LogManager.GetLogger(typeof(frmProxy));
        public frmProxy()
        {
            InitializeComponent();
        }

        private void frmProxy_Load(object sender, EventArgs e)
        {
            try
            {
                ProxyConfig config = ProxyConfig.GetConfig();
                if (config == null)
                    return;

                // proxy mode
                var proxyMode = (string)proxySetting.EditValue;
                if (config.NoneProxy)
                    proxySetting.EditValue = "0";
                else if (config.UseIEProxy)
                    proxySetting.EditValue = "1";
                else if (config.UseCustomProxy)
                    proxySetting.EditValue = "2";
                else
                    proxySetting.EditValue = "2";

                // proxy authen
                chkAuthen.Checked = config.ProxyAuthen;

                // proxy profile
                txtProxyHost.Text = config.ProxyHost;
                txtProxyPort.Text = config.ProxyPort.ToString();
                txtProxyUser.Text = config.ProxyUser;
                txtProxyPass.Text = config.ProxyPass;

            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            try
            {
                ProxyConfig config = new ProxyConfig();

                // proxy mode
                var proxyMode = (string)proxySetting.EditValue;
                if (proxyMode == "0")
                    config.NoneProxy = true;
                else if (proxyMode == "1")
                    config.UseIEProxy = true;
                else if (proxyMode == "2")
                    config.UseCustomProxy = true;

                // proxy authen
                config.ProxyAuthen = chkAuthen.Checked;

                // proxy profile
                config.ProxyHost = txtProxyHost.Text;
                config.ProxyPort = !string.IsNullOrEmpty(txtProxyPort.Text) ? Int32.Parse(txtProxyPort.Text.Trim()) : 0;
                config.ProxyUser = txtProxyUser.Text;
                config.ProxyPass = txtProxyPass.Text;

                config.SaveConfig();
                config.SetProxy();
                this.Close();
                XtraMessageBox.Show("Cập nhật thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                XtraMessageBox.Show("Lỗi:" + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void proxySetting_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                pnlProfile.Enabled = (string)proxySetting.EditValue == "2";
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }
    }
}
