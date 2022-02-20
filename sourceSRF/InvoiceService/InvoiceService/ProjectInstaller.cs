using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace InvoiceService
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();

            //Gán tên
            this.SRFInvoice.ServiceName = new CommonUtil().GetServiceName();
        }

        private void RSFInvoice_AfterInstall(object sender, InstallEventArgs e)
        {
            using (ServiceController sc = new ServiceController(new CommonUtil().GetServiceName()))
            {
                sc.Start();
            }
        }
    }
}
