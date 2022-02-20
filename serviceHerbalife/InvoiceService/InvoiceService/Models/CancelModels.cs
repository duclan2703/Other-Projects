using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceService.Models
{
    public class CancelModels
    {
        public string codeTax { get; set; }
        public string invNo { get; set; }
        public string dateIssue { get; set; }
        public string additionalReferenceDesc { get; set; }
        public string addRefDate { get; set; }
    }
}
