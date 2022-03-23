using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parse.Core.Domain
{
    public class SapUpload
    {
        public virtual string Id { get; set; }
        public virtual string Pattern { get; set; }
        public virtual string Serial { get; set; }
        public virtual string Sap { get; set; }
        public virtual DateTime UpdateDate { get; set; }
    }
}
