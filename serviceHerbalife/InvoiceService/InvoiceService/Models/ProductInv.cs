using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceService.Models
{
    public class ProductInv
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public string Remark { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal VATRate { get; set; }
        public decimal VATAmount { get; set; }
        public decimal Total { get; set; }
        public decimal Amount { get; set; }
        public string ProDate { get; set; }
        public decimal DiscountAmount { get; set; }
        public string Type { get; set; }
    }
}
