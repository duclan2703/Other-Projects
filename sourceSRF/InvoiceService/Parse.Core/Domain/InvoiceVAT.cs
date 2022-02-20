using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parse.Core.Domain
{
    public class InvoiceVAT
    {
        public string No { get; set; }
        public string Serial { get; set; }
        public string Pattern { get; set; }

        public string Name { get; set; }
        public string ComName { get; set; }
        public string ComPhone { get; set; }
        public string ComAddress { get; set; }
        public string ComTaxCode { get; set; }
        public string ComBankName { get; set; }
        public string ComBankNo { get; set; }
        public string ComFax { get; set; }

        public string CusName { get; set; }
        public string CusCode { get; set; }
        public string CusPhone { get; set; }
        public string CusAddress { get; set; }
        public string CusTaxCode { get; set; }
        public string CusBankName { get; set; }
        public string CusBankNo { get; set; }

        public int PaymentStatus { get; set; }
        public string PaymentMethod { get; set; }
        public string CreateBy { get; set; }
        public string Note { get; set; }
        public string ProcessInvNote { get; set; }
        public decimal Total { get; set; }
        public float VATRate { get; set; }
        public decimal VATAmount { get; set; }
        public decimal Amount { get; set; }
        public string SignDate { get; set; }
        public DateTime ArisingDate { get; set; }
        public string AmountInWords { get; set; }
        public int? Type { get; set; }
        public int? Status { get; set; }
        public int? Paymentstatus { get; set; }
        public List<ProductInv> Products { get; set; }
        public string SapNumber { get; set; }
        public string Fkey { get; set; }
        public string Buyer { get; set; }
        public string CusEmail { get; set; }
        public decimal TotalNo { get; set; }
        public decimal VATAmountNo { get; set; }
        public decimal AmountNo { get; set; }
        public decimal Total0 { get; set; }
        public decimal VATAmount0 { get; set; }
        public decimal Amount0 { get; set; }
        public decimal Total5 { get; set; }
        public decimal VATAmount5 { get; set; }
        public decimal Amount5 { get; set; }
        public decimal Total10 { get; set; }
        public decimal VATAmount10 { get; set; }
        public decimal Amount10 { get; set; }
        public string UGINo { get; set; }
        public decimal DiscountAmount { get; set; }
        public string U_DeductType { get; set; }
        public string TrackNo { get; set; }
        public string SaleOrder { get; set; }
    }
}
