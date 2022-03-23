using System;
using System.Collections.Generic;
using System.Text;

namespace Parse.Core.Models
{
    public class InvoiceModels
    {
        public InvoiceInfo generalInvoiceInfo { get; set; }
        public BuyerInfo buyerInfo { get; set; }
        public SellerInfo sellerInfo { get; set; }
        public IList<ItemInfo> itemInfo { get; set; } = new List<ItemInfo>();
        public IList<ItemInfo> discountItemInfo { get; set; } = new List<ItemInfo>();
        public IList<MeterRead> meterReading { get; set; } = new List<MeterRead>();
        public SummarizeInfo summarizeInfo { get; set; }
        public IList<TaxBreakdown> taxBreakdowns { get; set; } = new List<TaxBreakdown>();
        public IList<Metadata> metadata { get; set; } = new List<Metadata>();
        public IList<Payment> payments { get; set; } = new List<Payment>();
    }

    public class TaxBreakdown
    {
        public decimal? taxPercentage { get; set; }
        public decimal? taxableAmount { get; set; }
        public decimal? taxAmount { get; set; }
    }

    public class MeterRead
    {
        public decimal previousIndex { get; set; }
        public decimal currentIndex { get; set; }
        public decimal factor { get; set; }
        public decimal amount { get; set; }
    }

    public class Metadata
    {
        public virtual int invoiceCustomFieldId { get; set; }
        public virtual string keyTag { get; set; }
        public virtual string valueType { get; set; }
        public virtual string keyLabel { get; set; }
        public virtual string dateValue { get; set; }
        public virtual string numberValue { get; set; }
        public virtual string stringValue { get; set; }
    }

    public class Payment
    {
        public string paymentMethodName { get; set; }
    }
}
