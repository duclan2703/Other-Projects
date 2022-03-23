using Parse.Core.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parse.Core.Domain;
using System.Data;
using System.IO;
using System.Reflection;
using LumenWorks.Framework.IO.Csv;
using Parse.Core.Models;
using Parse.Core.Utils;
using System.Configuration;
using Parse.Core;
using System.Net;
using System.Net.Security;
using Newtonsoft.Json;
using System.Globalization;
using FX.Core;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;

namespace Parse.AEON
{
    public class AEONService : IAEONService
    {
        public DataTable ConvertCSVToData(string filePath)
        {
            DataTable dTable = new DataTable();

            dTable.Rows.Clear();
            dTable.Columns.Clear();

            // Set Column cho Table
            InvObj invModel = new InvObj();
            var lstCol = invModel.GetType().GetProperties().Select(c => c.Name).ToList();
            for (int i = 0; i < lstCol.Count; i++)
            {
                dTable.Columns.Add(lstCol[i].ToString());
            }
            //List<string> lines = File.ReadAllLines(filePath).ToList();
            //for (int i = 0; i < lines.Count; i += 3)
            //{
            //    List<string> line = (lines[i]).Split(new string[] { "\",\"" }, StringSplitOptions.None).ToList();
            //    var dRow = dTable.NewRow();
            //    for (int j = 0; j < line.Count; j++)
            //    {

            //        var a= line[j].Trim();
            //        dRow[j + 1] = line[j].Trim();
            //    }
            //    if (!CheckRowEmty(dRow))
            //    {
            //        dRow[0] = i + 1;
            //        dTable.Rows.Add(dRow);
            //    }
            //}
            using (CsvReader csv = new CsvReader(new StreamReader(filePath), false))
            {
                if (csv.FieldCount > 89 || csv.FieldCount < 87)
                    throw new Exception("File không đúng định dạng, số cột!");
                var rowNumber = 0;
                while (csv.ReadNextRecord())
                {
                    rowNumber++;
                    DataRow row = dTable.NewRow();
                    for (int i = 1; i < lstCol.Count; i++)
                    {
                        row[i] = csv[i - 1];
                    }
                    if (!CheckRowEmty(row))
                    {
                        row[0] = rowNumber;
                        dTable.Rows.Add(row);
                    }
                }
            }
            return dTable;
        }
        public void PrintFileMapping(int id, string filePath)
        {
            ILogDetailService detailService = IoC.Resolve<ILogDetailService>();
            var lstDetail = detailService.GetByLogFile(id);

            // khởi tạo wb rỗng
            var wb = new XSSFWorkbook();
            // Tạo ra 1 sheet
            ISheet sheet = wb.CreateSheet();

            // Bắt đầu ghi lên sheet

            // Tạo row
            var row0 = sheet.CreateRow(0);
            // Merge lại row đầu 3 cột
            row0.CreateCell(0); // tạo ra cell trc khi merge
            CellRangeAddress cellMerge = new CellRangeAddress(0, 0, 0, 2);
            sheet.AddMergedRegion(cellMerge);
            row0.GetCell(0).SetCellValue("Mapping số hóa đơn");

            // Ghi tên cột ở row 1
            var row1 = sheet.CreateRow(1);
            row1.CreateCell(0).SetCellValue("Số hóa đơn trên file");
            row1.CreateCell(1).SetCellValue("Số hóa đơn trên SInvoice");
            row1.CreateCell(2).SetCellValue("Serial");

            // bắt đầu duyệt mảng và ghi tiếp tục
            int rowIndex = 2;
            foreach (var item in lstDetail)
            {
                // tao row mới
                var newRow = sheet.CreateRow(rowIndex);

                // set giá trị
                newRow.CreateCell(0).SetCellValue(item.FolioNo);
                newRow.CreateCell(1).SetCellValue(item.InvNo);
                newRow.CreateCell(2).SetCellValue(item.Serial);

                // tăng index
                rowIndex++;
            }

            // save file
            FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            wb.Write(fs);
        }

