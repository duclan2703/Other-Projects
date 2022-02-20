using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parse.Core.Domain
{
    public class OCRD
    {
        public virtual string CardCode { get; set; }
        public virtual string E_Mail { get; set; }
        public virtual string CardName { get; set; }
        public virtual string AliasName { get; set; }
    }
}
