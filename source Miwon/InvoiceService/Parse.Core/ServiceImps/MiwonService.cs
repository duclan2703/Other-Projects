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
    public class MiwonService
    {
        static ILog log = LogManager.GetLogger(typeof(MiwonService));
        IHDDTService hddtService = IoC.Resolve<IHDDTService>();
        IPXKService pxkService = IoC.Resolve<IPXKService>();
        public void Processing()
        {
            var companyFile = Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]) + "\\App_Data\\company.json";
            var companyData = File.ReadAllText(companyFile);
            var company = JsonConvert.DeserializeObject<Company>(companyData);

            var dataFile = Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]) + "\\App_Data\\datainfo.json";
            var dataData = File.ReadAllText(dataFile);
            var data = JsonConvert.DeserializeObject<DataInfo>(dataData);

            foreach (var chitiet in data.MauHoaDon)
            {
                string sohoadon = "";
                try
                {
                    DateTime processTime;
                    DateTime currentTime = DateTime.Now.Date;
                    DateTime.TryParseExact(chitiet.ProcessTime, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out processTime);

                    List<HDDT> lstHddt = new List<HDDT>();
                    List<PXK> lstPxk = new List<PXK>();
                    List<InvoiceVAT> lstInv = new List<InvoiceVAT>();
                    int invTotal = 0;
                    if (chitiet.Ten == "Hoa don gia tri gia tang")
                    {
                        lstHddt = hddtService.GetListHDDTByDate(processTime, company.TaxCode);
                        lstInv = GetListInvoiceVAT(lstHddt, currentTime, chitiet, ref sohoadon, ref invTotal);
                    }
                    else
                    {
                        lstPxk = pxkService.GetListPXKByDate(processTime, company.TaxCode);
                        lstInv = GetListPXK(lstPxk, currentTime, chitiet, ref sohoadon, ref invTotal);
                    }
                    if (lstInv.Count > 0)
                    {
                        var lstHuy = lstInv.Where(c => c.Type == 5).ToList();
                        if (lstHuy.Count != 0)
                            lstInv = lstInv.Except(lstHuy).OrderBy(c => c.ArisingDate).ToList();
                        //Phát hành hóa đơn
                        var lstInvApi = ConvertToAPIModel(lstInv);
                        APIResults results = SendInvoices(company, lstInvApi);

                        int invSuccess = 0;
                        int invError = 0;
                        string errorList = "";


                        //Cập nhật trạng thái hóa đơn đã phát hành
                        foreach (var result in results.createInvoiceOutputs)
                        {
                            if (chitiet.Ten == "Hoa don gia tri gia tang")
                            {
                                var fkey = result.transactionUuid.Split('-').ToList().First();
                                var hd = lstHddt.FirstOrDefault(c => c.Fkey == fkey);
                                var inv = lstInv.FirstOrDefault(c => c.Fkey == fkey);
                                if (hd != null && inv != null)
                                    hddtService.UpdateHDDT(hd, result, inv, ref invSuccess, ref invError, ref errorList);
                            }
                            else
                            {
                                var fkey = result.transactionUuid.Split('-').ToList().First();
                                var pxk = lstPxk.FirstOrDefault(c => c.Fkey == fkey);
                                var inv = lstInv.FirstOrDefault(c => c.Fkey == fkey);
                                if (pxk != null && inv != null)
                                    pxkService.UpdatePXK(pxk, result, inv, ref invSuccess, ref invError, ref errorList);
                            }
                        }

                        if (lstHuy.Count != 0)
                        {
                            foreach (var inv in lstHuy)
                            {
                                if (chitiet.Ten == "Hoa don gia tri gia tang")
                                {
                                    var resultHuy = CancelInvoice(company, inv);
                                    var hd = lstHddt.FirstOrDefault(c => c.Fkey == inv.Fkey);
                                    if (hd != null)
                                        hddtService.UpdateCancelHDDT(hd, resultHuy, ref invSuccess, ref invError, ref errorList);
                                }
                                else
                                {
                                    var resultHuy = CancelInvoice(company, inv);
                                    var pxk = lstPxk.FirstOrDefault(c => c.Fkey == inv.Fkey);
                                    if (pxk != null)
                                        pxkService.UpdateCancelPXK(pxk, resultHuy, ref invSuccess, ref invError, ref errorList);
                                }
                            }
                        }

                        if (invSuccess == invTotal)
                        {
                            log.Error("Xử lý dữ liệu " + chitiet.Ten + " thành công: " + invSuccess + "/" + invTotal);
                            var lstBackDate = lstInv.Where(c => c.isBackDate == 1).ToList();
                            if (lstBackDate.Count > 0)
                            {
                                chitiet.ProcessTime = lstBackDate.Max(c => c.ArisingDate).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                                string jsonData = JsonConvert.SerializeObject(data);
                                File.WriteAllText(dataFile, jsonData);
                            }
                        }
                        else
                        {
                            log.Error("Xử lý dữ liệu " + chitiet.Ten + " thành công: " + invSuccess + "/" + invTotal + ". Có: " + invError + " hóa đơn lỗi:" + errorList);
                        }
                    }

                }
                catch (Exception ex)
                {
                    log.Error(sohoadon + " " + ex);
                }
            }

        }

        public List<InvoiceVAT> GetListInvoiceVAT(List<HDDT> lstHDDT, DateTime currentTime, ChiTiet chitiet, ref string sohoadon, ref int invTotal)
        {
            List<InvoiceVAT> lstInv = new List<InvoiceVAT>();
            foreach (var hd in lstHDDT)
            {
                InvoiceVAT inv = new InvoiceVAT();
                inv.InvType = 0;
                sohoadon = hd.Fkey;
                inv.ArisingDate = hd.CreatedDate;
                inv.Pattern = chitiet.MauSo;
                if (inv.ArisingDate < currentTime)
                {
                    inv.Serial = chitiet.KyHieuLuiNgay;
                    inv.isBackDate = 1;
                }
                else
                    inv.Serial = hd.Serial;
                inv.Type = hd.Type;
                if (inv.Type == 2 || inv.Type == 3 || inv.Type == 5)
                {
                    var oldHd = hddtService.GetByFkey(hd.PreFkey);
                    if (oldHd != null)
                    {
                        inv.invNo = oldHd.InvNo.Replace("|", "");
                        inv.dateIssue = oldHd.CreatedDate;
                        inv.Serial = oldHd.Serial;
                    }
                }
                inv.Fkey = sohoadon;

                inv.Buyer = hd.Buyer;
                inv.CusCode = hd.CusCode;
                inv.CusName = hd.CusName;
                inv.CusAddress = hd.CusAddress;
                inv.CusTaxCode = hd.CusTaxCode;
                inv.Currency = hd.Currency;
                inv.Note = hd.Note;
                inv.CusEmail = hd.CusEmail;

                foreach (var detail in hd.lstDetail)
                {
                    inv.Products.Add(new ProductInv
                    {
                        Code = detail.Code,
                        Name = detail.Description,
                        Quantity = detail.Quantity.HasValue ? Math.Abs(detail.Quantity.Value) : 0,
                        Unit = detail.Unit,
                        Price = detail.Price.HasValue ? Math.Abs(detail.Price.Value) : 0,
                        Total = detail.Total.HasValue ? Math.Abs(detail.Total.Value) : 0,
                        VATRate = detail.Total.HasValue ? Convert.ToDecimal(detail.VatRate.Value) : 0,
                        VATAmount = detail.VatAmount.HasValue ? Math.Abs(detail.VatAmount.Value) : 0,
                        Amount = detail.TotalAmount.HasValue ? Math.Abs(detail.TotalAmount.Value) : 0,
                    });
                }
                if (hd.Total.HasValue)
                    inv.Total = Math.Abs(hd.Total.Value);
                else
                    inv.Total = inv.Products.Sum(c => c.Total);
                if (hd.TaxAmount.HasValue)
                    inv.VATAmount = Math.Abs(hd.TaxAmount.Value);
                else
                    inv.VATAmount = inv.Products.Sum(c => c.VATAmount);
                if (hd.Discount.HasValue)
                    inv.DiscountAmount = hd.Discount.Value;
                if (hd.TotalAmount.HasValue)
                    inv.Amount = Math.Abs(hd.TotalAmount.Value);
                else
                    inv.Amount = inv.DiscountAmount != 0 ? inv.Products.Sum(c => c.Amount) - inv.DiscountAmount : inv.Products.Sum(c => c.Amount);

                lstInv.Add(inv);
            }
            invTotal = lstInv.Count;
            return lstInv;
        }

        public List<InvoiceVAT> GetListPXK(List<PXK> lstPXK, DateTime currentTime, ChiTiet chitiet, ref string sohoadon, ref int invTotal)
        {
            List<InvoiceVAT> lstInv = new List<InvoiceVAT>();
            foreach (var pxk in lstPXK)
            {
                InvoiceVAT inv = new InvoiceVAT();
                inv.InvType = 1;
                sohoadon = pxk.Fkey;
                inv.ArisingDate = pxk.CommandDate;
                inv.Pattern = chitiet.MauSo;
                inv.Serial = chitiet.KyHieu;
                inv.Type = pxk.Type == 1 ? 1 : 5;
                if (inv.Type == 2 || inv.Type == 3 || inv.Type == 5)
                {
                    var oldPxk = pxkService.GetByFkey(pxk.PreFkey);
                    if (oldPxk != null)
                    {
                        inv.invNo = oldPxk.InvNo.Replace("|", "");
                        inv.dateIssue = oldPxk.CreatedDate;
                        inv.Serial = oldPxk.Serial;
                    }
                }
                inv.Fkey = sohoadon;
                inv.CommandOf = pxk.CommandOf;
                inv.CommandDescription = pxk.CommandDescription;
                inv.Buyer = pxk.Transporter;
                inv.Transporter = pxk.Transporter;
                inv.contractNo = pxk.ContractNo;
                inv.TransportMethod = pxk.TransportMethod;
                inv.TenKhoXuat = pxk.ExportAt;
                inv.TenKhoNhap = pxk.ImportAt;

                foreach (var detail in pxk.lstDetail)
                {
                    inv.Products.Add(new ProductInv
                    {
                        Code = detail.Code,
                        Name = detail.Description,
                        Quantity = detail.Quantity.HasValue ? detail.Quantity.Value : 0,
                        Unit = detail.Unit,
                        Price = detail.Price.HasValue ? Math.Abs(detail.Price.Value) : 0,
                        Total = detail.Total.HasValue ? Math.Abs(detail.Total.Value) : 0,
                        Amount = detail.Total.HasValue ? Math.Abs(detail.Total.Value) : 0
                    });
                }
                if (pxk.Total.HasValue)
                    inv.Total = Math.Abs(pxk.Total.Value);
                else
                    inv.Total = inv.Products.Sum(c => c.Total);

                lstInv.Add(inv);
            }
            invTotal = lstInv.Count;
            return lstInv;
        }

        public bool CheckErrorData(DateTime readtime)
        {
            IHDDTService hddtService = IoC.Resolve<IHDDTService>();
            var lstError = hddtService.Query.Where(c => c.CreatedDate == readtime && (c.InvNo == null || c.InvNo == "")).ToList();
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
                objInvoice.transactionUuid = invoice.Fkey + "-" + invoice.ArisingDate.ToString("ddMMyyyy");
                objInvoice.invoiceType = !string.IsNullOrEmpty(invoice.Pattern) ? invoice.Pattern.Substring(0, 6) : ""; // "01GTKT"
                objInvoice.templateCode = !string.IsNullOrEmpty(invoice.Pattern) ? invoice.Pattern : "";
                objInvoice.invoiceSeries = !string.IsNullOrEmpty(invoice.Serial) ? invoice.Serial : ""; //ko nhập lấy mặc định
                objInvoice.invoiceIssuedDate = invoice.ArisingDate != DateTime.Now.Date ? NumberUtil.ConvertToUnixTime(invoice.ArisingDate.AddHours(23).AddMinutes(59).AddSeconds(59)).ToString() : "";
                objInvoice.currencyCode = "VND";
                objInvoice.adjustmentType = "1";
                objInvoice.paymentStatus = "true";
                objInvoice.invoiceNote = invoice.Note;

                if (invoice.Type == 3 || invoice.Type == 2)
                {
                    objInvoice.adjustmentType = "5";
                    objInvoice.adjustmentInvoiceType = "1";
                    objInvoice.originalInvoiceId = invoice.invNo;
                    objInvoice.originalInvoiceIssueDate = invoice.dateIssue.ToString("yyyy-MM-dd");
                    objInvoice.additionalReferenceDesc = "Điều chỉnh hóa đơn " + invoice.invNo;
                    objInvoice.additionalReferenceDate = NumberUtil.ConvertToUnixTime(invoice.ArisingDate).ToString();
                }

                objInvoice.cusGetInvoiceRight = true;

                BuyerInfo objBuyer = new BuyerInfo();
                objBuyer.buyerAddressLine = invoice.CusAddress;
                objBuyer.buyerCode = invoice.CusCode;
                objBuyer.buyerBankName = invoice.CusBankName;
                objBuyer.buyerBankAccount = invoice.CusBankNo;
                //objBuyer.buyerIdNo = invoice.CusTaxCode;
                //objBuyer.buyerIdType = "1";
                if (invoice.Pattern.Contains("GTKT"))
                {
                    objBuyer.buyerName = invoice.Buyer;
                    objBuyer.buyerLegalName = invoice.CusName;
                }
                else
                {
                    objBuyer.buyerName = invoice.Buyer;
                    objBuyer.buyerLegalName = !string.IsNullOrEmpty(invoice.CusName) ? invoice.CusName : (!string.IsNullOrEmpty(invoice.TenKhoXuat) ? invoice.TenKhoXuat : "KL");
                }
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
                    item.itemTotalAmountWithTax = pro.Amount.ToString(culture);
                    item.lineNumber = "1";
                    item.quantity = pro.Quantity != 0 ? pro.Quantity.ToString(culture) : null;
                    item.taxAmount = pro.VATAmount.ToString(culture);
                    item.taxPercentage = pro.VATRate.ToString(culture);
                    item.unitName = pro.Unit;
                    item.unitPrice = pro.Price.ToString(culture);
                    item.expDate = pro.ProDate;
                    item.itemNote = pro.Remark;
                    if (invoice.Type == 2)
                    {
                        item.itemName = "Điều chỉnh tăng tiền hàng, tiền thuế của hàng hóa/dịch vụ: " + pro.Name;
                        item.adjustmentTaxAmount = "1";
                        item.isIncreaseItem = "true";
                    }
                    else if (invoice.Type == 3)
                    {
                        item.itemName = "Điều chỉnh giảm tiền hàng, tiền thuế của hàng hóa/dịch vụ: " + pro.Name;
                        item.adjustmentTaxAmount = "1";
                        item.isIncreaseItem = "false";
                    }
                    else if (invoice.Type == 4)
                    {
                        item.selection = "3";
                        item.isIncreaseItem = "false";
                    }
                    lstItem.Add(item);
                }

                if (invoice.Type == 2)
                {
                    lstItem.Add(new ItemInfo
                    {
                        selection = "2",
                        isIncreaseItem = "false",
                        lineNumber = "1",
                        itemName = "Điều chỉnh tăng tiền hàng, tiền thuế cho hóa đơn điện tử mẫu " + invoice.Pattern + " ký hiệu " + invoice.Serial + " số " + invoice.invNo + " lập ngày " + invoice.dateIssue.ToString("yyyy-MM-dd") + " số tiền: " + invoice.Amount.ToString(),
                    });
                }
                else if (invoice.Type == 3)
                {
                    lstItem.Add(new ItemInfo
                    {
                        selection = "2",
                        isIncreaseItem = "false",
                        lineNumber = "1",
                        taxAmount = "0",
                        itemName = "Điều chỉnh giảm tiền hàng, tiền thuế cho hóa đơn điện tử mẫu " + invoice.Pattern + " ký hiệu " + invoice.Serial + " số " + invoice.invNo + " lập ngày " + invoice.dateIssue.ToString("yyyy-MM-dd") + " số tiền: " + invoice.Amount.ToString(),
                    });
                }

                SummarizeInfo objSummary = new SummarizeInfo();
                objSummary.discountAmount = invoice.DiscountAmount.ToString(culture);
                //objSummary.settlementDiscountAmount = "0";
                objSummary.taxPercentage = invoice.VATRate.ToString(culture);
                objSummary.totalAmountWithTax = invoice.Amount.ToString(culture) != "0" ? invoice.Amount.ToString(culture) : invoice.Total.ToString(culture);
                objSummary.totalAmountWithTaxInWords = NumberUtil.DocSoThanhChu(objSummary.totalAmountWithTax);
                objSummary.totalTaxAmount = invoice.VATAmount.ToString(culture);
                objSummary.totalAmountWithoutTax = invoice.Total.ToString(culture);
                objSummary.sumOfTotalLineAmountWithoutTax = invoice.Total.ToString(culture);
                if (invoice.Type == 3 || invoice.Type == 4)
                {
                    objSummary.isTotalAmountPos = false;
                    objSummary.isTotalAmtWithoutTaxPos = false;
                    objSummary.isTotalTaxAmountPos = false;
                    objSummary.isDiscountAmtPos = false;
                }


                InvoiceModels model = new InvoiceModels();
                model.generalInvoiceInfo = objInvoice;
                model.buyerInfo = objBuyer;
                model.sellerInfo = objSeller;
                model.itemInfo = lstItem;
                model.summarizeInfo = objSummary;
                model.payments = new List<Payment>() { new Payment() { paymentMethodName = "TM/CK" } };
                //model.taxBreakdowns = lstTax;
                model.taxBreakdowns.Add(new TaxBreakdown() { taxPercentage = (decimal)invoice.VATRate, taxableAmount = invoice.Total, taxAmount = invoice.VATAmount });
                if (invoice.InvType == 0)
                {
                    List<Metadata> lstMetaData = new List<Metadata>();

                    lstMetaData.Add(new Metadata
                    {
                        invoiceCustomFieldId = 658,
                        keyTag = "invoiceNote",
                        stringValue = invoice.Note,
                        valueType = "text",
                        keyLabel = "Ghi chú",
                    });
                    model.metadata = lstMetaData;
                }
                if (invoice.InvType == 1)
                {
                    //Dữ liệu trường động
                    List<Metadata> lstMetaData = new List<Metadata>();

                    lstMetaData.Add(new Metadata
                    {
                        invoiceCustomFieldId = 776,
                        keyTag = "economicContractNo",
                        stringValue = invoice.Fkey,
                        valueType = "text",
                        keyLabel = "Căn cứ hợp đồng kinh tế số",
                    });

                    lstMetaData.Add(new Metadata
                    {
                        invoiceCustomFieldId = 1164,
                        keyTag = "commandNo",
                        stringValue = invoice.Fkey,
                        valueType = "text",
                        keyLabel = "Lệnh điều động số",
                    });

                    lstMetaData.Add(new Metadata
                    {
                        invoiceCustomFieldId = 777,
                        keyTag = "commandDate",
                        dateValue = NumberUtil.ConvertToUnixTime(invoice.ArisingDate).ToString(),
                        valueType = "date",
                        keyLabel = "Ngày điều động",
                    });

                    lstMetaData.Add(new Metadata
                    {
                        invoiceCustomFieldId = 778,
                        keyTag = "commandOf",
                        stringValue = invoice.CommandOf,
                        valueType = "text",
                        keyLabel = "của",
                    });

                    lstMetaData.Add(new Metadata
                    {
                        invoiceCustomFieldId = 779,
                        keyTag = "commandDes",
                        stringValue = invoice.CommandDescription,
                        valueType = "text",
                        keyLabel = "về việc",
                    });

                    lstMetaData.Add(new Metadata
                    {
                        invoiceCustomFieldId = 780,
                        keyTag = "contractNo",
                        stringValue = invoice.contractNo,
                        valueType = "text",
                        keyLabel = "Hợp đồng số",
                    });

                    lstMetaData.Add(new Metadata
                    {
                        invoiceCustomFieldId = 781,
                        keyTag = "vehicle",
                        stringValue = invoice.TransportMethod,
                        valueType = "text",
                        keyLabel = "Phương tiện vận chuyển",
                    });
                    lstMetaData.Add(new Metadata
                    {
                        invoiceCustomFieldId = 782,
                        keyTag = "exportAt",
                        stringValue = invoice.TenKhoXuat,
                        valueType = "text",
                        keyLabel = "Xuất tại kho",
                    });

                    //lstMetaData.Add(new Metadata
                    //{
                    //    invoiceCustomFieldId = 783,
                    //    keyTag = "exportAtNo",
                    //    stringValue = invoice.MaKhoXuat,
                    //    valueType = "text",
                    //    keyLabel = "Mã kho",
                    //});

                    lstMetaData.Add(new Metadata
                    {
                        invoiceCustomFieldId = 784,
                        keyTag = "importAt",
                        stringValue = invoice.TenKhoNhap,
                        valueType = "text",
                        keyLabel = "Nhập tại kho",
                    });

                    lstMetaData.Add(new Metadata
                    {
                        invoiceCustomFieldId = 4890,
                        keyTag = "deliveryDate",
                        dateValue = invoice.ArisingDate != DateTime.Now.Date ? NumberUtil.ConvertToUnixTime(invoice.ArisingDate.AddHours(23).AddMinutes(59).AddSeconds(59)).ToString() : "",
                        valueType = "date",
                        keyLabel = "Ngày xuất",
                    });

                    lstMetaData.Add(new Metadata
                    {
                        invoiceCustomFieldId = 4891,
                        keyTag = "receiptDate",
                        dateValue = invoice.ArisingDate != DateTime.Now.Date ? NumberUtil.ConvertToUnixTime(invoice.ArisingDate.AddHours(23).AddMinutes(59).AddSeconds(59)).ToString() : "",
                        valueType = "date",
                        keyLabel = "Ngày nhập",
                    });

                    //lstMetaData.Add(new Metadata
                    //{
                    //    invoiceCustomFieldId = 785,
                    //    keyTag = "importAtNo",
                    //    stringValue = invoice.MaKhoNhap,
                    //    valueType = "text",
                    //    keyLabel = "Mã kho",
                    //});

                    model.metadata = lstMetaData;
                }
                dsHoaDon.Add(model);
            }
            return dsHoaDon;
        }

        public static APIResults SendInvoices(Company company, List<InvoiceModels> invoices)
        {
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
                log.Info("Json data:" + data);
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

        public APIResult CancelInvoice(Company company, InvoiceVAT invoice)
        {
            string userPass = company.Username + ":" + company.Password;
            string apiLink = company.Url + @"/InvoiceAPI/InvoiceWS/cancelTransactionInvoice?supplierTaxCode=" + company.TaxCode + "&invoiceNo=" + invoice.invNo + "&strIssueDate=" + invoice.dateIssue.ToString("yyyyMMddHHmmss") + "&additionalReferenceDesc=" + invoice.Fkey + "&additionalReferenceDate=" + invoice.ArisingDate.ToString("yyyyMMddHHmmss");
            string autStr = Base64Encode(userPass);

            APIResult result = new APIResult();
            var message = Request(apiLink, "", autStr, "GET", "");
            result = ParseResult<APIResult>(message);
            return result;
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
