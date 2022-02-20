using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parse.Core.Domain
{
    public class OINV
    {
        public virtual int DocEntry { get; set; }
        public virtual char CANCELED { get; set; }
        public virtual DateTime DocDate { get; set; }
        public virtual string CardCode { get; set; }
        public virtual string Address { get; set; }
        public virtual string Comments { get; set; }
        public virtual int GroupNum { get; set; }
        public virtual string LicTradNum { get; set; }
        public virtual string TrackNo { get; set; }
        public virtual string U_GINo { get; set; }
        public virtual string U_DeductType { get; set; }
        public virtual decimal? DiscSum { get; set; }
    }
}
