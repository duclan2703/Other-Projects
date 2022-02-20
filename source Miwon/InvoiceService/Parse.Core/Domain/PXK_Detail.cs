using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parse.Core.Domain
{
    public class PXK_Detail
    {
        public virtual int PXK_Detail_Id { get; set; }
        public virtual int PXK_Id { get; set; }
        public virtual string Code { get; set; }
        public virtual string Description { get; set; }
        public virtual int? Quantity { get; set; }
        public virtual string Unit { get; set; }
        public virtual decimal? Price { get; set; }
        public virtual decimal? Total { get; set; }
    }
}
