using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parse.Core.Domain
{
    public class Discount
    {
        public virtual int Id { get; set; }
        public virtual string Description { get; set; }
        public virtual Decimal Value { get; set; }
        public virtual Decimal Total { get; set; }
    }
}
