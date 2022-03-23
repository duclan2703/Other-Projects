using Parse.Core.Domain;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parse.Core
{
    public static class BuildXMLInvService
    {
        public static string BuildXMLInv(IList<InvoiceVAT> lstInvoice)
        {
            StringBuilder str = new StringBuilder("<Invoices>");
            foreach (var invoice in lstInvoice)
            {
                if (invoice.Products.Count > 0)
                {
                    str.Append("<Inv>");
                    str.AppendFormat("<key>{0}</key>", invoice.Fkey);
                    str.Append("<Invoice>");
                    str.AppendFormat("<ArisingDate>{0}</ArisingDate>", invoice.ArisingDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
                    str.AppendFormat("<CusCode>{0}</CusCode>", invoice.CusCode);
                    str.AppendFormat("<CusName>{0}</CusName>", convertSpecialCharacter(invoice.CusName));
                    str.AppendFormat("<Buyer>{0}</Buyer>", convertSpecialCharacter(invoice.Buyer));
                    str.AppendFormat("<CusAddress>{0}</CusAddress>", convertSpecialCharacter(invoice.CusAddress));
                    str.AppendFormat("<CusPhone>{0}</CusPhone>", invoice.CusPhone);
                    str.AppendFormat("<CusTaxCode>{0}</CusTaxCode>", invoice.CusTaxCode);
                    str.AppendFormat("<PaymentMethod>{0}</PaymentMethod>", invoice.PaymentMethod);
                    str.AppendFormat("<RoomNo>{0}</RoomNo>", invoice.RoomNo);
                    str.AppendFormat("<BookingNo>{0}</BookingNo>", invoice.BookingNo);
                    str.AppendFormat("<FolioNo>{0}</FolioNo>", invoice.FolioNo);
                    str.AppendFormat("<ArrivalDate>{0}</ArrivalDate>", invoice.ArrivalDate);
                    str.AppendFormat("<DepartureDate>{0}</DepartureDate>", invoice.DepartureDate);
                    str.Append("<Products>");

                    foreach (var el in invoice.Products)
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
                    str.AppendFormat("<ServiceCharge>{0}</ServiceCharge>", invoice.ServiceCharge);
                    str.AppendFormat("<Total>{0}</Total>", invoice.Total);
                    str.AppendFormat("<VATRate>{0}</VATRate>", invoice.VATRate);
                    str.AppendFormat("<VATAmount>{0}</VATAmount>", invoice.VATAmount);
                    str.AppendFormat("<Amount>{0}</Amount>", invoice.Amount);
                    str.AppendFormat("<AmountInWords>{0}</AmountInWords>", invoice.AmountInWords);
                    str.Append("</Invoice>");
                    str.Append("</Inv>");
                }
            }
            str.Append("</Invoices>");
            return str.ToString();
        }

        //public static string BuildXMLInv(IList<InvoiceVAT> lstInvoice)
        //{
        //    StringBuilder str = new StringBuilder("<Invoices>");
        //    str.Append("<Inv>");
        //    str.AppendFormat("<key>{0}</key>", lstInvoice[0].Fkey);
        //    str.Append("<Invoice>");
        //    str.AppendFormat("<ArisingDate>{0}</ArisingDate>", lstInvoice[0].ArisingDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
        //    str.AppendFormat("<CusCode>{0}</CusCode>", lstInvoice[0].CusCode);
        //    str.AppendFormat("<CusName>{0}</CusName>", convertSpecialCharacter(lstInvoice[0].CusName));
        //    str.AppendFormat("<Buyer>{0}</Buyer>", convertSpecialCharacter(lstInvoice[0].Buyer));
        //    str.AppendFormat("<CusAddress>{0}</CusAddress>", convertSpecialCharacter(lstInvoice[0].CusAddress));
        //    str.AppendFormat("<CusPhone>{0}</CusPhone>", lstInvoice[0].CusPhone);
        //    str.AppendFormat("<CusTaxCode>{0}</CusTaxCode>", lstInvoice[0].CusTaxCode);
        //    str.AppendFormat("<PaymentMethod>{0}</PaymentMethod>", lstInvoice[0].PaymentMethod);
        //    str.AppendFormat("<RoomNo>{0}</RoomNo>", lstInvoice[0].RoomNo);
        //    str.AppendFormat("<BookingNo>{0}</BookingNo>", lstInvoice[0].BookingNo);
        //    str.AppendFormat("<FolioNo>{0}</FolioNo>", lstInvoice[0].FolioNo);
        //    str.AppendFormat("<ArrivalDate>{0}</ArrivalDate>", lstInvoice[0].ArrivalDate);
        //    str.AppendFormat("<DepartureDate>{0}</DepartureDate>", lstInvoice[0].DepartureDate);
        //    str.Append("<Products>");

        //    foreach (var el in lstInvoice[0].Products)
        //    {
        //        str.Append("<Product>");
        //        str.AppendFormat("<Code>{0}</Code>", convertSpecialCharacter(el.Code));
        //        str.AppendFormat("<ProdName>{0}</ProdName>", convertSpecialCharacter(el.Name));
        //        str.AppendFormat("<ProdPrice>{0}</ProdPrice>", el.Price);
        //        str.AppendFormat("<ProdQuantity>{0}</ProdQuantity>", el.Quantity);
        //        str.AppendFormat("<ProdUnit>{0}</ProdUnit>", el.Unit);
        //        str.AppendFormat("<Amount>{0}</Amount>", el.Amount);
        //        str.Append("</Product>");
        //    }
        //    str.Append("</Products>");

        //    //tong tien dich vu                
        //    str.AppendFormat("<ServiceCharge>{0}</ServiceCharge>", lstInvoice[0].ServiceCharge);
        //    str.AppendFormat("<Total>{0}</Total>", lstInvoice[0].Total);
        //    str.AppendFormat("<VATRate>{0}</VATRate>", lstInvoice[0].VATRate);
        //    str.AppendFormat("<VATAmount>{0}</VATAmount>", lstInvoice[0].VATAmount);
        //    str.AppendFormat("<Amount>{0}</Amount>", lstInvoice[0].Amount);
        //    str.AppendFormat("<AmountInWords>{0}</AmountInWords>", lstInvoice[0].AmountInWords);
        //    str.Append("</Invoice>");
        //    str.Append("</Inv>");
        //    str.Append("</Invoices>");
        //    return str.ToString();
        //}
        private static string convertSpecialCharacter(string xmlData)
        {
            return "<![CDATA[" + xmlData + "]]>";
        }
    }
}
