using log4net;
using Parse.Core.ServiceImps;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace InvoiceService
{
    public partial class Service1 : ServiceBase
    {
        private Timer timer = null;
        ILog log = LogManager.GetLogger(typeof(Service1));
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            // Tạo 1 timer từ libary System.Timers
            timer = new Timer();
            // Set thời gian quét
            double waitTime;
            if (double.TryParse(ConfigurationManager.AppSettings["WaitTime"], out waitTime))
                timer.Interval = waitTime * 60000;
            else
                timer.Interval = 180000;
            // Enable timer
            timer.Enabled = true;
            timer.Elapsed += timer_Tick;
        }
        private void timer_Tick(object sender, ElapsedEventArgs args)
        {
            try
            {
                timer.Stop();
                MiwonService srv = new MiwonService();
                srv.Processing();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            finally
            {
                timer.Start();
            }
        }

        protected override void OnStop()
        {
            timer.Enabled = true;
            timer.Stop();
        }
    }
}
