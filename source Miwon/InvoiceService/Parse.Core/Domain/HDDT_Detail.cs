using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parse.Core.Domain
{
    public class HDDT_Detail
    {
        public virtual int HDDT_Detail_Id { get; set; }
        public virtual int HDDT_Id { get; set; }
        public virtual string Code { get; set; }
        public virtual string Description { get; set; }
        public virtual int? Quantity { get; set; }
        public virtual string Unit { get; set; }
        public virtual decimal? Price { get; set; }
        public virtual decimal? Total { get; set; }
        public virtual decimal? VatRate { get; set; }
        public virtual decimal? VatAmount { get; set; }
        public virtual decimal? TotalAmount { get; set; }
    }
}
