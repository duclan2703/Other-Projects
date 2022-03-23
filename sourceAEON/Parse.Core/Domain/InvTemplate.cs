using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parse.Core.Domain
{
    public class InvTemplate
    {
        public string ArisingDate { get; set; }

        public string ComName { get; set; }
        public string ComTaxCode { get; set; }
        public string ComAddress { get; set; }
        public string ComBankNo { get; set; }
        public string ComPhone { get; set; }

        public string CusName { get; set; }
        public string CusTaxCode { get; set; }
        public string CusPhone { get; set; }
        public string CusAddress { get; set; }
        public string PaymentMethod { get; set; }
        public string CusBankNo { get; set; }
        //public string CusBankName { get; set; }

        public string ProdPattern { get; set; }
        public string DiscountRate { get; set; }
        public string DiscountAmount { get; set; }
        public string Total { get; set; }
        public string VATRate { get; set; }
        public string VATAmount { get; set; }
        public string Amount { get; set; }
        public string AmountInWords { get; set; }
    }
}
