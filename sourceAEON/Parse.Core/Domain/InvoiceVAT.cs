using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Globalization;
using System.Threading;

namespace Parse.Core.Domain
{
    [XmlType("Invoice")]
    [DataContract(Name = "Invoice", Namespace = "")]
    [KnownType(typeof(InvoiceVAT))]
    public class InvoiceVAT : InvoiceBase
    {
        public virtual int Id { get; set; }
        public virtual PublishStatus Publish { get; set; } = PublishStatus.None;
        public virtual string MessageError { get; set; }
        public virtual string AppName { get; set; } // OPERA, MICROS, SPA
        public virtual bool Converted { get; set; } // in chuyen doi
        public virtual string CusEmail { get; set; }
        public virtual string CusComName { get; set; }
        public virtual string CusDeliveryAddress { get; set; }
        public virtual string StaffId { get; set; }
        public virtual string DeliveryId { get; set; }
        public virtual float Weight { get; set; }
        public virtual decimal Quantity { get; set; }
        public virtual decimal DiscountAmount { get; set; }
        public virtual string FolioOrigin { get; set; }
        public virtual string SellMethod { get; set; }
        public virtual decimal TotalNo { get; set; }
        public virtual decimal VATAmountNo { get; set; }
        public virtual decimal AmountNo { get; set; }
        public virtual decimal Total0 { get; set; }
        public virtual decimal VATAmount0 { get; set; }
        public virtual decimal Amount0 { get; set; }
        public virtual decimal Total5 { get; set; }
        public virtual decimal VATAmount5 { get; set; }
        public virtual decimal Amount5 { get; set; }
        public virtual decimal Total10 { get; set; }
        public virtual decimal VATAmount10 { get; set; }
        public virtual decimal Amount10 { get; set; }
        public virtual int SapNumber { get; set; }

        //Số phòng
        [XmlElement("RoomNo")]
        [DataMember(Name = "RoomNo")]
        public virtual string RoomNo { get; set; }

        [XmlElement("BookingNo")]
        [DataMember(Name = "BookingNo")]
        public virtual string BookingNo { get; set; }

        //Số người
        [XmlElement("GuestNo")]
        [DataMember(Name = "GuestNo")]
        public virtual string GuestNo { get; set; }

        //Số tham chiếu
        [XmlElement("FolioNo")]
        [DataMember(Name = "FolioNo")]
        public virtual string FolioNo { get; set; }

        //Ngày checkin
        [XmlElement("ArrivalDate")]
        [DataMember(Name = "ArrivalDate")]
        public virtual string ArrivalDate { get; set; }

        //Ngày checkout
        [XmlElement("DepartureDate")]
        [DataMember(Name = "DepartureDate")]
        public virtual string DepartureDate { get; set; }

        //Phí dịch vụ
        [XmlElement("ServiceCharge")]
        [DataMember(Name = "ServiceCharge")]
        public virtual decimal ServiceCharge { get; set; }

        //VAT phí dịch vụ
        [XmlElement("VATSerCharge")]
        [DataMember(Name = "VATSerCharge")]
        public virtual float VATSerCharge { get; set; }

        //Phí dịch vụ đặc biệt
        [XmlElement("ServiceSpecial")]
        [DataMember(Name = "ServiceSpecial")]
        public virtual decimal ServiceSpecial { get; set; }

        //VAT phí dịch vụ đặc biệt
        [XmlElement("VATSerSpecial")]
        [DataMember(Name = "VATSerSpecial")]
        public virtual float VATSerSpecial { get; set; }

        public override string SerializeToXML()
        {
            try
            {
                StringBuilder str = new StringBuilder("<Invoice>");
                str.AppendFormat("<ArisingDate>{0}</ArisingDate>", this.ArisingDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
                str.AppendFormat("<CusCode>{0}</CusCode>", this.CusCode);
                str.AppendFormat("<CusName>{0}</CusName>", convertSpecialCharacter(this.CusName));
                str.AppendFormat("<Buyer>{0}</Buyer>", convertSpecialCharacter(this.Buyer));
                str.AppendFormat("<CusAddress>{0}</CusAddress>", convertSpecialCharacter(this.CusAddress));
                str.AppendFormat("<CusPhone>{0}</CusPhone>", this.CusPhone);
                str.AppendFormat("<CusTaxCode>{0}</CusTaxCode>", this.CusTaxCode);
                str.AppendFormat("<PaymentMethod>{0}</PaymentMethod>", this.PaymentMethod);
                str.AppendFormat("<RoomNo>{0}</RoomNo>", this.RoomNo);
                str.AppendFormat("<BookingNo>{0}</BookingNo>", this.BookingNo);
                str.AppendFormat("<FolioNo>{0}</FolioNo>", this.FolioNo);
                str.AppendFormat("<ArrivalDate>{0}</ArrivalDate>", this.ArrivalDate);
                str.AppendFormat("<DepartureDate>{0}</DepartureDate>", this.DepartureDate);
                str.Append("<Products>");

                foreach (var el in this.Products)
                {
                    str.Append("<Product>");
                    str.AppendFormat("<Code>{0}</Code>", convertSpecialCharacter(el.Code));
                    str.AppendFormat("<ProdName>{0}</ProdName>", convertSpecialCharacter(el.Name));
                    str.AppendFormat("<ProdPrice>{0}</ProdPrice>", el.Price);
                    str.AppendFormat("<ProdQuantity>{0}</ProdQuantity>", el.Quantity);
                    str.AppendFormat("<ProdUnit>{0}</ProdUnit>", el.Unit);
                    str.AppendFormat("<Amount>{0}</Amount>", el.Amount);
                    str.Append("</Product>");
                }
                str.Append("</Products>");

                //tong tien dich vu                
                str.AppendFormat("<ServiceCharge>{0}</ServiceCharge>", this.ServiceCharge);
                str.AppendFormat("<Total>{0}</Total>", this.Total);
                str.AppendFormat("<VATRate>{0}</VATRate>", this.VATRate);
                str.AppendFormat("<VATAmount>{0}</VATAmount>", this.VATAmount);
                str.AppendFormat("<Amount>{0}</Amount>", this.Amount);
                str.AppendFormat("<AmountInWords>{0}</AmountInWords>", this.AmountInWords);

                str.Append("</Invoice>");
                return str.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string convertSpecialCharacter(string xmlData)
        {
            return "<![CDATA[" + xmlData + "]]>";
        }
    }
}
