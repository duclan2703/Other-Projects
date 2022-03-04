using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceService.Models
{
    public class Warehouse
    {
        public List<Mapping> WarehouseMapping { get; set; }
    }

    public class Mapping
    {
        public List<string> Warehouse { get; set; }
        public string Taxcode { get; set; }
        public string Pattern { get; set; }
        public string Serial { get; set; }
        public string ResendPattern { get; set; }
        public string ResendSerial { get; set; }
        public int TT78 { get; set; }
    }
}
