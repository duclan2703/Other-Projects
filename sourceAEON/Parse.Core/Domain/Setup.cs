using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parse.Core.Domain
{
    public class Setup
    {
        public virtual int Id { get; set; }
        public virtual string Code { get; set; }
        public virtual string FilePath { get; set; }
    }
}
