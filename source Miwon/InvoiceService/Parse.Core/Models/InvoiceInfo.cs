using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parse.Core.Models
{
    public class InvoiceInfo
    {
        public string transactionUuid { get; set; }
        public string templateCode { get; set; }
        public string invoiceSeries { get; set; }
        public string invoiceIssuedDate { get; set; }
        public string invoiceType { get; set; }
        public string currencyCode { get; set; }
        public string adjustmentType { get; set; }
        public string paymentStatus { get; set; }
        public string paymentType { get; set; }
        public string paymentTypeName { get; set; }
        public bool cusGetInvoiceRight { get; set; }
        public string buyerIdNo { get; set; }
        public string buyerIdType { get; set; }
        public string invoiceNote { get; set; }
        public string adjustmentInvoiceType { get; set; }
        public string originalInvoiceId { get; set; }
        public string originalInvoiceIssueDate { get; set; }
        public string additionalReferenceDesc { get; set; }
        public string additionalReferenceDate { get; set; }
    }

    public class BuyerInfo
    {
        public string buyerName { get; set; } = string.Empty;
        public string buyerLegalName { get; set; } = string.Empty;
        public string buyerTaxCode { get; set; } = string.Empty;
        public string buyerAddressLine { get; set; } = string.Empty;
        public string buyerPhoneNumber { get; set; } = string.Empty;
        public string buyerEmail { get; set; } = string.Empty;
        public string buyerIdNo { get; set; } = string.Empty;
        public string buyerIdType { get; set; } = string.Empty;
        public string buyerCode { get; set; } = string.Empty;
        public string buyerBankName { get; set; } = string.Empty;
        public string buyerBankAccount { get; set; } = string.Empty;
    }

    public class SellerInfo
    {
        public string sellerLegalName { get; set; }
        public string sellerTaxCode { get; set; }
        public string sellerAddressLine { get; set; }
        public string sellerPhoneNumber { get; set; }
        public string sellerEmail { get; set; }
        public string sellerBankName { get; set; }
        public string sellerBankAccount { get; set; }
    }

    public class ItemInfo
    {
        public string selection { get; set; } = "1"; // 1: hàng hóa thông thường, 2: ghi chú, 3: chiết khấu, 4: bảng kê, 5: phí khác
        public string lineNumber { get; set; }
        public string itemCode { get; set; }
        public string itemName { get; set; }
        public string unitName { get; set; }
        public string unitPrice { get; set; }
        public string quantity { get; set; }
        public string itemTotalAmountWithoutTax { get; set; }
        public string itemTotalAmountWithTax { get; set; }
        public string taxPercentage { get; set; }
        public string taxAmount { get; set; }
        public string discount { get; set; }
        public string itemDiscount { get; set; }
        public string adjustmentTaxAmount { get; set; }
        public string isIncreaseItem { get; set; }
        public string expDate { get; set; }
        public string itemNote { get; set; }
    }

    public class ZipFileResponse
    {
        public string errorCode { get; set; }
        public string description { get; set; }
        public string fileName { get; set; }
        public byte[] fileToBytes { get; set; }
        public bool paymentStatus { get; set; }
    }

    public class GetFileRequest
    {
        public string invoiceNo { get; set; }
        public string fileType { get; set; }
        public string strIssueDate { get; set; }
        public string additionalReferenceDesc { get; set; }
        public string additionalReferenceDate { get; set; }
        public string pattern { get; set; }
        public string transactionUuid { get; set; }
    }

    public class SummarizeInfo
    {
        public string sumOfTotalLineAmountWithoutTax { get; set; }
        public string totalAmountWithoutTax { get; set; }
        public string totalTaxAmount { get; set; }
        public string totalAmountWithTax { get; set; }
        public string totalAmountWithTaxInWords { get; set; }
        public string discountAmount { get; set; }
        public string settlementDiscountAmount { get; set; }
        public string taxPercentage { get; set; }
        public bool isTotalAmountPos { get; set; }
        public bool isTotalTaxAmountPos { get; set; }
        public bool isTotalAmtWithoutTaxPos { get; set; }
        public bool isDiscountAmtPos { get; set; }
    }

    public class TaxBreakdowns
    {
        public string taxPercentage { get; set; }
        public string taxableAmount { get; set; }
        public string taxAmount { get; set; }
    }
}