        public void FileProcessing(string filePath, string pattern, string serial, ref int invSuccess, ref int invTotal, ref string mesError)
        {
            DataTable dTable = new DataTable();
            List<InvoiceVAT> lstInv = new List<InvoiceVAT>();
            List<InvObj> lstObj = new List<InvObj>();
            List<UploadError> lstError = new List<UploadError>();

            //Lấy số SAP tương ứng pattern và serial
            string ptsr = pattern + "-" + serial;
            int sap = 0;
            ISapUploadService sapService = IoC.Resolve<ISapUploadService>();
            var sapObj = sapService.Getbykey(ptsr);
            if (sapObj != null)
                sap = Convert.ToInt32(sapObj.Sap);

            //Lấy dữ liệu ra DataTable
            dTable = ConvertCSVToData(filePath);
            //Lấy dữ liệu ra list InvObj
            lstObj = ConvertTableToListObj<InvObj>(dTable);
            //Lấy dữ liệu ra list InvoiceVAT
            lstInv = ConvertToListInvoiceVAT(lstObj, pattern, serial, sap, ref invTotal, ref lstError);

            //Viết log detail
            ILogDetailService detailService = IoC.Resolve<ILogDetailService>();
            if (lstError.Count > 0)
            {
                mesError = "File dữ liệu có lỗi!";
                //Viết bussiness log
                IBussinessLogService logService = IoC.Resolve<IBussinessLogService>();
                BussinessLog bussinessLog = new BussinessLog();
                bussinessLog.FileName = filePath;
                bussinessLog.Count = invTotal;
                bussinessLog.CreateDate = DateTime.Now;
                bussinessLog.Error = mesError;
                logService.CreateNew(bussinessLog);
                logService.CommitChanges();
                foreach (var item in lstError)
                {
                    LogDetail log = new LogDetail();
                    log.Type = "Đọc file";
                    log.LogId = bussinessLog.Id;
                    log.FolioNo = item.FolioNo;
                    foreach (var error in item.ErrorList)
                    {
                        log.Detail += " - Dòng " + error.rowNumber.ToString() + ": " + error.Detail;
                    }
                    detailService.CreateNew(log);
                }

                detailService.CommitChanges();
            }
            else
            {
                //Viết bussiness log
                IBussinessLogService logService = IoC.Resolve<IBussinessLogService>();
                BussinessLog bussinessLog = new BussinessLog();
                bussinessLog.FileName = filePath;
                bussinessLog.Count = invTotal;
                bussinessLog.CreateDate = DateTime.Now;
                logService.CreateNew(bussinessLog);
                logService.CommitChanges();
                //Group theo ngày
                //Halt_esct update ngày 26/06/2020 --BEGIN--
                //var lstGrp = lstInv.GroupBy(c => c.ArisingDate).OrderByDescending(c => c.Key);
                var lstGrp = lstInv.GroupBy(c => c.ArisingDate).OrderBy(c => c.Key);
                //--END--

                invSuccess = 0;
                int invError = 0;
                //Phát hành hóa đơn
                foreach (var lstinv in lstGrp)
                {
                    foreach (var inv in lstinv.OrderBy(c => c.SapNumber).ToList())
                    {
                        var InvApi = ConvertToAPIModel(inv);
                        APIResult result = SendInvoice(InvApi);

                        LogDetail log = new LogDetail();
                        log.Type = "Phát hành";
                        log.LogId = bussinessLog.Id;
                        log.FolioNo = inv.FolioNo;
                        var sapnumber = Convert.ToInt32(log.FolioNo);
                        if (string.IsNullOrEmpty(result.errorCode))
                        {
                            log.Detail = "Phát hành thành công";
                            log.InvNo = result.result.invoiceNo;
                            log.Serial = result.result.invoiceNo.Substring(0, 6);
                            invSuccess += 1;

                            //sapnumber
                            if (sapnumber > sap)
                                sap = sapnumber;
                        }
                        else
                        {
                            log.Detail = result.errorCode + " - " + result.description;
                            invError += 1;
                        }
                        detailService.CreateNew(log);
                        detailService.CommitChanges();
                        if (invError > 0)
                            break;
                    }
                    if (invError > 0)
                        break;
                }

                //Lưu số sap lớn nhất
                if (sapObj != null)
                {
                    sapObj.Sap = sap.ToString();
                    sapObj.UpdateDate = DateTime.Now;
                    sapService.Update(sapObj);
                }
                else
                {
                    SapUpload newSap = new SapUpload();
                    newSap.Pattern = pattern;
                    newSap.Serial = serial;
                    newSap.Id = ptsr;
                    newSap.Sap = sap.ToString();
                    newSap.UpdateDate = DateTime.Now;
                    sapService.CreateNew(newSap);
                }
                sapService.CommitChanges();

                mesError = string.Format("Upload và phát hành hóa đơn thành công: " + invSuccess + "/" + invTotal);
                bussinessLog.Error = mesError;
                logService.Update(bussinessLog);
                logService.CommitChanges();

            }
        }

