using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Parse.Core.Domain
{
    [XmlType("Product")]
    [DataContract(Name = "Product")]
    public class ProductInv : IProductInv
    {
        public virtual int Id { get; set; }
        public virtual int InvoiceID { get; set; }
        public virtual int Type { get; set; }
        public virtual decimal DiscountAmount { get; set; }
        public virtual float Weight { get; set; }

        [XmlElement("Code")]
        [DataMember(Name = "Code")]
        public virtual string Code { get; set; }

        [XmlElement("ProdName")]
        [DataMember(Name = "ProdName")]
        public virtual string Name { get; set; }

        [XmlElement("ProdPrice")]
        [DataMember(Name = "ProdPrice")]
        public virtual Decimal Price { get; set; }

        [XmlElement("ProdQuantity")]
        [DataMember(Name = "ProdQuantity")]
        public virtual Decimal Quantity { get; set; }

        [XmlElement("ProdUnit")]
        [DataMember(Name = "ProdUnit")]
        public virtual string Unit { get; set; }

        [XmlElement("VATRate")]
        [DataMember(Name = "VATRate")]
        public virtual float VATRate { get; set; }        

        [XmlElement("VATAmount")]
        [DataMember(Name = "VATAmount")]
        public virtual Decimal VATAmount { get; set; }       

        [XmlElement("Amount")]
        [DataMember(Name = "Amount")]
        public virtual Decimal Amount { get; set; }

        /// <summary>
        /// Ghi chu san pham
        /// </summary>
        [XmlElement("Remark")]
        [DataMember(Name = "Remark")]
        public virtual string Remark { get; set; }

        /// <summary>
        /// tong cong tien truoc thue
        /// </summary>
        [XmlElement("Total")]
        [DataMember(Name = "Total")]
        public virtual Decimal Total { get; set; }

        /// <summary>
        /// Ngày
        /// </summary>
        [XmlElement("ProDate")]
        [DataMember(Name = "ProDate")]
        public virtual string ProDate { get; set; }
    }
}
