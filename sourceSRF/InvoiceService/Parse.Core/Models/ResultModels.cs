using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parse.Core.Models
{
    public class APIResults
    {
        public List<APIResult> createInvoiceOutputs { get; set; }
    }
    public class APIResult
    {
        public string transactionUuid { get; set; }
        public string errorCode { get; set; }
        public string description { get; set; }
        public InvoiceResult result { get; set; }
    }

    public class InvoiceResult
    {
        public string supplierTaxCode { get; set; }
        public string invoiceNo { get; set; }
        public string transactionID { get; set; }
        public string reservationCode { get; set; }
    }
}
