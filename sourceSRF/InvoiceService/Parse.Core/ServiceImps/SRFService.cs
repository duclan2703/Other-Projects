using FX.Core;
using log4net;
using Newtonsoft.Json;
using Parse.Core.Domain;
using Parse.Core.Models;
using Parse.Core.Services;
using Parse.Core.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace Parse.Core.ServiceImps
{
    public class SRFService
    {

        IEInvoice_LogService eLogService = IoC.Resolve<IEInvoice_LogService>();
        static ILog log = LogManager.GetLogger(typeof(SRFService));
        public void Processing()
        {
            string mesError = "";

            var companyFile = Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]) + "\\App_Data\\company.json";
            var companyData = File.ReadAllText(companyFile);
            var company = JsonConvert.DeserializeObject<Company>(companyData);
            DateTime starttime;
            DateTime processtime;
            if (DateTime.TryParseExact(company.StartTime, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out starttime) && DateTime.TryParseExact(company.ProcessTime, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out processtime))
            {
                bool isSuccessAll;
                while (processtime <= DateTime.Now.Date)
                {
                    mesError = "";
                    if (processtime <= starttime)
                        processtime = starttime;
                    isSuccessAll = ProcesssData(processtime, company.TaxCode, company.Pattern, company.Serial, ref mesError);
                    if (!string.IsNullOrEmpty(mesError))
                        log.Error(mesError);

                    if (isSuccessAll)
                    {
                        if (CheckErrorData(processtime))
                        {
                            break;
                        }
                        if (processtime < DateTime.Now.Date)
                        {
                            processtime = processtime.AddDays(1);
                            company.ProcessTime = processtime.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                            string jsonData = JsonConvert.SerializeObject(company);
                            File.WriteAllText(companyFile, jsonData);
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
            else
            {
                mesError = "Sai định dạng ngày tháng quét hóa đơn, phải theo định dạng dd/MM/yyyy";
                log.Error(mesError);
            }
        }

        bool ProcesssData(DateTime readtime, string taxcode, string pattern, string serial, ref string mesError)
        {
            int sohoadon = 0;
            try
            {
                List<OINV> lstOINV = new List<OINV>();
                List<INV1> lstINV1 = new List<INV1>();
                List<OCRD> lstOCRD = new List<OCRD>();
                List<CRD1> lstCRD1 = new List<CRD1>();
                GetDataFromDB(readtime, ref lstOINV, ref lstINV1, ref lstOCRD, ref lstCRD1);
                int invTotal = 0;
                if (lstOINV.Count > 0 && lstINV1.Count > 0)
                {
                    List<InvoiceVAT> lstInv = GetListInvoiceVAT(lstOINV, lstINV1, lstOCRD, lstCRD1, taxcode, pattern, serial, ref sohoadon, ref invTotal);

                    //Phát hành hóa đơn
                    var lstInvApi = ConvertToAPIModel(lstInv);
                    APIResults results = SendInvoices(lstInvApi);

                    int invSuccess = 0;
                    int invError = 0;
                    string errorList = "";


                    //Cập nhật trạng thái hóa đơn đã phát hành
                    foreach (var result in results.createInvoiceOutputs)
                    {
                        var mt = lstOINV.FirstOrDefault(c => c.DocEntry.ToString() == result.transactionUuid.Split('-').ToList().Last());
                        if (mt != null)
                        {
                            EInvoice_Log eLog = new EInvoice_Log();
                            eLog.DocEntry = mt.DocEntry;
                            eLog.Pattern = pattern;
                            eLog.Serial = serial;
                            eLog.CreatedDate = DateTime.Now;
                            eLog.IssueDate = mt.DocDate;

                            if (!string.IsNullOrEmpty(result.errorCode))
                            {
                                eLog.ErrorCode = result.errorCode;
                                eLog.ErrorDesc = result.description;
                                invError += 1;
                            }
                            else
                            {
                                eLog.InvNo = result.result.invoiceNo;
                                invSuccess += 1;
                            }
                            eLogService.CreateNew(eLog);
                        }
                    }
                    eLogService.CommitChanges();

                    mesError = string.Format("Upload và phát hành hóa đơn thành công: " + invSuccess + "/" + invTotal + ". Có: " + invError + " hóa đơn lỗi:" + errorList);

                    if (invSuccess == invTotal)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.Error(sohoadon + " " + ex);
                return false;
            }
        }

        public void GetDataFromDB(DateTime readtime, ref List<OINV> invoice, ref List<INV1> products, ref List<OCRD> ocrd, ref List<CRD1> crd1)
        {
            IOINVService oinvService = IoC.Resolve<IOINVService>();
            IOCRDService ocrdService = IoC.Resolve<IOCRDService>();
            ICRD1Service crd1Service = IoC.Resolve<ICRD1Service>();
            IINV1Service inv1Service = IoC.Resolve<IINV1Service>();
            var invQuery = from o in oinvService.Query
                           join d in ocrdService.Query on o.CardCode equals d.CardCode
                           where o.CANCELED == 'N' && o.DocDate == readtime && !(eLogService.Query.Any(e => e.DocEntry == o.DocEntry))
                           select new DataModel(o, d);
            var lstModel = invQuery.ToList();
            invoice = lstModel.Select(c => c.invoice).Distinct().ToList();
            ocrd = lstModel.Select(c => c.ocrd).Distinct().ToList();

            //oinvService.Query.Where(x => x.CANCELED == 'N' && x.DocDate == readtime).ToList();

            string sqlquery = "select";
            sqlquery += " INV1.DocEntry,";
            sqlquery += " INV1.ItemCode,";
            sqlquery += " INV1.Dscription,";
            sqlquery += " INV1.Quantity,";
            sqlquery += " INV1.Price,";
            sqlquery += " INV1.LineTotal,";
            sqlquery += " INV1.unitMsr";
            sqlquery += " INV1.VatPrcnt";
            sqlquery += " from INV1 inner join OINV on OINV.DocEntry = INV1.DocEntry";
            sqlquery += " where OINV.CANCELED = 'N'";
            sqlquery += " and OINV.DocDate = '" + readtime.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + "'";
            sqlquery += " and not (exists(select EInvoice_Log.Id from EInvoice_Log where EInvoice_Log.DocEntry = OINV.DocEntry and EInvoice_Log.InvNo is NULL))";

            products = (List<INV1>)inv1Service.GetbyQuery<INV1>(sqlquery, false);

            string crdquery = "select";
            crdquery += " CRD1.CardCode,";
            crdquery += " CRD1.U_TaxAddress";
            crdquery += " from CRD1 inner join OINV on OINV.CardCode = CRD1.CardCode";
            crdquery += " where OINV.CANCELED = 'N'";
            crdquery += " and OINV.DocDate = '" + readtime.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + "'";
            crdquery += " and CRD1.AdresType = 'B'";
            crdquery += " and not (exists(select EInvoice_Log.Id from EInvoice_Log where EInvoice_Log.DocEntry = OINV.DocEntry and EInvoice_Log.InvNo is NULL))";

            crd1 = (List<CRD1>)crd1Service.GetbyQuery<CRD1>(crdquery, false);
        }

        public List<InvoiceVAT> GetListInvoiceVAT(List<OINV> lstOINV, List<INV1> lstINV1, List<OCRD> lstOCRD, List<CRD1> lstCRD1, string taxcode, string pattern, string serial, ref int sohoadon, ref int invTotal)
        {
            List<InvoiceVAT> lstInv = new List<InvoiceVAT>();

            foreach (var mt in lstOINV)
            {
                decimal vatRate = 0;
                InvoiceVAT inv = new InvoiceVAT();

                sohoadon = mt.DocEntry;
                inv.Fkey = taxcode + "-" + sohoadon;
                inv.Pattern = pattern;
                inv.Serial = serial;
                inv.ArisingDate = mt.DocDate;
                inv.CusTaxCode = mt.LicTradNum;
                if (mt.GroupNum == 13 || mt.GroupNum == 3)
                    inv.PaymentMethod = "CK";
                else if (mt.GroupNum == 20)
                    inv.PaymentMethod = "DTCN";
                inv.CusCode = mt.CardCode;
                var crd = lstCRD1.FirstOrDefault(c => c.CardCode == mt.CardCode && (c.U_TaxAddress != null || c.U_TaxAddress != ""));
                if (crd != null)
                {
                    inv.CusAddress = crd.U_TaxAddress;
                }
                var ocr = lstOCRD.FirstOrDefault(c => c.CardCode == mt.CardCode);
                if (ocr != null)
                {
                    inv.CusEmail = ocr.E_Mail;
                    inv.Buyer = ocr.AliasName;
                    inv.CusName = ocr.CardName;
                }
                inv.UGINo = mt.U_GINo;
                inv.TrackNo = mt.TrackNo;
                inv.SaleOrder = mt.Comments.Substring(21, 21);

                inv.Products = GetListProductInv(lstINV1, mt.DocEntry, out vatRate);
                inv.VATRate = (float)vatRate;

                inv.Total = Math.Round(inv.Products.Sum(c => c.Total), MidpointRounding.AwayFromZero);

                if (!string.IsNullOrEmpty(mt.U_DeductType))
                {
                    inv.U_DeductType = mt.U_DeductType;
                    if (mt.DiscSum.HasValue && mt.DiscSum != 0)
                    {
                        inv.DiscountAmount = mt.DiscSum.Value;
                        inv.Total -= inv.DiscountAmount;
                        inv.VATAmount = Math.Round(inv.Total * (decimal)(inv.VATRate / 100), MidpointRounding.AwayFromZero);
                        inv.Amount = Math.Round(inv.Total + inv.VATAmount, MidpointRounding.AwayFromZero);
                    }
                    else
                    {
                        inv.VATAmount = Math.Round(inv.Total * (decimal)(inv.VATRate / 100), MidpointRounding.AwayFromZero);
                        inv.Amount = Math.Round(inv.Total + inv.VATAmount, MidpointRounding.AwayFromZero);
                    }
                }
                else
                {
                    inv.VATAmount = Math.Round(inv.Total * (decimal)(inv.VATRate / 100), MidpointRounding.AwayFromZero);
                    inv.Amount = Math.Round(inv.Total + inv.VATAmount, MidpointRounding.AwayFromZero);
                }

                lstInv.Add(inv);
            }
            invTotal = lstInv.Count;
            return lstInv;
        }
        public List<ProductInv> GetListProductInv(List<INV1> lstINV1, int docentry, out decimal VatRate)
        {
            List<ProductInv> lstProduct = new List<ProductInv>();
            var lstItem = lstINV1.Where(c => c.DocEntry == docentry).ToList();
            VatRate = lstItem.FirstOrDefault().VatPrcnt;
            foreach (var dt in lstItem)
            {
                ProductInv pro = new ProductInv();
                pro.Code = dt.ItemCode;
                pro.Name = dt.Dscription;
                pro.Unit = dt.unitMsr;
                pro.Quantity = dt.Quantity;
                pro.Price = dt.Price;
                pro.Total = dt.LineTotal;
                pro.VATRate = dt.VatPrcnt;
                if (pro.Price == 0)
                {
                    pro.Name += " (Khuyến mãi)";
                }
                lstProduct.Add(pro);
            }
            return lstProduct;
        }

        public bool CheckErrorData(DateTime readtime)
        {
            var lstError = eLogService.Query.Where(c => c.IssueDate == readtime && (c.InvNo == null || c.InvNo == "")).ToList();
            if (lstError.Count > 0)
                return true;
            else
                return false;
        }

        public List<InvoiceModels> ConvertToAPIModel(List<InvoiceVAT> invoices)
        {
            CultureInfo culture = new CultureInfo("en-US");
            List<InvoiceModels> dsHoaDon = new List<InvoiceModels>();
            var invSortByDate = invoices.OrderBy(c => c.ArisingDate);
            foreach (var invoice in invSortByDate)
            {
                InvoiceInfo objInvoice = new InvoiceInfo();
                objInvoice.transactionUuid = invoice.Fkey;
                objInvoice.invoiceType = !string.IsNullOrEmpty(invoice.Pattern) ? invoice.Pattern.Substring(0, 6) : ""; // "01GTKT"
                objInvoice.templateCode = !string.IsNullOrEmpty(invoice.Pattern) ? invoice.Pattern : "";
                objInvoice.invoiceSeries = !string.IsNullOrEmpty(invoice.Serial) ? invoice.Serial : ""; //ko nhập lấy mặc định
                objInvoice.invoiceIssuedDate = invoice.ArisingDate < DateTime.Now.Date ? NumberUtil.ConvertToUnixTime(invoice.ArisingDate.AddHours(23).AddMinutes(59).AddSeconds(59)).ToString() : "";
                objInvoice.currencyCode = "VND";
                objInvoice.adjustmentType = "1";
                objInvoice.paymentStatus = "true";

                objInvoice.paymentType = invoice.PaymentMethod;
                objInvoice.paymentTypeName = "0";
                objInvoice.cusGetInvoiceRight = true;
                objInvoice.buyerIdType = "1";
                objInvoice.buyerIdNo = invoice.CusTaxCode;

                BuyerInfo objBuyer = new BuyerInfo();
                objBuyer.buyerAddressLine = invoice.CusAddress;
                objBuyer.buyerCode = invoice.CusCode;
                objBuyer.buyerBankName = invoice.CusBankName;
                objBuyer.buyerBankAccount = invoice.CusBankNo;
                objBuyer.buyerIdNo = invoice.CusTaxCode;
                objBuyer.buyerIdType = "1";
                objBuyer.buyerName = invoice.Buyer;
                objBuyer.buyerLegalName = invoice.CusName;
                objBuyer.buyerPhoneNumber = invoice.CusPhone;
                objBuyer.buyerTaxCode = invoice.CusTaxCode;
                objBuyer.buyerEmail = invoice.CusEmail;
                SellerInfo objSeller = new SellerInfo();
                objSeller.sellerAddressLine = invoice.ComAddress;
                objSeller.sellerBankAccount = invoice.ComBankNo;
                objSeller.sellerBankName = invoice.ComBankName;
                objSeller.sellerEmail = "";
                objSeller.sellerLegalName = invoice.ComName;
                objSeller.sellerPhoneNumber = invoice.ComPhone;
                objSeller.sellerTaxCode = invoice.ComTaxCode;


                //Danh sách hàng hóa
                List<ItemInfo> lstItem = new List<ItemInfo>();

                foreach (var pro in invoice.Products)
                {

                    ItemInfo item = new ItemInfo();
                    item.discount = "0.0";
                    item.itemCode = pro.Code;
                    item.itemDiscount = pro.DiscountAmount.ToString(culture);
                    item.itemName = pro.Name;
                    item.itemTotalAmountWithoutTax = pro.Total.ToString(culture);
                    item.lineNumber = "1";
                    item.quantity = pro.Quantity.ToString(culture);
                    item.taxAmount = pro.VATAmount.ToString(culture);
                    item.taxPercentage = pro.VATRate.ToString(culture);
                    item.unitName = pro.Unit;
                    item.unitPrice = pro.Price.ToString(culture);
                    item.expDate = pro.ProDate;
                    item.itemNote = pro.Remark;

                    lstItem.Add(item);
                }
                if (!string.IsNullOrEmpty(invoice.U_DeductType))
                    lstItem.Add(new ItemInfo
                    {
                        selection = "2",
                        isIncreaseItem = "false",
                        itemName = invoice.U_DeductType,
                        itemTotalAmountWithoutTax = invoice.DiscountAmount.ToString(culture),
                        lineNumber = "1",
                        taxAmount = "0"
                    });

                SummarizeInfo objSummary = new SummarizeInfo();
                objSummary.discountAmount = invoice.DiscountAmount.ToString(culture);
                //objSummary.settlementDiscountAmount = "0";
                objSummary.taxPercentage = invoice.VATRate.ToString(culture);
                objSummary.totalAmountWithTax = invoice.Amount.ToString(culture);
                objSummary.totalAmountWithTaxInWords = NumberUtil.DocSoThanhChu(objSummary.totalAmountWithTax);
                objSummary.totalTaxAmount = invoice.VATAmount.ToString(culture);
                objSummary.totalAmountWithoutTax = invoice.Total.ToString(culture);
                objSummary.sumOfTotalLineAmountWithoutTax = invoice.Total.ToString(culture);

                //Dữ liệu trường động
                List<Metadata> lstMetaData = new List<Metadata>();
                lstMetaData.Add(new Metadata
                {
                    invoiceCustomFieldId = 2461,
                    keyTag = "invoiceNote",
                    stringValue = invoice.TrackNo + " " + invoice.UGINo + " " + invoice.SaleOrder,
                    valueType = "text",
                    keyLabel = "Ghi chú",
                });
                lstMetaData.Add(new Metadata
                {
                    invoiceCustomFieldId = 701,
                    keyTag = "customerRef",
                    stringValue = invoice.CusCode,
                    valueType = "text",
                    keyLabel = "Số tham chiếu Khách hàng",
                });

                //Dữ liệu thuế
                //List<TaxBreakdown> lstTax = new List<TaxBreakdown>();
                //lstTax.Add(new TaxBreakdown
                //{
                //    taxPercentage = -2,
                //    taxableAmount = invoice.TotalNo,
                //    taxAmount = invoice.VATAmountNo,
                //});
                //lstTax.Add(new TaxBreakdown
                //{
                //    taxPercentage = 0,
                //    taxableAmount = invoice.Total0,
                //    taxAmount = invoice.VATAmount0,
                //}); lstTax.Add(new TaxBreakdown
                //{
                //    taxPercentage = 5,
                //    taxableAmount = invoice.Total5,
                //    taxAmount = invoice.VATAmount5,
                //}); lstTax.Add(new TaxBreakdown
                //{
                //    taxPercentage = 10,
                //    taxableAmount = invoice.Total10,
                //    taxAmount = invoice.VATAmount10,
                //});

                InvoiceModels model = new InvoiceModels();
                model.generalInvoiceInfo = objInvoice;
                model.buyerInfo = objBuyer;
                model.sellerInfo = objSeller;
                model.itemInfo = lstItem;
                model.summarizeInfo = objSummary;
                model.metadata = lstMetaData;
                model.payments = new List<Payment>() { new Payment() { paymentMethodName = invoice.PaymentMethod } };
                //model.taxBreakdowns = lstTax;
                model.taxBreakdowns.Add(new TaxBreakdown() { taxPercentage = (decimal)invoice.VATRate, taxableAmount = invoice.Total, taxAmount = invoice.VATAmount });

                dsHoaDon.Add(model);
            }
            return dsHoaDon;
        }

        public static APIResults SendInvoices(List<InvoiceModels> invoices)
        {
            var companyFile = Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]) + "\\App_Data\\company.json";
            var companyData = File.ReadAllText(companyFile);
            var company = JsonConvert.DeserializeObject<Company>(companyData);

            string userPass = company.Username + ":" + company.Password;
            string codeTax = company.TaxCode;
            string apiLink = company.Url + @"/InvoiceAPI/InvoiceWS/createBatchInvoice/" + codeTax;
            string autStr = Base64Encode(userPass);
            string contentType = "application/json";

            List<APIResult> lstResult = new List<APIResult>();
            APIResults resultConverted = new APIResults();
            int arrayLength = (int)Math.Ceiling(invoices.Count / (decimal)100);

            for (int i = 0; i < arrayLength; i++)
            {
                var listinv = invoices.Skip(i * 100).Take(100).ToList();
                string data = JsonConvert.SerializeObject(listinv);
                string loHoaDon = "{\"commonInvoiceInputs\": " + data + " }";
                string result = Request(apiLink, loHoaDon, autStr, "POST", contentType);
                var resultTemp = ParseResult<APIResults>(result);
                lstResult.AddRange(resultTemp.createInvoiceOutputs);
            }
            resultConverted.createInvoiceOutputs = lstResult;

            return resultConverted;

            //string data = JsonConvert.SerializeObject(invoices);
            // string loHoaDon = "{\"commonInvoiceInputs\": " + data + " }";

            //string result = Request(apiLink, loHoaDon, autStr, "POST", contentType);
            //var resultConverted = ParseResult<APIResults>(result);
            //return resultConverted;

        }

        public static APIResult SendDrafInvoices(InvoiceModels invoice)
        {
            var companyFile = Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]) + "\\App_Data\\company.json";
            var companyData = File.ReadAllText(companyFile);
            var company = JsonConvert.DeserializeObject<Company>(companyData);

            string userPass = company.Username + ":" + company.Password;
            string codeTax = company.TaxCode;
            string apiLink = company.Url + @"/InvoiceAPI/InvoiceWS/createOrUpdateInvoiceDraft/" + codeTax;
            string autStr = Base64Encode(userPass);
            string contentType = "application/json";

            string data = JsonConvert.SerializeObject(invoice);

            string result = Request(apiLink, data, autStr, "POST", contentType);
            var resultConverted = ParseResult<APIResult>(result);
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
