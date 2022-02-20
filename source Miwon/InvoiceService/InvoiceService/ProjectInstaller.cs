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
            this.MiwonInvoice.ServiceName = new CommonUtil().GetServiceName();
        }

        private void MiwonInvoice_AfterInstall(object sender, InstallEventArgs e)
        {
            using (ServiceController sc = new ServiceController(new CommonUtil().GetServiceName()))
            {
                sc.Start();
            }
        }
    }
}
