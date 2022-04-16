using InvoiceService.Models;
using InvoiceService.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceService
{
    public class ProcessService
    {
        public InvoiceModels ConvertToAPIModel(InvoiceVAT invoice, bool isAdjust, out string errormsg)
        {
            errormsg = "";
            try
            {
                InvoiceInfo objInvoice = new InvoiceInfo();
                objInvoice.transactionUuid = invoice.Fkey;
                if (invoice.TT78 == 0)
                {
                    objInvoice.invoiceType = !string.IsNullOrEmpty(invoice.Pattern) ? invoice.Pattern.Substring(0, 6) : "";// "01GTKT";
                    objInvoice.templateCode = !string.IsNullOrEmpty(invoice.Pattern) ? invoice.Pattern : "";
                    objInvoice.invoiceSeries = !string.IsNullOrEmpty(invoice.Serial) ? invoice.Serial : ""; //ko nhập lấy mặc định
                }
                else
                {
                    objInvoice.invoiceType = !string.IsNullOrEmpty(invoice.Pattern) ? invoice.Pattern.Substring(0, 1) : "";// "1";
                    objInvoice.templateCode = !string.IsNullOrEmpty(invoice.Pattern) ? invoice.Pattern : "";
                    objInvoice.invoiceSeries = !string.IsNullOrEmpty(invoice.Serial) ? invoice.Serial : ""; //ko nhập lấy mặc định
                }

                //Ngày hóa đơn
                DateTimeOffset arisingDate = new DateTimeOffset(invoice.ArisingDate, new TimeSpan(+7, 0, 0));
                objInvoice.currencyCode = "VND";
                objInvoice.adjustmentType = "1";
                if (isAdjust)
                {
                    DateTimeOffset adjustDate = new DateTimeOffset(invoice.AdjustDate, new TimeSpan(+7, 0, 0));
                    objInvoice.invoiceIssuedDate = invoice.AdjustDate < DateTime.Now.Date ? adjustDate.AddHours(23).AddMinutes(59).AddSeconds(59).ToString("yyyy-MM-ddTHH:mm:sszzz") : "";
                    objInvoice.adjustmentType = "5";
                    objInvoice.adjustmentInvoiceType = "1";
                    objInvoice.originalInvoiceId = invoice.OriginalInvoiceId;
                    objInvoice.originalInvoiceIssueDate = arisingDate.ToString("yyyy-MM-dd");
                    objInvoice.additionalReferenceDesc = "";
                    objInvoice.additionalReferenceDate = NumberUtil.ConvertToUnixTime(invoice.AdjustDate).ToString();
                }
                else
                {
                    objInvoice.invoiceIssuedDate = invoice.ArisingDate < DateTime.Now.Date ? arisingDate.AddHours(23).AddMinutes(59).AddSeconds(59).ToString("yyyy-MM-ddTHH:mm:sszzz") : "";
                }
                objInvoice.paymentStatus = "true";

                objInvoice.paymentType = invoice.PaymentMethod;
                //objInvoice.paymentTypeName = "0";
                objInvoice.cusGetInvoiceRight = true;
                objInvoice.buyerIdType = "1";
                objInvoice.buyerIdNo = invoice.No;

                BuyerInfo objBuyer = new BuyerInfo();
                objBuyer.buyerAddressLine = invoice.CusAddress;
                objBuyer.buyerCode = invoice.Warehouse;
                objBuyer.buyerIdNo = invoice.No;
                objBuyer.buyerBankName = invoice.CusBankName;
                objBuyer.buyerBankAccount = invoice.CusBankNo;
                objBuyer.buyerIdType = "1";
                objBuyer.buyerName = invoice.Buyer;
                objBuyer.buyerLegalName = invoice.CusName;
                objBuyer.buyerPhoneNumber = invoice.CusPhone;
                objBuyer.buyerTaxCode = invoice.CusTaxCode;
                objBuyer.buyerEmail = invoice.CusEmail;
                SellerInfo objSeller = new SellerInfo();
                //objSeller.sellerCode = invoice.Warehouse;
                //objSeller.sellerAddressLine = invoice.ComAddress;
                //objSeller.sellerBankAccount = invoice.ComBankNo;
                //objSeller.sellerBankName = invoice.ComBankName;
                //objSeller.sellerEmail = "";
                //objSeller.sellerLegalName = invoice.ComName;
                //objSeller.sellerPhoneNumber = invoice.ComPhone;
                //objSeller.sellerTaxCode = invoice.ComTaxCode;


                //Danh sách hàng hóa
                List<ItemInfo> lstItem = new List<ItemInfo>();

                foreach (var pro in invoice.Products)
                {

                    ItemInfo item = new ItemInfo();
                    item.discount = "0.0";
                    item.itemCode = pro.Code;
                    item.itemDiscount = pro.DiscountAmount.ToString();
                    item.itemName = pro.Name;
                    item.itemTotalAmountWithoutTax = pro.Total.ToString();
                    item.lineNumber = "1";
                    item.quantity = pro.Quantity.ToString();
                    item.taxAmount = pro.VATAmount.ToString();
                    item.taxPercentage = pro.VATRate.ToString();
                    item.unitName = pro.Unit;
                    item.unitPrice = pro.Price.ToString();
                    item.expDate = pro.ProDate;
                    item.itemNote = pro.Type;
                    if (isAdjust)
                    {
                        item.itemName = "Điều chỉnh giảm tiền hàng, tiền thuế của hàng hóa/dịch vụ: " + pro.Name;
                        item.adjustmentTaxAmount = "1";
                        item.isIncreaseItem = "false";
                    }
                    lstItem.Add(item);
                }

                if (isAdjust)
                {
                    lstItem.Add(new ItemInfo
                    {
                        selection = "2",
                        isIncreaseItem = "false",
                        lineNumber = "1",
                        taxAmount = "0",
                        itemName = "Điều chỉnh giảm tiền hàng, tiền thuế cho hóa đơn điện tử mẫu " + invoice.Pattern + " ký hiệu " + invoice.Serial + " số " + invoice.OriginalInvoiceId.Substring(6, invoice.OriginalInvoiceId.Length - 6) + " lập ngày " + arisingDate.ToString("dd/MM/yyyy") + " số tiền: " + invoice.Amount.ToString(),
                    });
                }


                SummarizeInfo objSummary = new SummarizeInfo();
                objSummary.discountAmount = "0";
                objSummary.settlementDiscountAmount = "0";
                objSummary.taxPercentage = invoice.VATRate.ToString();
                objSummary.totalAmountWithTax = invoice.Amount.ToString();
                objSummary.totalAmountWithTaxInWords = NumberUtil.DocSoThanhChu(objSummary.totalAmountWithTax);
                objSummary.totalTaxAmount = invoice.VATAmount.ToString();
                objSummary.totalAmountWithoutTax = invoice.Total.ToString();
                objSummary.sumOfTotalLineAmountWithoutTax = invoice.Total.ToString();
                objSummary.isTotalAmountPos = false;
                objSummary.isTotalAmtWithoutTaxPos = false;
                objSummary.isTotalTaxAmountPos = false;
                objSummary.isDiscountAmtPos = false;


                List<Metadata> lstMetaData = new List<Metadata>();

                lstMetaData.Add(new Metadata
                {
                    invoiceCustomFieldId = 1233,
                    keyTag = "ordMonth",
                    stringValue = invoice.OrderMonth,
                    valueType = "text",
                    keyLabel = "Tháng đặt hàng_Ord Month",
                });

                lstMetaData.Add(new Metadata
                {
                    invoiceCustomFieldId = 1281,
                    keyTag = "ordNo",
                    stringValue = invoice.No,
                    valueType = "text",
                    keyLabel = "Số đơn hàng_Ord No",
                });
                lstMetaData.Add(new Metadata
                {
                    invoiceCustomFieldId = 1282,
                    keyTag = "ordDate",
                    dateValue = invoice.OrderDate.ToString("yyyy-MM-dd"),
                    valueType = "date",
                    keyLabel = "Ngày đặt hàng_Ord Date",
                });

                lstMetaData.Add(new Metadata
                {
                    invoiceCustomFieldId = 1234,
                    keyTag = "ordType",
                    stringValue = invoice.OrderType,
                    valueType = "text",
                    keyLabel = "Loại đơn hàng_Ord Type",
                });

                lstMetaData.Add(new Metadata
                {
                    invoiceCustomFieldId = 1283,
                    keyTag = "wareHouse",
                    stringValue = invoice.Warehouse,
                    valueType = "text",
                    keyLabel = "Kho_Warehouse",
                });

                lstMetaData.Add(new Metadata
                {
                    invoiceCustomFieldId = 1284,
                    keyTag = "herbalifeID",
                    stringValue = invoice.CusCode,
                    valueType = "text",
                    keyLabel = "Mã số Herbalife _ID",
                });

                lstMetaData.Add(new Metadata
                {
                    invoiceCustomFieldId = 1235,
                    keyTag = "FQSID",
                    stringValue = invoice.FQSID,
                    valueType = "text",
                    keyLabel = "FQS Mã số Herbalife _ID",
                });

                lstMetaData.Add(new Metadata
                {
                    invoiceCustomFieldId = 1285,
                    keyTag = "FQSName",
                    stringValue = invoice.FQSName,
                    valueType = "text",
                    keyLabel = "Tên_name",
                });

                lstMetaData.Add(new Metadata
                {
                    invoiceCustomFieldId = 1286,
                    keyTag = "QSID",
                    stringValue = invoice.QSID,
                    valueType = "text",
                    keyLabel = "QS Mã số Herbalife_ID",
                });

                lstMetaData.Add(new Metadata
                {
                    invoiceCustomFieldId = 1236,
                    keyTag = "QSName",
                    stringValue = invoice.QSName,
                    valueType = "text",
                    keyLabel = "Tên_QSname",
                });

                if (!string.IsNullOrEmpty(invoice.VolumePoints))
                {
                    lstMetaData.Add(new Metadata
                    {
                        invoiceCustomFieldId = 1287,
                        keyTag = "vp",
                        stringValue = invoice.VolumePoints,
                        valueType = "text",
                        keyLabel = "Điểm doanh số _VP",
                    });
                }

                lstMetaData.Add(new Metadata
                {
                    invoiceCustomFieldId = 1288,
                    keyTag = "salesChannel",
                    stringValue = invoice.OrderChannel,
                    valueType = "text",
                    keyLabel = "Sales Channel",
                });

                lstMetaData.Add(new Metadata
                {
                    invoiceCustomFieldId = 1237,
                    keyTag = "date",
                    stringValue = "",
                    valueType = "date",
                    keyLabel = "Ngày_date",
                });

                InvoiceModels model = new InvoiceModels();
                model.generalInvoiceInfo = objInvoice;
                model.buyerInfo = objBuyer;
                model.sellerInfo = objSeller;
                model.itemInfo = lstItem;
                model.summarizeInfo = objSummary;
                model.metadata = lstMetaData;
                if (!string.IsNullOrEmpty(invoice.PaymentMethod))
                {
                    model.payments = new List<Payment>() { new Payment() { paymentMethodName = invoice.PaymentMethod } };
                }
                model.taxBreakdowns = new List<TaxBreakdown>();
                model.taxBreakdowns.Add(new TaxBreakdown() { taxPercentage = invoice.VATRate, taxableAmount = invoice.Total, taxAmount = invoice.VATAmount });

                return model;
            }
            catch (Exception ex)
            {
                errormsg = ex.Message;
                log.Error(ex);
                return null;
            }
        }
    }
}
