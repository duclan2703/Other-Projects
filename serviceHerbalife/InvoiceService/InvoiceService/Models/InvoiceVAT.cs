using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceService.Models
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
        public decimal VATRate { get; set; }
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
        public virtual DateTime OrderDate { get; set; }
        public virtual string OrderMonth { get; set; }
        public virtual string OrderType { get; set; }
        //public virtual string FacturaNumber { get; set; }
        public virtual string Warehouse { get; set; }
        public virtual string FQSID { get; set; }
        public virtual string FQSName { get; set; }
        public virtual string QSID { get; set; }
        public virtual string QSName { get; set; }
        public virtual string OrderChannel { get; set; }
        public virtual string VolumePoints { get; set; }
        public virtual decimal Freight { get; set; }
        public virtual decimal TaxFreight { get; set; }
        public virtual string OriginalInvoiceId { get; set; }
        public virtual DateTime AdjustDate { get; set; }
    }
}
