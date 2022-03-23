using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parse.Core.Domain
{
    public class BussinessLog
    {
        public virtual int Id { get; set; }
        public virtual string FileName { get; set; }
        public virtual int Count { get; set; }
        public virtual DateTime CreateDate { get; set; }
        public virtual string Error { get; set; }
    }
}
