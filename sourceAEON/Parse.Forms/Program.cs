using FX.Core;
using log4net;
using log4net.Config;
using Microsoft.Win32;
using Parse.Core;
using Parse.Core.IService;
using Parse.Forms.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Parse.Forms
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

#if !DEBUG
            string key = Application.ProductName;
            RegistryKey registry = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (registry.GetValue(key) == null)
                registry.SetValue(key, "\"" + Application.ExecutablePath.ToString() + "\"");
#endif
            bool ownmutex;
            using (Mutex mutex = new Mutex(true, Application.ProductName, out ownmutex))
            {
                if (ownmutex)
                {
                    XmlConfigurator.ConfigureAndWatch(new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "Config/logging.config"));
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Bootstrapper.InitializeContainer();
                    CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");
                    Thread.CurrentThread.CurrentCulture = culture;
                    try
                    {
                        var config = ProxyConfig.GetConfig();
                        if (config != null)
                            config.SetProxy();

                        string comCode = !string.IsNullOrEmpty(ConfigurationManager.AppSettings["COM_CODE"]) ? ConfigurationManager.AppSettings["COM_CODE"].ToLower() : null;
                        ICompanyService service = IoC.Resolve<ICompanyService>();
                        var com = service.GetCompanyByCode(comCode);
                        AppContext.InitContext(com);
                        if (com != null)
                        {
                            var publishTaskAutoStart = AppContext.Current.company.Config.ContainsKey("PUBLISH_TASK_AUTO_START") ?
                                (Convert.ToInt32(AppContext.Current.company.Config["PUBLISH_TASK_AUTO_START"]) == 1 ? true : false) : false;

                            if (publishTaskAutoStart)
                            {
                                var publishTaskDuration = AppContext.Current.company.Config.ContainsKey("PUBLISH_TASK_DURATION") ?
                                    Convert.ToInt32(AppContext.Current.company.Config["PUBLISH_TASK_DURATION"]) : 5;

                                // start task
                                AutoPublishTask.Start(publishTaskDuration);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ILog log = LogManager.GetLogger(typeof(Program));
                        log.Error(ex);
                    }

                    Application.Run(new frmMain());
                    mutex.ReleaseMutex();

                }
                else
                {
                    Application.Exit();
                }
            }
        }
    }
}
