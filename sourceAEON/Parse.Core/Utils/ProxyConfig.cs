using System.IO;
using System.Net;

namespace System
{
    public class ProxyConfig
    {
        public const string PROXY_CONFIG = "proxy.conf";

        public bool NoneProxy { get; set; }
        public bool UseIEProxy { get; set; }
        public bool UseCustomProxy { get; set; }
        public string ProxyHost { get; set; }
        public int ProxyPort { get; set; }
        public bool ProxyAuthen { get; set; }
        public string ProxyUser { get; set; }
        public string ProxyPass { get; set; }

        public static ProxyConfig GetConfig()
        {
            string configPath = Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]) + "\\" + PROXY_CONFIG;
            if (string.IsNullOrEmpty(configPath) || !File.Exists(configPath))
                return null;

            try
            {
                System.Xml.Serialization.XmlSerializer reader = new System.Xml.Serialization.XmlSerializer(typeof(ProxyConfig));
                FileInfo fi = new FileInfo(configPath);
                using (System.IO.StreamReader file = new System.IO.StreamReader(configPath))
                {
                    ProxyConfig config = (ProxyConfig)reader.Deserialize(file);
                    return config;
                }
            }
            catch (Exception ex)
            {
                //ex.LogExceptionToFile();
                return null;
            }
        }

        public IWebProxy SetProxy()//ProxyConfig config
        {
            IWebProxy proxy = null;
            if (this.NoneProxy)
            {
                proxy = null;
            }
            else if (this.UseCustomProxy)
            {
                Uri newUri = new Uri(string.Format("http://{0}:{1}", this.ProxyHost, this.ProxyPort));
                proxy = new WebProxy(newUri);
                if (this.ProxyAuthen)
                {
                    proxy.Credentials = new NetworkCredential(this.ProxyUser, this.ProxyPass);
                }
            }
            else if (this.UseIEProxy)
            {
                // use system proxy
                proxy = WebRequest.GetSystemWebProxy();

                // var request = (HttpWebRequest)WebRequest.Create(link);
                //if (request.Proxy != null) // check system proxy
                //{
                //    var uri = request.Proxy.GetProxy(request.RequestUri);
                //    proxy = new WebProxy(uri, false);
                //    proxy.Credentials = System.Net.CredentialCache.DefaultCredentials;                    
                //}               
            }

            // set global http request proxy
            WebRequest.DefaultWebProxy = proxy;
            return proxy;
        }
        public void SaveConfig()//ProxyConfig config
        {
            string configPath = Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]) + "\\" + PROXY_CONFIG;
            if (!File.Exists(configPath))
                File.Create(configPath).Close();

            var writer = new System.Xml.Serialization.XmlSerializer(typeof(ProxyConfig));
            using (var wfile = new System.IO.StreamWriter(configPath, false))
            {
                writer.Serialize(wfile, this);
                wfile.Close();
            }
        }


    }
}