        public List<InvoiceVAT> ConvertToListInvoiceVAT(List<InvObj> lstObj, string pattern, string serial, int sap, ref int invTotal, ref List<UploadError> lstError)
        {


            List<InvoiceVAT> lstInv = new List<InvoiceVAT>();
            var lstGrp = lstObj.GroupBy(c => c.SoHoaDon).ToList();
            invTotal = lstGrp.Count;
            string codeTax = AppContext.Current.company.TaxCode;
            decimal dVal = 0;
            float fVal = 0;
            int iVal = 0;
            DateTime dateVal;

            //Parse dữ liệu hóa đơn
            foreach (var item in lstGrp)
            {
                //Thông số viết log cho lỗi
                UploadError invError = new UploadError();
                List<ErrorDetail> lstErDetail = new List<ErrorDetail>();

                //Hóa đơn mới
                InvoiceVAT inv = new InvoiceVAT();
                List<ProductInv> lstProduct = new List<ProductInv>();
                var firstItem = item.First();

                //Thông tin chung hóa đơn
                //Pattern
                inv.Pattern = pattern;
                //Serial
                inv.Serial = serial;
                //Số hóa đơn
                var sohoadon = new string(firstItem.SoHoaDon.Where(Char.IsDigit).ToArray());
                if (int.TryParse(sohoadon, out iVal))
                {
                    if (iVal > sap)
                    {
                        inv.FolioNo = sohoadon;
                        inv.SapNumber = iVal;
                    }
                    else
                    {
                        lstErDetail.Add(new ErrorDetail { rowNumber = firstItem.rowNumber, Detail = "Số hóa đơn không hợp lệ" });
                    }
                }
                else
                {
                    lstErDetail.Add(new ErrorDetail { rowNumber = firstItem.rowNumber, Detail = "Số hóa đơn không hợp lệ" });
                }
                invError.FolioNo = sohoadon;

                //Tên gian hàng khách
                inv.Buyer = firstItem.TenGianHang;
                //Tên đơn vị khách
                inv.CusName = firstItem.cusTenDonVi;
                //Mã số thuế khách
                inv.CusTaxCode = firstItem.cusMST;
                //Địa chỉ khách
                inv.CusAddress = firstItem.cusDiachi2;
                //Email khách
                inv.CusEmail = firstItem.Email;
                //Hình thức thanh toán
                inv.PaymentMethod = firstItem.PaymentMethod;
                //Hình thức bán hàng
                inv.SellMethod = firstItem.HinhThucBanHangSkip;
                //Ngày hóa đơn
                if (DateTime.TryParseExact((new string(firstItem.Date.Where(Char.IsDigit).ToArray())), "ddMMyyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateVal))
                    inv.ArisingDate = dateVal;
                else
                {
                    lstErDetail.Add(new ErrorDetail { rowNumber = firstItem.rowNumber, Detail = "Sai thông tin ngày tháng" });
                }
                //Fkey
                inv.Fkey = codeTax + "-" + inv.FolioNo;

                //Thông tin thanh toán hóa đơn
                //Tiền hàng không thuế
                if (decimal.TryParse(firstItem.TongTienHangKhongChiuThue.Replace(".", ""), out dVal))
                    inv.TotalNo = dVal;
                else
                {
                    lstErDetail.Add(new ErrorDetail { rowNumber = firstItem.rowNumber, Detail = "Sai thông tin hàng không chịu thuế" });
                }
                //Thuế hàng không thuế
                if (decimal.TryParse(firstItem.TongThueHangKhongChiuThue.Replace(".", ""), out dVal))
                    inv.VATAmountNo = dVal;
                else
                {
                    lstErDetail.Add(new ErrorDetail { rowNumber = firstItem.rowNumber, Detail = "Sai thông tin hàng không chịu thuế" });
                }
                //Tiền thanh toán hàng không thuế
                if (decimal.TryParse(firstItem.TongTienThanhToanHangKhongChiuThue.Replace(".", ""), out dVal))
                    inv.AmountNo = dVal;
                else
                {
                    lstErDetail.Add(new ErrorDetail { rowNumber = firstItem.rowNumber, Detail = "Sai thông tin hàng không chịu thuế" });
                }
                //Tiền hàng thuế 0%
                if (decimal.TryParse(firstItem.TongTienHangThue0.Replace(".", ""), out dVal))
                    inv.Total0 = dVal;
                else
                {
                    lstErDetail.Add(new ErrorDetail { rowNumber = firstItem.rowNumber, Detail = "Sai thông tin hàng chịu thuế 0%" });
                }
                //Thuế hàng thuế 0%
                if (decimal.TryParse(firstItem.TongThue0.Replace(".", ""), out dVal))
                    inv.VATAmount0 = dVal;
                else
                {
                    lstErDetail.Add(new ErrorDetail { rowNumber = firstItem.rowNumber, Detail = "Sai thông tin hàng chịu thuế 0%" });
                }
                //Tiền thanh toán hàng thuế 0%
                if (decimal.TryParse(firstItem.TongThanhToanThue0.Replace(".", ""), out dVal))
                    inv.Amount0 = dVal;
                else
                {
                    lstErDetail.Add(new ErrorDetail { rowNumber = firstItem.rowNumber, Detail = "Sai thông tin hàng chịu thuế 0%" });
                }
                //Tiền hàng thuế 5%
                if (decimal.TryParse(firstItem.TongTienHangThue5.Replace(".", ""), out dVal))
                    inv.Total5 = dVal;
                else
                {
                    lstErDetail.Add(new ErrorDetail { rowNumber = firstItem.rowNumber, Detail = "Sai thông tin hàng chịu thuế 5%" });
                }
                //Thuế hàng thuế 5%
                if (decimal.TryParse(firstItem.TongThue5.Replace(".", ""), out dVal))
                    inv.VATAmount5 = dVal;
                else
                {
                    lstErDetail.Add(new ErrorDetail { rowNumber = firstItem.rowNumber, Detail = "Sai thông tin hàng chịu thuế 5%" });
                }
                //Tiền thanh toán hàng thuế 5%
                if (decimal.TryParse(firstItem.TongThanhToanThue5.Replace(".", ""), out dVal))
                    inv.Amount5 = dVal;
                else
                {
                    lstErDetail.Add(new ErrorDetail { rowNumber = firstItem.rowNumber, Detail = "Sai thông tin hàng chịu thuế 5%" });
                }
                //Tiền hàng thuế 10%
                if (decimal.TryParse(firstItem.TongTienHangThue10.Replace(".", ""), out dVal))
                    inv.Total10 = dVal;
                else
                {
                    lstErDetail.Add(new ErrorDetail { rowNumber = firstItem.rowNumber, Detail = "Sai thông tin hàng chịu thuế 10%" });
                }
                //Thuế hàng thuế 10%
                if (decimal.TryParse(firstItem.TongThue10.Replace(".", ""), out dVal))
                    inv.VATAmount10 = dVal;
                else
                {
                    lstErDetail.Add(new ErrorDetail { rowNumber = firstItem.rowNumber, Detail = "Sai thông tin hàng chịu thuế 10%" });
                }
                //Tiền thanh toán hàng thuế 10%
                if (decimal.TryParse(firstItem.TongThanhToanThue10.Replace(".", ""), out dVal))
                    inv.Amount10 = dVal;
                else
                {
                    lstErDetail.Add(new ErrorDetail { rowNumber = firstItem.rowNumber, Detail = "Sai thông tin hàng chịu thuế 10%" });
                }

                //Tổng tiền hàng
                if (decimal.TryParse(firstItem.TongTienHang2.Replace(".", ""), out dVal))
                    inv.Total = dVal;
                else
                {
                    lstErDetail.Add(new ErrorDetail { rowNumber = firstItem.rowNumber, Detail = "Sai thông tin tổng tiền hàng cả hóa đơn" });
                }
                //Tổng thuế
                if (decimal.TryParse(firstItem.TongThue2.Replace(".", ""), out dVal))
                    inv.VATAmount = dVal;
                else
                {
                    lstErDetail.Add(new ErrorDetail { rowNumber = firstItem.rowNumber, Detail = "Sai thông tin tổng tiền thuế cả hóa đơn" });
                }
                //Tổng tiền thanh toán
                if (decimal.TryParse(firstItem.TongTienThanhToan2.Replace(".", ""), out dVal))
                    inv.Amount = dVal;
                else
                {
                    lstErDetail.Add(new ErrorDetail { rowNumber = firstItem.rowNumber, Detail = "Sai thông tin tổng tiền thanh toán cả hóa đơn" });
                }
                //Số tiền bằng chữ
                //inv.AmountInWords = !string.IsNullOrEmpty(firstItem.SoTienBangChu) ? firstItem.SoTienBangChu : "";

                //Thông tin sản phẩm
                foreach (var product in item)
                {
                    ProductInv pro = new ProductInv();
                    pro.Name = product.TenHang;
                    pro.Unit = product.DVT;
                    if (string.IsNullOrEmpty(product.SoLuong))
                        pro.Quantity = 0;
                    else
                    {
                        if (decimal.TryParse(product.SoLuong.Replace(".", ""), out dVal))
                            pro.Quantity = dVal;
                        else
                        {
                            lstErDetail.Add(new ErrorDetail { rowNumber = product.rowNumber, Detail = "Sai thông tin số lượng hàng" });
                        }
                    }
                    if (decimal.TryParse(product.DonGia.Replace(".", ""), out dVal))
                        pro.Price = dVal;
                    else
                    {
                        lstErDetail.Add(new ErrorDetail { rowNumber = product.rowNumber, Detail = "Sai thông tin đơn giá hàng" });
                    }
                    if (decimal.TryParse(product.ThanhTien.Replace(".", ""), out dVal))
                        pro.Total = dVal;
                    else
                    {
                        lstErDetail.Add(new ErrorDetail { rowNumber = product.rowNumber, Detail = "Sai thông tin cộng tiền hàng" });
                    }
                    if (float.TryParse(product.ThueSuat.Replace("%", ""), out fVal))
                        pro.VATRate = fVal;
                    else
                    {
                        pro.VATRate = 0;
                    }
                    if (decimal.TryParse(product.ThueGTGT.Replace(".", ""), out dVal))
                        pro.VATAmount = dVal;
                    else
                    {
                        lstErDetail.Add(new ErrorDetail { rowNumber = product.rowNumber, Detail = "Sai thông tin tiền thuế hàng" });
                    }
                    if (decimal.TryParse(product.Cong.Replace(".", ""), out dVal))
                        pro.Amount = dVal;
                    else
                    {
                        lstErDetail.Add(new ErrorDetail { rowNumber = product.rowNumber, Detail = "Sai thông tin tiền hàng" });
                    }

                    lstProduct.Add(pro);
                }
                if (lstErDetail.Count > 0)
                {
                    invError.ErrorList = lstErDetail;
                    lstError.Add(invError);
                }
                else
                {
                    inv.Products = lstProduct;
                    lstInv.Add(inv);
                }
            }
            return lstInv;
        }

        public List<T> ConvertTableToListObj<T>(DataTable dt)
        {
            var columnNames = dt.Columns.Cast<DataColumn>()
                    .Select(c => c.ColumnName)
                    .ToList();
            var properties = typeof(T).GetProperties();

            return dt.AsEnumerable().Select(row =>
            {
                var objT = Activator.CreateInstance<T>();
                foreach (var pro in properties)
                {
                    if (columnNames.Contains(pro.Name))
                    {
                        PropertyInfo pI = objT.GetType().GetProperty(pro.Name);
                        pro.SetValue(objT, row[pro.Name] == DBNull.Value ? null : Convert.ChangeType(row[pro.Name], pI.PropertyType), null);
                    }
                }
                return objT;
            }).ToList();
        }

        public bool CheckRowEmty(DataRow row)
        {
            var itemArray = row.ItemArray;
            if (itemArray == null || string.IsNullOrEmpty(row[44].ToString()))
                return true;
            return itemArray.All(x => string.IsNullOrWhiteSpace(x.ToString()));
        }

        public InvoiceModels ConvertToAPIModel(InvoiceVAT invoice)
        {
            InvoiceInfo objInvoice = new InvoiceInfo();
            objInvoice.transactionUuid = invoice.Fkey;
            objInvoice.invoiceType = !string.IsNullOrEmpty(invoice.Pattern) ? invoice.Pattern.Substring(0, 6) : "";// "01GTKT";
            objInvoice.templateCode = !string.IsNullOrEmpty(invoice.Pattern) ? invoice.Pattern : "";
            objInvoice.invoiceSeries = !string.IsNullOrEmpty(invoice.Serial) ? invoice.Serial : ""; //ko nhập lấy mặc định
            objInvoice.invoiceIssuedDate = invoice.ArisingDate < DateTime.Now.Date ? NumberUtil.ConvertToUnixTime(invoice.ArisingDate.AddHours(23).AddMinutes(59).AddSeconds(59)).ToString() : "";
            objInvoice.currencyCode = "VND";
            objInvoice.adjustmentType = "1";
            objInvoice.paymentStatus = "true";

            //objInvoice.paymentType = "TM/CK";
            //objInvoice.paymentTypeName = "0";
            objInvoice.cusGetInvoiceRight = true;
            objInvoice.buyerIdType = "1";
            objInvoice.buyerIdNo = invoice.CusTaxCode;

            BuyerInfo objBuyer = new BuyerInfo();
            objBuyer.buyerAddressLine = invoice.CusAddress;
            objBuyer.buyerCode = invoice.CusCode;
            objBuyer.buyerIdNo = invoice.CusTaxCode;
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
                item.itemDiscount = pro.DiscountAmount.ToString();
                item.itemName = pro.Name;
                item.itemTotalAmountWithoutTax = pro.Total.ToString();
                item.lineNumber = "1";
                item.quantity = pro.Quantity.ToString();
                item.taxAmount = pro.VATAmount.ToString();
                if (pro.VATRate == 0)
                    pro.VATRate = -2;
                item.taxPercentage = pro.VATRate.ToString();
                item.unitName = pro.Unit;
                item.unitPrice = pro.Price.ToString();
                item.expDate = pro.ProDate;
                item.itemNote = pro.Remark;

                lstItem.Add(item);
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

            //Dữ liệu trường động
            List<Metadata> lstMetaData = new List<Metadata>();
            lstMetaData.Add(new Metadata
            {
                invoiceCustomFieldId = 1801,
                keyTag = "boothName",
                stringValue = invoice.Buyer,
                valueType = "text",
                keyLabel = "Ten gian hang",
            });

            //Dữ liệu thuế
            List<TaxBreakdown> lstTax = new List<TaxBreakdown>();
            if (invoice.TotalNo != 0)
            {
                lstTax.Add(new TaxBreakdown
                {
                    taxPercentage = -2,
                    taxableAmount = invoice.TotalNo,
                    taxAmount = 0,
                });
            }

            if (invoice.Total0 != 0)
            {
                lstTax.Add(new TaxBreakdown
                {
                    taxPercentage = 0,
                    taxableAmount = invoice.Total0,
                    taxAmount = 0,
                });
            }

            if (invoice.Total5 != 0)
            {
                lstTax.Add(new TaxBreakdown
                {
                    taxPercentage = 5,
                    taxableAmount = invoice.Total5,
                    taxAmount = invoice.VATAmount5,
                });
            }
            if (invoice.Total10 != 0)
            {
                lstTax.Add(new TaxBreakdown
                {
                    taxPercentage = 10,
                    taxableAmount = invoice.Total10,
                    taxAmount = invoice.VATAmount10,
                });
            }


            InvoiceModels model = new InvoiceModels();
            model.generalInvoiceInfo = objInvoice;
            model.buyerInfo = objBuyer;
            model.sellerInfo = objSeller;
            model.itemInfo = lstItem;
            model.summarizeInfo = objSummary;
            model.metadata = lstMetaData;
            model.payments = new List<Payment>() { new Payment() { paymentMethodName = invoice.PaymentMethod } };
            model.taxBreakdowns = lstTax;
            //model.taxBreakdowns.Add(new TaxBreakdown() { taxPercentage = (decimal)invoice.VATRate, taxableAmount = invoice.Total, taxAmount = invoice.VATAmount });

            return model;
        }

        public static APIResult SendInvoice(InvoiceModels invoice)
        {
            string userPass = AppContext.Current.company.UserName + ":" + AppContext.Current.company.PassWord;
            string codeTax = AppContext.Current.company.TaxCode;
            string apiLink = AppContext.Current.company.Domain + @"/InvoiceAPI/InvoiceWS/createInvoice/" + codeTax;
            string autStr = Base64Encode(userPass);
            string contentType = "application/json";


            string data = JsonConvert.SerializeObject(invoice);
            string result = Request(apiLink, data, autStr, "POST", contentType);
            var resultConverted = ParseResult<APIResult>(result);
            return resultConverted;
        }

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

            resultConverted.createInvoiceOutputs = lstResult;

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
