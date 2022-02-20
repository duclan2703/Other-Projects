using Parse.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parse.Core.Models
{
    public class DataModel
    {
        public OINV invoice { get; set; }
        public OCRD ocrd { get; set; }

        public DataModel(OINV invoice, OCRD ocrd)
        {
            this.invoice = invoice;
            this.ocrd = ocrd;
        }
    }
}
