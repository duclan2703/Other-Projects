using System.Collections;
using System.Collections.Generic;
using System;

namespace Parse.Core.Domain
{
    public interface Invoice
    {
        string No { get; set; }
        string Serial { get; set; }
        string Pattern { get; set; }

        string Name { get; set; }
        string ComName { get; set; }
        string ComPhone { get; set; }
        string ComAddress { get; set; }
        string ComTaxCode { get; set; }
        string ComBankName { get; set; }
        string ComBankNo { get; set; }
        string ComFax { get; set; }

        string CusName { get; set; }
        string CusCode { get; set; }
        string CusPhone { get; set; }
        string CusAddress { get; set; }
        string CusTaxCode { get; set; }
        string CusBankName { get; set; }
        string CusBankNo { get; set; }

        int PaymentStatus { get; set; }
        string PaymentMethod { get; set; }
        string CreateBy { get; set; }
        string Note { get; set; }
        string ProcessInvNote { get; set; }
        decimal Total { get; set; }
        float VATRate { get; set; }
        decimal VATAmount { get; set; }
        decimal Amount { get; set; }   
        string SignDate { get; set; }
        DateTime ArisingDate { get; set; }        
        string AmountInWords { get; set; }
        int? Type { get; set; }
        int? Status { get; set; }
        int? Paymentstatus { get; set; }
        IList<ProductInv> Products { get; set; }        
        string SerializeToXML();        
        string Fkey { get; set; }
    }
}
