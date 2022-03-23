using Parse.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parse.Forms.Common
{
    public class CustomWatcher : FileSystemWatcher
    {
        public MenuModel Menu { get; set; }
    }
}
