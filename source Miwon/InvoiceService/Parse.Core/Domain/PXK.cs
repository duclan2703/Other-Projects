using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parse.Core.Domain
{
    public class PXK
    {
        public virtual int PXK_Id { get; set; }
        public virtual string Fkey { get; set; }
        public virtual DateTime CommandDate { get; set; }
        public virtual string CommandOf { get; set; }
        public virtual string CommandDescription { get; set; }
        public virtual string Transporter { get; set; }
        public virtual string ContractNo { get; set; }
        public virtual string TransportMethod { get; set; }
        public virtual string ExportAt { get; set; }
        public virtual string ImportAt { get; set; }
        public virtual decimal? Total { get; set; }
        public virtual int Type { get; set; }
        public virtual string PreFkey { get; set; }
        public virtual string TaxCode { get; set; }
        public virtual DateTime CreatedDate { get; set; }
        public virtual string Pattern { get; set; }
        public virtual string Serial { get; set; }
        public virtual string InvNo { get; set; }
        public virtual string ErrorCode { get; set; }
        public virtual string ErrorDesc { get; set; }
        public virtual int Status { get; set; }
        public virtual IList<PXK_Detail> lstDetail { get; set; }
    }
}
