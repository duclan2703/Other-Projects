using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parse.Core.Domain
{
    public class INV1
    {
        public virtual int DocEntry { get; set; }
        public virtual string ItemCode { get; set; }
        public virtual string Dscription { get; set; }
        public virtual decimal Quantity { get; set; }
        public virtual decimal Price { get; set; }
        public virtual decimal LineTotal { get; set; }
        public virtual string FreeTxt { get; set; }
        public virtual string unitMsr { get; set; }
        public virtual decimal VatPrcnt { get; set; }
    }
}
