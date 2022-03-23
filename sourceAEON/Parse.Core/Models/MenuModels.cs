using log4net;
using Newtonsoft.Json;
using Parse.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parse.Core
{
    public class MenuModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Caption { get; set; }
        public string ImageUri { get; set; }
        public string ServiceName { get; set; }
        public string[] FileExtensions { get; set; }


        private static List<MenuModel> _MenuItems = new List<MenuModel>();
        public static List<MenuModel> MenuItems
        {
            get
            {
                if (_MenuItems == null || _MenuItems.Count == 0)
                {
                    var MENU_CONFIG = string.Concat(ConfigurationManager.AppSettings["COM_CODE"], ".json");
                    var configPath = Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]) + "\\Config\\" + MENU_CONFIG.ToLower();
                    if (!string.IsNullOrEmpty(configPath) && File.Exists(configPath))
                    {
                        using (StreamReader r = new StreamReader(configPath))
                        {
                            string json = r.ReadToEnd();
                            _MenuItems = JsonConvert.DeserializeObject<List<MenuModel>>(json);
                        }

                    }
                }
                return _MenuItems;
            }
        }
        public static MenuModel GetByCode(string code)
        {
            MenuModel menu = MenuItems.FirstOrDefault(x => x.Code.ToLower() == code.ToLower());
            return menu;
        }

        public static MenuModel GetById(int id)
        {
            MenuModel menu = MenuItems.FirstOrDefault(x => x.Id == id);
            return menu;
        }

    }
}
