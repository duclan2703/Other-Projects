using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parse.Core.Domain
{
    public class PublishResult
    {
        public virtual string status { get; set; }
        public virtual string messages { get; set; }
        public virtual object data { get; set; }
        
    }
    public class PublishResultConverted
    {
        public virtual string status { get; set; }
        public virtual string messages { get; set; }
        public virtual List<Data> data { get; set; }

    }
    public class Data
    {
        public virtual string Key { get; set; }
        public virtual string InvSerial { get; set; }
        public virtual string InvNo { get; set; }
    }
}
