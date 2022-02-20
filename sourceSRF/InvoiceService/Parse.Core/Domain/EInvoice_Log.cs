using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parse.Core.Domain
{
    public class EInvoice_Log
    {
        public virtual decimal Id { get; set; }
        public virtual int DocEntry { get; set; }
        public virtual string Serial { get; set; }
        public virtual string Pattern { get; set; }
        public virtual string InvNo { get; set; }
        public virtual DateTime IssueDate { get; set; }
        public virtual DateTime CreatedDate { get; set; }
        public virtual string ErrorCode { get; set; }
        public virtual string ErrorDesc { get; set; }
    }
}
