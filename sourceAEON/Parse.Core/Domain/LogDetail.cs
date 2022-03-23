using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parse.Core.Domain
{
    public class LogDetail
    {
        public virtual int ID { get; set; }
        public virtual int LogId { get; set; }
        public virtual string FolioNo { get; set; }
        public virtual string Type { get; set; }
        public virtual string Detail { get; set; }
        public virtual string InvNo { get; set; }
        public virtual string Serial { get; set; }
    }
}
