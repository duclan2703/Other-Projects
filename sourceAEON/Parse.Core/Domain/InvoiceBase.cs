using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Configuration;
using System.Xml.Linq;

namespace Parse.Core.Domain
{
    [DataContract(Namespace = "")]
    public abstract class InvoiceBase : Invoice
    {
        [XmlIgnore]
        [DataMember(Name = "InvoiceNo", Order = 4)]
        public virtual string No { get; set; }

        [XmlIgnore]
        [DataMember(Name = "SerialNo", Order = 3)]
        public virtual string Serial { get; set; }

        [XmlIgnore]
        [DataMember(Name = "InvoicePattern", Order = 2)]
        public virtual string Pattern { get; set; }

        [XmlIgnore]
        [DataMember(Name = "InvoiceName", Order = 1)]
        public virtual string Name { get; set; }

        [XmlIgnore]
        [DataMember(Name = "ComName", Order = 7)]
        public virtual string ComName { get; set; }

        [XmlIgnore]
        [DataMember(Name = "ComPhone", Order = 10)]
        public virtual string ComPhone { get; set; }

        [XmlIgnore]
        [DataMember(Name = "ComAddress", Order = 9)]
        public virtual string ComAddress { get; set; }

        [XmlIgnore]
        [DataMember(Name = "ComTaxCode", Order = 8)]
        public virtual string ComTaxCode { get; set; }

        [XmlIgnore]
        [DataMember(Name = "ComBankName", Order = 12)]
        public virtual string ComBankName { get; set; }

        [XmlIgnore]
        [DataMember(Name = "ComBankNo", Order = 11)]
        public virtual string ComBankNo { get; set; }

        [DataMember(Name = "CusName", Order = 14)]
        public virtual string CusName { get; set; }

        [DataMember(Name = "Buyer", Order = 27)]
        public virtual string Buyer { get; set; }

        [DataMember(Name = "CusCode", Order = 13)]
        public virtual string CusCode { get; set; }

        [XmlIgnore]
        [DataMember(Name = "CusPhone", Order = 16)]
        public virtual string CusPhone { get; set; }

        [DataMember(Name = "CusAddress", Order = 17)]
        public virtual string CusAddress { get; set; }

        [DataMember(Name = "CusTaxCode", Order = 15)]
        public virtual string CusTaxCode { get; set; }

        [XmlIgnore]
        [DataMember(Name = "CusBankName", Order = 18)]
        public virtual string CusBankName { get; set; }

        [XmlIgnore]
        [DataMember(Name = "CusBankNo", Order = 19)]
        public virtual string CusBankNo { get; set; }

        [XmlElement("PaymentStatus")]
        public virtual int PaymentStatus { get; set; }

        [DataMember(Name = "PaymentMethod", Order = 6)]
        public virtual string PaymentMethod { get; set; }

        [XmlElement("CreateBy")]
        public virtual string CreateBy { get; set; }

        [XmlElement("Note")]
        public virtual string Note { get; set; }

        [XmlElement("ProcessInvNote")]
        public virtual string ProcessInvNote { get; set; }
        [DataMember(Name = "VATRate", Order = 22)]
        public virtual float VATRate { get; set; }

        [DataMember(Name = "VATAmount", Order = 21)]
        public virtual decimal VATAmount { get; set; }

        [DataMember(Name = "Amount", Order = 25)]
        public virtual decimal Amount { get; set; }

        [XmlIgnore]
        [DataMember(Name = "SignDate")]
        public virtual string SignDate { get; set; } = DateTime.Now.ToString("dd/MM/yyyy");

        [DataMember(Name = "ArisingDate")]
        private string performanceDateSerialized { get; set; }

        [OnSerializing]
        void OnSerializing(StreamingContext context)
        {
            this.performanceDateSerialized = this.ArisingDate.ToString("dd/MM/yyyy");
        }

        public virtual DateTime ArisingDate { get; set; }

        [DataMember(Name = "AmountInWords", Order = 26)]
        public virtual string AmountInWords { get; set; }

        [DataMember(Name = "Total", Order = 20)]
        public virtual decimal Total { get; set; }

        private IList<ProductInv> _Products = new List<ProductInv>();

        //Datdt: Lấy thông tin công ty từ database
        public virtual string GetXMLData(Company company)
        {
            ComName = company.Name;
            ComBankName = company.BankName;
            ComBankNo = company.BankNumber;
            ComTaxCode = company.TaxCode;
            ComPhone = company.Phone;
            ComAddress = company.Address;
            List<Type> knownTypeList = new List<Type>();
            knownTypeList.Add(typeof(ProductInv));
            DataContractSerializer dcs = new DataContractSerializer(this.GetType(), knownTypeList);

            MemoryStream ms = new MemoryStream();
            dcs.WriteObject(ms, this);

            XmlDocument xdoc = new XmlDocument();
            xdoc.PreserveWhitespace = true;
            xdoc.LoadXml(Encoding.UTF8.GetString(ms.GetBuffer()));

            string href = string.Format("{0}/{1}", company.Domain, "InvoiceTemplate/GetXSLTbyPattern?pattern=01GTKT0/001");
            XmlProcessingInstruction newPI;
            String PItext = "type='text/xsl' href='" + href + "'";
            newPI = xdoc.CreateProcessingInstruction("xml-stylesheet", PItext);
            xdoc.InsertBefore(newPI, xdoc.DocumentElement);

            XDocument xd = XmlHelper.GetXDocument(xdoc);
            XmlHelper.RemoveAttribte(xd);
            XmlHelper.RemoveNamespace(xd);

            xdoc = XmlHelper.GetXmlDocument(xd);

            XmlNodeList xlist = xdoc.DocumentElement.ChildNodes;
            XmlNode newnode = xdoc.DocumentElement.AppendChild(xdoc.CreateElement("Content"));
            XmlAttribute xa1 = xdoc.CreateAttribute("Id");
            xa1.Value = "SigningData";
            newnode.Attributes.Append(xa1);
            for (int i = 0; i < xlist.Count - 1; i++)
            {
                newnode.AppendChild(xlist[i]);
                i--;
            }

            xdoc.DocumentElement.RemoveAll();
            xdoc.DocumentElement.AppendChild(newnode);

            return xdoc.OuterXml;
        }

        [XmlArray("Products")]
        [DataMember(Name = "Products", Order = 27)]
        public virtual IList<ProductInv> Products
        {
            get
            {
                return _Products;
            }
            set { _Products = value; }
        }

        public virtual string SerializeToXML()
        {
            return string.Empty;
        }

        [XmlIgnore]
        [DataMember(Name = "ComFax", Order = 27)]
        public virtual string ComFax { get; set; }

        [XmlIgnore]
        public virtual int? Type { get; set; }

        [XmlIgnore]
        public virtual int? Status { get; set; }

        [XmlIgnore]
        public virtual int? Paymentstatus { get; set; }

        [XmlIgnore]
        [IgnoreDataMember]
        public virtual string Fkey { get; set; }
    }
}
