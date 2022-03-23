using Newtonsoft.Json;
using Parse.Core;
using Parse.Core.Domain;
using Parse.Core.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;
using ViettelAPI.Models;
using ViettelAPI.Utils;

namespace ViettelAPI
{
    public class APIHelper
    {
        /// <summary>
        /// API tao lap hóa đơn
        /// </summary>
        public static string SendInvoice(InvoiceVAT invoice, string pattern, out string message)
        {
            message = "";

            string userPass = AppContext.Current.company.UserName + ":" + AppContext.Current.company.PassWord;// ConfigurationManager.AppSettings["UserPass"].ToString();
            string codeTax = AppContext.Current.company.TaxCode;  //ConfigurationManager.AppSettings["CodeTax"].ToString();
            string apiLink = AppContext.Current.company.Domain + @"/InvoiceAPI/InvoiceWS/createInvoice/" + codeTax;
            string autStr = Base64Encode(userPass);
            string contentType = "application/json";

            InvoiceInfo objInvoice = new InvoiceInfo();
            objInvoice.transactionUuid = invoice.Fkey;
            objInvoice.invoiceType = "01GTKT";
            objInvoice.templateCode = pattern;
            objInvoice.invoiceSeries = null; //ko nhập lấy mặc định
            //objInvoice.invoiceIssuedDate = ((Int64)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds).ToString();
            objInvoice.currencyCode = "VND";
            objInvoice.adjustmentType = "1";
            objInvoice.paymentStatus = "true";
            objInvoice.paymentType = invoice.PaymentMethod;
            objInvoice.paymentTypeName = invoice.PaymentMethod;
            objInvoice.cusGetInvoiceRight = true;
            objInvoice.buyerIdType = "1";
            objInvoice.buyerIdNo = invoice.CusTaxCode;

            BuyerInfo objBuyer = new BuyerInfo();
            objBuyer.buyerAddressLine = invoice.CusAddress;
            objBuyer.buyerIdNo = invoice.CusTaxCode;
            objBuyer.buyerIdType = "1";
            objBuyer.buyerName = invoice.CusName;
            objBuyer.buyerPhoneNumber = invoice.CusPhone;
            SellerInfo objSeller = new SellerInfo();
            objSeller.sellerAddressLine = invoice.ComAddress;
            objSeller.sellerBankAccount = invoice.ComBankNo;
            objSeller.sellerBankName = invoice.ComBankName;
            objSeller.sellerEmail = "";
            objSeller.sellerLegalName = invoice.ComName;
            objSeller.sellerPhoneNumber = invoice.ComPhone;
            objSeller.sellerTaxCode = invoice.ComTaxCode;

            Decimal sumOfTotalLineAmountWithoutTax = 0;
            List<ItemInfo> lstItem = new List<ItemInfo>();
            foreach (var pro in invoice.Products)
            {
                ItemInfo item = new ItemInfo();
                item.discount = "0.0";
                item.itemCode = pro.Code;
                item.itemDiscount = "0.0";
                item.itemName = pro.Name;
                item.itemTotalAmountWithoutTax = pro.Amount.ToString();
                item.lineNumber = "1";
                item.quantity = pro.Quantity.ToString();
                item.taxAmount = pro.VATAmount.ToString();
                item.taxPercentage = pro.VATRate.ToString();
                item.unitName = pro.Unit;
                item.unitPrice = pro.Price.ToString();
                lstItem.Add(item);

                sumOfTotalLineAmountWithoutTax += pro.Amount;
            }
            // phí dịch vụ
            //lstItem.Add(new ItemInfo()
            //{
            //    selection = "5",
            //    discount = "0.0",
            //    //itemCode = pro.Code,
            //    itemDiscount = "0.0",
            //    itemName = "Service Charge",
            //    itemTotalAmountWithoutTax = invoice.ServiceCharge.ToString(),
            //    lineNumber = "1",
            //    //quantity = "1",
            //    taxAmount = "0",
            //    //taxPercentage = "0",
            //    //unitName = pro.Unit,
            //    //unitPrice = pro.Price.ToString(),
            //});
            //sumOfTotalLineAmountWithoutTax += invoice.ServiceCharge;

            SummarizeInfo objSummary = new SummarizeInfo();
            objSummary.discountAmount = "0";
            objSummary.settlementDiscountAmount = "0";
            objSummary.sumOfTotalLineAmountWithoutTax = sumOfTotalLineAmountWithoutTax.ToString();
            objSummary.taxPercentage = invoice.VATRate.ToString();
            objSummary.totalAmountWithoutTax = sumOfTotalLineAmountWithoutTax.ToString();
            objSummary.totalAmountWithTax = invoice.Amount.ToString();
            objSummary.totalAmountWithTaxInWords = NumberUtil.DocSoThanhChu(objSummary.totalAmountWithTax);
            objSummary.totalTaxAmount = invoice.VATAmount.ToString();

            InvoiceModels model = new InvoiceModels();
            model.generalInvoiceInfo = objInvoice;
            model.buyerInfo = objBuyer;
            model.sellerInfo = objSeller;
            model.itemInfo = lstItem;
            model.summarizeInfo = objSummary;
            model.payments = new List<Payment>() { new Payment() { paymentMethodName = invoice.PaymentMethod } };
            model.taxBreakdowns = new List<TaxBreakdown>();
            model.taxBreakdowns.Add(new TaxBreakdown() { taxPercentage = (decimal)invoice.VATRate, taxableAmount = invoice.Total, taxAmount = invoice.VATAmount });

            string data = JsonConvert.SerializeObject(model);
            string result = Request(apiLink, data, autStr, "POST", contentType);
            return result;
        }
        /// <summary>
        /// API tao lap lô hóa đơn
        /// </summary>
        public static APIResults SendInvoices(List<InvoiceModels> invoices)
        {
            string userPass = AppContext.Current.company.UserName + ":" + AppContext.Current.company.PassWord;
            string codeTax = AppContext.Current.company.TaxCode;
            string apiLink = AppContext.Current.company.Domain + @"/InvoiceAPI/InvoiceWS/createBatchInvoice/" + codeTax;
            string autStr = Base64Encode(userPass);
            string record = ConfigurationManager.AppSettings["RecordPublish"].ToString();
            int number = !string.IsNullOrEmpty(record) ? Convert.ToInt32(record) : 10;
            string contentType = "application/json";

            List<APIResult> lstResult = new List<APIResult>();
            APIResults resultConverted = new APIResults();

            int arrayLength = (int)Math.Ceiling(invoices.Count() / (double)number);
            for (int i = 0; i < arrayLength; i++)
            {
                var listinv = invoices.Skip(i * number).Take(number).ToList();
                string data = JsonConvert.SerializeObject(listinv);
                string loHoaDon = "{\"commonInvoiceInputs\": " + data + " }";
                string result = Request(apiLink, loHoaDon, autStr, "POST", contentType);
                var resultTemp = ParseResult<APIResults>(result);
                lstResult.AddRange(resultTemp.createInvoiceOutputs);
            }

            //if (invoices.Count > number)
            //{
            //    var count = invoices.Count % number > 0 ? (invoices.Count / number + 1) : invoices.Count / number;
            //    for (int i = 0; i < count; i++)
            //    {
            //        List<InvoiceModels> lst = new List<InvoiceModels>();
            //        for (int j = 0; j < number; j++)
            //        {
            //            int k = i * number + j;
            //            if (k < invoices.Count)
            //                lst.Add(invoices[k]);
            //            else
            //                break;
            //        }
            //        string data1 = JsonConvert.SerializeObject(lst);
            //        string loHoaDon1 = "{\"commonInvoiceInputs\": " + data1 + " }";
            //        //File.WriteAllText(@"D:\Projects\test.txt", loHoaDon);
            //        string result1 = Request(apiLink, loHoaDon1, autStr, "POST", contentType);
            //        var resultConverted1 = ParseResult<APIResults>(result1);
            //        lstResult.AddRange(resultConverted1.createInvoiceOutputs);
            //    }
            //    resultConverted.createInvoiceOutputs = lstResult;
            //}
            //else
            //{
            //    string data = JsonConvert.SerializeObject(invoices);
            //    string loHoaDon = "{\"commonInvoiceInputs\": " + data + " }";
            //    //File.WriteAllText(@"D:\Projects\test.txt", loHoaDon);
            //    string result = Request(apiLink, loHoaDon, autStr, "POST", contentType);
            //    resultConverted = ParseResult<APIResults>(result);
            //}
            resultConverted.createInvoiceOutputs = lstResult;

            return resultConverted;
        }
        public static PDFFileResponse GetInvoicePdf(string invNo, string Fkey)
        {
            string user = AppContext.Current.company.UserName;
            string pass = AppContext.Current.company.PassWord;
            string userPass = user + ":" + pass;
            string codeTax = AppContext.Current.company.TaxCode;
            string apiLink = AppContext.Current.company.Domain + @"/InvoiceAPI/InvoiceUtilsWS/getInvoiceRepresentationFile";
            string invPattern = AppContext.Current.company.InvPattern;
            string contentType = "application/json";
            string autStr = Base64Encode(userPass);

            var CommonDataInput = new
            {
                supplierTaxCode = codeTax,
                invoiceNo = invNo,
                pattern = invPattern,
                transactionUuid = Fkey,
                fileType = "PDF"
            };

            string data = JsonConvert.SerializeObject(CommonDataInput);

            string result = Request(apiLink, data, autStr, "POST", contentType);

            var resultConverted = ParseResult<PDFFileResponse>(result);
            return resultConverted;
        }
        private static string Request(string pzUrl, string pzData, string pzAuthorization, string pzMethod, string pzContentType)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(pzUrl);
            httpWebRequest.ContentType = pzContentType;
            httpWebRequest.Method = pzMethod;
            httpWebRequest.Headers.Add("Authorization", "Basic " + pzAuthorization);
            // using proxy
            httpWebRequest.Proxy = WebRequest.DefaultWebProxy; //new WebProxy();//no proxy

            if (!string.IsNullOrEmpty(pzData))
            {
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = pzData;

                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }
            }
            InitiateSSLTrust();//bypass SSL
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            var result = string.Empty;
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
            }
            return result;
        }
        private static void InitiateSSLTrust()
        {
            try
            {
                ServicePointManager.ServerCertificateValidationCallback =
                   new RemoteCertificateValidationCallback(
                        delegate
                        { return true; }
                    );
            }
            catch (Exception ex)
            {

            }
        }
        private static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        private static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
        private static T ParseResult<T>(string result)
        {
            T obj = JsonConvert.DeserializeObject<T>(result);
            return obj;
        }
    }
}
