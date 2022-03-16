using FluentFTP;
using InvoiceService.Models;
using InvoiceService.Utils;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace InvoiceService
{
    public class HerbalifeService
    {
        static string mappingFile = AppDomain.CurrentDomain.BaseDirectory + "\\Config\\warehouse.json";
        static string mappingData = File.ReadAllText(mappingFile);
        static Warehouse mapping = JsonConvert.DeserializeObject<Warehouse>(mappingData);
        static ILog log = LogManager.GetLogger(typeof(HerbalifeService));

        public void Processing()
        {

            //thông in Ftp
            var ftpFile = AppDomain.CurrentDomain.BaseDirectory + "\\Config\\ftpinfo.json";
            var ftpData = File.ReadAllText(ftpFile);
            var ftpInfo = JsonConvert.DeserializeObject<FtpInfo>(ftpData);

            //thông tin company
            var apiFile = AppDomain.CurrentDomain.BaseDirectory + "\\Config\\apiinfo.json";
            var apiData = File.ReadAllText(apiFile);
            var apiInfo = JsonConvert.DeserializeObject<ApiInfo>(apiData);

            // create an FTP client
            FtpClient client = new FtpClient(ftpInfo.FtpLink);

            // if you don't specify login credentials, we use the "anonymous" user account
            client.Credentials = new NetworkCredential(ftpInfo.FtpUsername, ftpInfo.FtpPassword);

            // begin connecting to the server
            client.Connect();

            string fileName = "";
            string fileFullName = "";
            string filePath = "";
            string orderNumber = "";
            int type = 1;

            try
            {
#if DEBUG
                #region test
                string fileTest = @"D:\Herbalife\NTS_VNXSWH_VN03766910_1.xml";
                type = 1;
                string messageTest = "";
                string mesErrorTest = "";
                List<InvoiceVAT> InvTest;
                DataSet dSetTest = new DataSet();
                dSetTest.ReadXml(fileTest);
                InvTest = ConvertToInvoiceVAT(dSetTest, false, ref mesErrorTest);
                orderNumber = InvTest.FirstOrDefault().No;

                //Phát hành hóa đơn
                if (string.IsNullOrEmpty(mesErrorTest))
                {
                    foreach (var item in InvTest)
                    {
                        var InvApi = ConvertToAPIModel(item, false, out string Errorms);
                        APIResult result = SendInvoice(apiInfo, InvApi, item.ComTaxCode);
                        WriteNewResult(fileTest, result, item.ArisingDate, item.Pattern, item.Serial);
                        if (string.IsNullOrEmpty(result.errorCode))
                        {
                            messageTest = "Issue invoice successfully: " + item.No;
                        }
                        else
                        {
                            messageTest = "Fail to issue invoice: " + item.No;
                        }
                        log.Error(messageTest);
                    }
                }
                else
                {
                    WriteErrorResult(fileTest, mesErrorTest);
                    log.Error("Fail to issue invoice: " + InvTest.FirstOrDefault().No + mesErrorTest);
                }
                #endregion
#else
                //Xử lý file trong folder

                foreach (FtpListItem file in client.GetListing(ftpInfo.ReIssuePath).OrderBy(c => c.Modified))
                {
                    if (file.Type != FtpFileSystemObjectType.File)
                        continue;
                    fileName = file.Name;
                    fileFullName = file.FullName;
                    type = 1;
                    string message = "";
                    string mesError = "";
                    string tempFile = AppDomain.CurrentDomain.BaseDirectory + "Temp/temp.xml";
                    File.WriteAllText(tempFile, "");
                    client.DownloadFile(tempFile, file.FullName);
                    List<InvoiceVAT> lstInv = new List<InvoiceVAT>();
                    DataSet dSet = new DataSet();
                    dSet.ReadXml(tempFile);
                    lstInv = ConvertToInvoiceVAT(dSet, true, ref mesError);
                    if (lstInv == null)
                        continue;
                    orderNumber = lstInv.FirstOrDefault().No;

                    //Phát hành hóa đơn
                    if (string.IsNullOrEmpty(mesError))
                    {
                        foreach (var Inv in lstInv)
                        {
                            var InvApi = ConvertToAPIModel(Inv, false, out string Error);
                            APIResult result = SendInvoice(apiInfo, InvApi, Inv.ComTaxCode);
                            WriteNewResult(tempFile, result, Inv.ArisingDate, Inv.Pattern, Inv.Serial);
                            if (string.IsNullOrEmpty(result.errorCode))
                            {
                                message = "Issue invoice successfully: " + Inv.No + Inv.TaxFreight.ToString("0");
                                //File.Delete(tempFile);
                            }
                            else
                            {
                                message = "Fail to issue invoice: " + Inv.No + Inv.TaxFreight.ToString("0");
                                //client.UploadFile(tempFile, ftpInfo.IssueFailedPath + "/" + file.Name);
                                //File.Delete(tempFile);
                                //client.DeleteFile(file.FullName);
                                SendFailedMail(file.Name, Inv.No, result.errorCode, result.description, "", type);
                            }
                            log.Error(string.Format("{0} {1}", message, result.description));
                        }
                        client.MoveFile(file.FullName, ftpInfo.IssueSuccessPath + "/" + file.Name);
                    }
                    else
                    {
                        WriteErrorResult(tempFile, mesError);
                        //client.UploadFile(tempFile, ftpInfo.IssueFailedPath + "/" + file.Name);
                        //File.Delete(tempFile);
                        //client.DeleteFile(file.FullName);
                        SendFailedMail(file.Name, lstInv.FirstOrDefault().No, "", "", mesError, type);
                        log.Error("Fail to issue invoice: " + lstInv.FirstOrDefault().No + mesError);
                    }
                }

                foreach (FtpListItem file in client.GetListing(ftpInfo.NewIssueXMLPath).OrderBy(c => c.Modified))
                {
                    if (file.Type != FtpFileSystemObjectType.File)
                        continue;
                    fileName = file.Name;
                    fileFullName = file.FullName;
                    type = 1;
                    string message = "";
                    string mesError = "";
                    string tempFile = AppDomain.CurrentDomain.BaseDirectory + "Temp/temp.xml";
                    File.WriteAllText(tempFile, "");
                    client.DownloadFile(tempFile, file.FullName);
                    List<InvoiceVAT> lstInv;
                    DataSet dSet = new DataSet();
                    dSet.ReadXml(tempFile);
                    lstInv = ConvertToInvoiceVAT(dSet, false, ref mesError);
                    orderNumber = lstInv.FirstOrDefault().No;

                    //Phát hành hóa đơn
                    if (string.IsNullOrEmpty(mesError))
                    {
                        foreach (var Inv in lstInv)
                        {
                            var InvApi = ConvertToAPIModel(Inv, false, out string Error);
                            APIResult result = SendInvoice(apiInfo, InvApi, Inv.ComTaxCode);
                            WriteNewResult(tempFile, result, Inv.ArisingDate, Inv.Pattern, Inv.Serial);
                            if (string.IsNullOrEmpty(result.errorCode))
                            {
                                message = "Issue invoice successfully: " + Inv.No + Inv.TaxFreight.ToString("0");
                                //File.Delete(tempFile);
                            }
                            else
                            {
                                message = "Fail to issue invoice: " + Inv.No + Inv.TaxFreight.ToString("0");
                                //client.UploadFile(tempFile, ftpInfo.IssueFailedPath + "/" + file.Name);
                                //File.Delete(tempFile);
                                //client.DeleteFile(file.FullName);
                                SendFailedMail(file.Name, Inv.No, result.errorCode, result.description, "", type);
                            }
                            log.Error(string.Format("{0} {1}", message, result.description));
                        }
                        client.MoveFile(file.FullName, ftpInfo.IssueSuccessPath + "/" + file.Name);
                    }
                    else
                    {
                        WriteErrorResult(tempFile, mesError);
                        //client.UploadFile(tempFile, ftpInfo.IssueFailedPath + "/" + file.Name);
                        //File.Delete(tempFile);
                        //client.DeleteFile(file.FullName);
                        SendFailedMail(file.Name, lstInv.FirstOrDefault().No, "", "", mesError, type);
                        log.Error("Fail to issue invoice: " + lstInv.FirstOrDefault().No + mesError);
                    }
                }

                foreach (FtpListItem file in client.GetListing(ftpInfo.AdjustXMLPath).OrderBy(c => c.Modified))
                {
                    if (file.Type != FtpFileSystemObjectType.File)
                        continue;
                    fileName = file.Name;
                    fileFullName = file.FullName;
                    type = 2;
                    string message = "";
                    string mesError = "";
                    string tempFile = AppDomain.CurrentDomain.BaseDirectory + "Temp/temp.xml";
                    File.WriteAllText(tempFile, "");
                    client.DownloadFile(tempFile, file.FullName);
                    List<InvoiceVAT> lstInv;
                    DataSet dSet = new DataSet();
                    dSet.ReadXml(tempFile);
                    lstInv = ConvertToInvoiceVAT(dSet, false, ref mesError);
                    orderNumber = lstInv.FirstOrDefault().No;

                    //Phát hành hóa đơn
                    if (string.IsNullOrEmpty(mesError))
                    {
                        foreach (var Inv in lstInv)
                        {
                            var InvApi = ConvertToAPIModel(Inv, false, out string Error);
                            APIResult result = SendInvoice(apiInfo, InvApi, Inv.ComTaxCode);
                            WriteNewResult(tempFile, result, Inv.ArisingDate, Inv.Pattern, Inv.Serial);
                            if (string.IsNullOrEmpty(result.errorCode))
                            {
                                message = "Adjust invoice successfully: " + Inv.No + Inv.TaxFreight.ToString("0");
                                //File.Delete(tempFile);
                            }
                            else
                            {
                                message = "Fail to adjust invoice: " + Inv.No + Inv.TaxFreight.ToString("0");
                                //client.UploadFile(tempFile, ftpInfo.AdjustFailedPath + "/" + file.Name);
                                //File.Delete(tempFile);
                                //client.DeleteFile(file.FullName);
                                SendFailedMail(file.Name, Inv.No, result.errorCode, result.description, "", type);
                            }
                            log.Error(string.Format("{0} {1}", message, result.description));
                        }
                        client.MoveFile(file.FullName, ftpInfo.IssueSuccessPath + "/" + file.Name);
                    }
                    else
                    {
                        WriteErrorResult(tempFile, mesError);
                        //client.UploadFile(tempFile, ftpInfo.AdjustFailedPath + "/" + file.Name);
                        //File.Delete(tempFile);
                        //client.DeleteFile(file.FullName);
                        SendFailedMail(file.Name, lstInv.FirstOrDefault().No, "", "", mesError, type);
                        log.Error("Fail to adjust invoice: " + lstInv.FirstOrDefault().No + mesError);
                    }
                }

                foreach (FtpListItem file in client.GetListing(ftpInfo.CancelXMLPath).OrderBy(c => c.Modified))
                {
                    if (file.Type != FtpFileSystemObjectType.File)
                        continue;
                    fileName = file.Name;
                    fileFullName = file.FullName;
                    type = 3;
                    string message = "";
                    string mesError = "";
                    string tempFile = AppDomain.CurrentDomain.BaseDirectory + "Temp/temp.xml";
                    File.WriteAllText(tempFile, "");
                    client.DownloadFile(tempFile, file.FullName);
                    CancelModels model = ConvertToCancelModel(tempFile, ref mesError);
                    orderNumber = model.additionalReferenceDesc;

                    if (string.IsNullOrEmpty(mesError))
                    {
                        var result = CancelInvoice(apiInfo, model);
                        WriteCancelResult(tempFile, result);
                        if (string.IsNullOrEmpty(result.errorCode))
                        {
                            message = "Cancel invoice successfully: " + orderNumber;
                            client.MoveFile(file.FullName, ftpInfo.IssueSuccessPath + "/" + file.Name);
                            //File.Delete(tempFile);
                        }
                        else
                        {
                            message = "Fail to cancel invoice: " + orderNumber;
                            //client.UploadFile(tempFile, ftpInfo.CancelFailedPath + "/" + file.Name);
                            //File.Delete(tempFile);
                            //client.DeleteFile(file.FullName);
                            SendFailedMail(file.Name, orderNumber, result.errorCode, result.description, "", type);
                        }
                        log.Error(string.Format("{0} {1}", message, result.description));
                    }
                    else
                    {
                        WriteErrorResult(tempFile, mesError);
                        //client.UploadFile(tempFile, ftpInfo.CancelFailedPath + "/" + file.Name);
                        //File.Delete(tempFile);
                        //client.DeleteFile(file.FullName);
                        SendFailedMail(file.Name, orderNumber, "", "", mesError, type);
                        log.Error("Fail to cancel invoice: " + orderNumber + mesError);
                    }
                }
#endif
            }
            catch (Exception ex)
            {
                log.Error(ex);
                if (ex.Message.StartsWith("ExceptionError: "))
                {
                    //WriteErrorResult(filePath, ex.Message);
                    //if (type == 1)
                    //    client.UploadFile(filePath, ftpInfo.IssueFailedPath + "/" + fileName);
                    //else
                    //    client.UploadFile(filePath, ftpInfo.CancelFailedPath + "/" + fileName);
                    //File.Delete(filePath);
                    //client.DeleteFile(fileFullName);
                    SendFailedMail(fileName, orderNumber, "", "", ex.Message, type);
                }
            }
            finally
            {
                client.Disconnect();
            }
        }

        private List<InvoiceVAT> ConvertToInvoiceVAT(DataSet dSet, bool isResend, ref string mesError)
        {
            mesError = "";
            try
            {
                List<InvoiceVAT> lstinv = new List<InvoiceVAT>();
                List<decimal> taxList = new List<decimal>();
                int taxCount = dSet.Tables["Tax"].Rows.Count;
                for (int i = 0; i < taxCount; i++)
                {
                    DataRow taxRow = dSet.Tables["Tax"].Rows[i];
                    var taxrate = taxRow["Tax_Rate"].ToString();
                    if (decimal.TryParse(taxrate, out decimal dVal))
                        taxList.Add(dVal);
                    else
                        mesError += " - error parse Tax_Rate";
                }
                if (taxList.Count > 1)
                {
                    foreach (var tax in taxList)
                    {
                        try
                        {
                            InvoiceVAT inv = new InvoiceVAT();
                            ILog log = LogManager.GetLogger(typeof(HerbalifeService));

                            DateTime dateVal = DateTime.Now;
                            decimal dVal = 0;

                            //Parse thông tin chung hóa đơn
                            DataRow infoRow = dSet.Tables["General"].Rows[0];
                            inv.No = infoRow["OrderNumber"].ToString();
                            //inv.FacturaNumber = infoRow["Factura_number"].ToString();
                            inv.OrderMonth = infoRow["OrderMonth"].ToString();
                            if (DateTime.TryParseExact(infoRow["OrderDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateVal))
                                inv.OrderDate = dateVal;
                            else
                            {
                                mesError += " - error parse OrderDate";
                            }
                            if (DateTime.TryParseExact(infoRow["NTS_Date"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateVal))
                                inv.ArisingDate = dateVal;
                            else
                            {
                                mesError += " - error parse NTS_Date";
                            }

                            inv.OrderChannel = infoRow["OrderChannel"].ToString();
                            inv.OrderType = infoRow["OrderType"].ToString();

                            inv.Warehouse = infoRow["WarehouseNumber"].ToString();
                            var warehouse = mapping.WarehouseMapping.FirstOrDefault(c => c.Warehouse.Contains(inv.Warehouse));
                            if (warehouse != null)
                            {
                                inv.ComTaxCode = warehouse.Taxcode;
                                if (isResend)
                                {
                                    inv.Pattern = warehouse.ResendPattern;
                                    inv.Serial = warehouse.ResendSerial;
                                }
                                else
                                {
                                    inv.Pattern = warehouse.Pattern;
                                    inv.Serial = warehouse.Serial;
                                }
                            }
                            else
                            {
                                mesError += " - warehouse code invalid";
                            }
                            //Fkey
                            if (!string.IsNullOrEmpty(inv.No))
                                inv.Fkey = string.Format("{0}{1}{2}{3}", inv.ComTaxCode.Replace("-", ""), inv.No, tax.ToString("0"), DateTime.Now.ToString("ddMMyymmss"));

                            //Parse thông tin khác
                            if (dSet.Tables.Contains("Order"))
                            {
                                DataRow orderRow = dSet.Tables["Order"].Rows[0];
                                inv.VolumePoints = orderRow["VolumePoints"].ToString();
                            }

                            //Parse thông tin người mua
                            DataRow dsRow = dSet.Tables["DistributorDetails"].Rows[0];
                            DataRow buyerRow = dSet.Tables["BillTo"].Rows[0];
                            inv.CusCode = dsRow["DSID"].ToString();
                            inv.CusEmail = dsRow["Primary_EmailAddress"].ToString();
                            inv.Buyer = buyerRow["DSLastName"].ToString() + " " + buyerRow["DSFirstName"].ToString();
                            inv.CusAddress = buyerRow["Address1"].ToString() + " " + buyerRow["Address2"].ToString() + " " + buyerRow["Zipcode"].ToString() + " " + buyerRow["City"].ToString() + " " + buyerRow["Country"].ToString();

                            //Parse thông tin người bán
                            DataRow sellerRow = dSet.Tables["Lineage"].Rows[0];
                            inv.FQSID = sellerRow["FQSID"].ToString();
                            inv.FQSName = sellerRow["FQSName"].ToString();
                            inv.QSID = sellerRow["QSID"].ToString();
                            inv.QSName = sellerRow["QSName"].ToString();

                            //Parse thông tin thuế
                            //DataRow taxRow = dSet.Tables["Tax"].Rows[0];
                            //var taxrate = taxRow["Tax_Rate"].ToString();
                            //if (decimal.TryParse(taxrate, out dVal))
                            inv.VATRate = tax;
                            //else
                            //{
                            //    mesError += " - error parse Tax_Rate";
                            //}

                            //Parse thông tin payment
                            List<string> lstpayment = new List<string>();
                            if (dSet.Tables.Contains("PaymentReference"))
                            {
                                inv.PaymentMethod = dSet.Tables["PaymentReference"].Rows[0]["PaymentType"].ToString();
                                //int paymentCount = dSet.Tables["PaymentReference"].Rows.Count;
                                //if (paymentCount <= 1)
                                //    inv.PaymentMethod = dSet.Tables["PaymentReference"].Rows[0]["PaymentType"].ToString();
                                //else
                                //{
                                //    for (int i = 0; i < paymentCount; i++)
                                //        lstpayment.Add(dSet.Tables["PaymentReference"].Rows[i]["PaymentType"].ToString());
                                //    inv.PaymentMethod = string.Join("+", lstpayment);
                                //}
                            }

                            //Parse thông tin vận chuyển
                            //if (taxList.IndexOf(tax) == 0)
                            //{
                            if (tax == warehouse.TaxFreight)
                            {
                                DataRow freightRow = dSet.Tables["OrderPricing"].Rows[0];
                                if (decimal.TryParse(freightRow["Total_Freight"].ToString(), out dVal))
                                    inv.Freight = Math.Abs(dVal);
                                if (decimal.TryParse(freightRow["Total_Tax_Freight"].ToString(), out dVal))
                                    inv.TaxFreight = Math.Abs(dVal);
                            }

                            //}
                            //DataRow freightRow = dSet.Tables["Tax"].Rows[taxList.IndexOf(tax)];
                            //if (decimal.TryParse(freightRow["Total_Freight"].ToString(), out dVal))
                            //    inv.Freight = Math.Abs(dVal);
                            //inv.TaxFreight = Math.Round(inv.Freight * warehouse.Tax / 100, MidpointRounding.AwayFromZero);

                            //Parse thông tin hóa đơn điều chỉnh
                            if (dSet.Tables.Contains("InvoiceAdjustment"))
                            {
                                DataRow oldInvRow = dSet.Tables["InvoiceAdjustment"].Rows[0];
                                inv.OriginalInvoiceId = oldInvRow["TaxInvoiceNumber"].ToString();
                                //Halt_esct chỉnh lại ngày phát hành hóa đơn điều chỉnh 14-05-2020 --BEGIN--
                                if (DateTime.TryParseExact(infoRow["LocalPrintDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateVal))
                                {
                                    inv.AdjustDate = dateVal;
                                    log.Info("NGày phát hành của hóa đơn điều chỉnh là :" + dateVal);
                                }
                                else
                                {
                                    mesError += " - error parse LocalPrintDate";
                                }
                                //if (DateTime.TryParseExact(oldInvRow["OrderCancellationDate"].ToString(), "dd-MMM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateVal))
                                //{
                                //    inv.AdjustDate = dateVal;
                                //}
                                //else
                                //{
                                //    mesError += " - error parse OrderCancellationDate";
                                //}
                                //Halt_esct chỉnh lại ngày phát hành hóa đơn điều chỉnh 14-05-2020 --END--
                            }

                            //Parse thông tin sản phẩm
                            List<ProductInv> lstProduct = new List<ProductInv>();
                            DataTable itemTb = dSet.Tables["Item"];
                            List<ProductObj> lstObj = ConvertTableToListObj<ProductObj>(itemTb);
                            var lstItem = lstObj.Where(c => c.QtyOrdered != null).ToList();

                            inv.VATAmount = 0;
                            inv.Total = 0;
                            inv.Amount = 0;

                            lstItem = lstItem.Where(x => x.TaxRate_1 == tax.ToString()).ToList();
                            foreach (var item in lstItem)
                            {
                                ProductInv prod = new ProductInv();
                                //Halt_esct 22-05-2020 Fix lấy mã sản phẩm --BEGIN--
                                //prod.Code = item.StockingSKU;
                                prod.Code = item.ProductCode;
                                //Halt_esct 22-05-2020 Fix lấy mã sản phẩm --END--
                                prod.Name = item.ItemDescription;
                                prod.Type = item.ProductType;
                                prod.Unit = item.UOM;
                                if (decimal.TryParse(item.QtyOrdered, out dVal))
                                    prod.Quantity = Math.Abs(dVal);
                                else
                                {
                                    mesError += " - error parse item quantity";
                                }
                                if (decimal.TryParse(item.DiscountAmt, out dVal))
                                    prod.DiscountAmount = Math.Abs(dVal);
                                else
                                {
                                    mesError += " - error parse item discount";
                                }
                                if (decimal.TryParse(item.ExtendedPrice, out dVal))
                                    prod.Total = Math.Abs(dVal) - prod.DiscountAmount;
                                else
                                {
                                    mesError += " - error parse item amount";
                                }
                                if (prod.Quantity != 0)
                                    prod.Price = prod.Total / prod.Quantity;
                                else
                                {
                                    mesError += " - quantity = 0";
                                }
                                if (decimal.TryParse(item.TaxRate_1, out dVal))
                                    prod.VATRate = dVal;
                                else
                                {
                                    mesError += " - error parse item tax rate";
                                }
                                prod.VATAmount = Math.Round(prod.Total * prod.VATRate / 100, MidpointRounding.AwayFromZero);
                                prod.Amount = prod.Total + prod.VATAmount;
                                inv.Total += prod.Total;
                                inv.Amount += prod.Amount;
                                inv.VATAmount += prod.VATAmount;
                                lstProduct.Add(prod);
                            }
                            inv.Products = lstProduct;
                            inv.Total = inv.Total + inv.Freight;
                            inv.VATAmount = inv.VATAmount + inv.TaxFreight;
                            inv.Amount = inv.Amount + inv.Freight + inv.TaxFreight;
                            lstinv.Add(inv);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("ExceptionError: " + ex.Message);
                        }
                    }
                    return lstinv;
                }
                else
                {
                    try
                    {
                        InvoiceVAT inv = new InvoiceVAT();
                        DateTime dateVal = DateTime.Now;
                        decimal dVal = 0;

                        //Parse thông tin chung hóa đơn
                        DataRow infoRow = dSet.Tables["General"].Rows[0];
                        inv.No = infoRow["OrderNumber"].ToString();
                        //inv.FacturaNumber = infoRow["Factura_number"].ToString();
                        inv.OrderMonth = infoRow["OrderMonth"].ToString();
                        if (DateTime.TryParseExact(infoRow["OrderDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateVal))
                            inv.OrderDate = dateVal;
                        else
                        {
                            mesError += " - error parse OrderDate";
                        }
                        if (DateTime.TryParseExact(infoRow["NTS_Date"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateVal))
                            inv.ArisingDate = dateVal;
                        else
                        {
                            mesError += " - error parse NTS_Date";
                        }

                        inv.OrderChannel = infoRow["OrderChannel"].ToString();
                        inv.OrderType = infoRow["OrderType"].ToString();

                        inv.Warehouse = infoRow["WarehouseNumber"].ToString();
                        var warehouse = mapping.WarehouseMapping.FirstOrDefault(c => c.Warehouse.Contains(inv.Warehouse));
                        inv.TT78 = warehouse.TT78;
                        if (warehouse != null)
                        {
                            inv.ComTaxCode = warehouse.Taxcode;
                            if (isResend)
                            {
                                inv.Pattern = warehouse.ResendPattern;
                                inv.Serial = warehouse.ResendSerial;
                            }
                            else
                            {
                                inv.Pattern = warehouse.Pattern;
                                inv.Serial = warehouse.Serial;
                            }
                        }
                        else
                        {
                            mesError += " - warehouse code invalid";
                        }
                        //Fkey
                        if (!string.IsNullOrEmpty(inv.No))
                            inv.Fkey = string.Format("{0}{1}{2}", inv.ComTaxCode.Replace("-", ""), inv.No, DateTime.Now.ToString("ddMMyymmss"));

                        //Parse thông tin khác
                        if (dSet.Tables.Contains("Order"))
                        {
                            DataRow orderRow = dSet.Tables["Order"].Rows[0];
                            inv.VolumePoints = orderRow["VolumePoints"].ToString();
                        }

                        //Parse thông tin người mua
                        DataRow dsRow = dSet.Tables["DistributorDetails"].Rows[0];
                        DataRow buyerRow = dSet.Tables["BillTo"].Rows[0];
                        inv.CusCode = dsRow["DSID"].ToString();
                        inv.CusEmail = dsRow["Primary_EmailAddress"].ToString();
                        inv.Buyer = buyerRow["DSLastName"].ToString() + " " + buyerRow["DSFirstName"].ToString();
                        inv.CusAddress = buyerRow["Address1"].ToString() + " " + buyerRow["Address2"].ToString() + " " + buyerRow["Zipcode"].ToString() + " " + buyerRow["City"].ToString() + " " + buyerRow["Country"].ToString();

                        //Parse thông tin người bán
                        DataRow sellerRow = dSet.Tables["Lineage"].Rows[0];
                        inv.FQSID = sellerRow["FQSID"].ToString();
                        inv.FQSName = sellerRow["FQSName"].ToString();
                        inv.QSID = sellerRow["QSID"].ToString();
                        inv.QSName = sellerRow["QSName"].ToString();

                        //Parse thông tin thuế
                        DataRow taxRow = dSet.Tables["Tax"].Rows[0];
                        var taxrate = taxRow["Tax_Rate"].ToString();
                        if (decimal.TryParse(taxrate, out dVal))
                            inv.VATRate = dVal;
                        else
                        {
                            mesError += " - error parse Tax_Rate";
                        }

                        //Parse thông tin payment
                        List<string> lstpayment = new List<string>();
                        if (dSet.Tables.Contains("PaymentReference"))
                        {
                            inv.PaymentMethod = dSet.Tables["PaymentReference"].Rows[0]["PaymentType"].ToString();
                            //int paymentCount = dSet.Tables["PaymentReference"].Rows.Count;
                            //if (paymentCount <= 1)
                            //    inv.PaymentMethod = dSet.Tables["PaymentReference"].Rows[0]["PaymentType"].ToString();
                            //else
                            //{
                            //    for (int i = 0; i < paymentCount; i++)
                            //        lstpayment.Add(dSet.Tables["PaymentReference"].Rows[i]["PaymentType"].ToString());
                            //    inv.PaymentMethod = string.Join("+", lstpayment);
                            //}
                        }

                        //Parse thông tin vận chuyển
                        DataRow freightRow = dSet.Tables["OrderPricing"].Rows[0];
                        if (decimal.TryParse(freightRow["Total_Freight"].ToString(), out dVal))
                            inv.Freight = Math.Abs(dVal);
                        if (decimal.TryParse(freightRow["Total_Tax_Freight"].ToString(), out dVal))
                            inv.TaxFreight = Math.Abs(dVal);

                        //Parse thông tin hóa đơn điều chỉnh
                        if (dSet.Tables.Contains("InvoiceAdjustment"))
                        {
                            DataRow oldInvRow = dSet.Tables["InvoiceAdjustment"].Rows[0];
                            inv.OriginalInvoiceId = oldInvRow["TaxInvoiceNumber"].ToString();
                            //Halt_esct chỉnh lại ngày phát hành hóa đơn điều chỉnh 14-05-2020 --BEGIN--
                            if (DateTime.TryParseExact(infoRow["LocalPrintDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateVal))
                            {
                                inv.AdjustDate = dateVal;
                                log.Info("NGày phát hành của hóa đơn điều chỉnh là :" + dateVal);
                            }
                            else
                            {
                                mesError += " - error parse LocalPrintDate";
                            }
                            //if (DateTime.TryParseExact(oldInvRow["OrderCancellationDate"].ToString(), "dd-MMM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateVal))
                            //{
                            //    inv.AdjustDate = dateVal;
                            //}
                            //else
                            //{
                            //    mesError += " - error parse OrderCancellationDate";
                            //}
                            //Halt_esct chỉnh lại ngày phát hành hóa đơn điều chỉnh 14-05-2020 --END--
                        }

                        //Parse thông tin sản phẩm
                        List<ProductInv> lstProduct = new List<ProductInv>();
                        DataTable itemTb = dSet.Tables["Item"];
                        List<ProductObj> lstObj = ConvertTableToListObj<ProductObj>(itemTb);
                        var lstItem = lstObj.Where(c => c.QtyOrdered != null).ToList();

                        inv.VATAmount = 0;
                        inv.Total = 0;
                        inv.Amount = 0;

                        foreach (var item in lstItem)
                        {
                            ProductInv prod = new ProductInv();
                            //Halt_esct 22-05-2020 Fix lấy mã sản phẩm --BEGIN--
                            //prod.Code = item.StockingSKU;
                            prod.Code = item.ProductCode;
                            //Halt_esct 22-05-2020 Fix lấy mã sản phẩm --END--
                            prod.Name = item.ItemDescription;
                            prod.Type = item.ProductType;
                            prod.Unit = item.UOM;
                            if (decimal.TryParse(item.QtyOrdered, out dVal))
                                prod.Quantity = Math.Abs(dVal);
                            else
                            {
                                mesError += " - error parse item quantity";
                            }
                            if (decimal.TryParse(item.DiscountAmt, out dVal))
                                prod.DiscountAmount = Math.Abs(dVal);
                            else
                            {
                                mesError += " - error parse item discount";
                            }
                            if (decimal.TryParse(item.ExtendedPrice, out dVal))
                                prod.Total = Math.Abs(dVal) - prod.DiscountAmount;
                            else
                            {
                                mesError += " - error parse item amount";
                            }
                            if (prod.Quantity != 0)
                                prod.Price = prod.Total / prod.Quantity;
                            else
                            {
                                mesError += " - quantity = 0";
                            }
                            if (decimal.TryParse(item.TaxRate_1, out dVal))
                                prod.VATRate = dVal;
                            else
                            {
                                mesError += " - error parse item tax rate";
                            }
                            prod.VATAmount = Math.Round(prod.Total * prod.VATRate / 100, MidpointRounding.AwayFromZero);
                            prod.Amount = prod.Total + prod.VATAmount;
                            inv.Total += prod.Total;
                            inv.Amount += prod.Amount;
                            inv.VATAmount += prod.VATAmount;
                            lstProduct.Add(prod);
                        }
                        inv.Products = lstProduct;
                        inv.Total = inv.Total + inv.Freight;
                        inv.VATAmount = inv.VATAmount + inv.TaxFreight;
                        inv.Amount = inv.Amount + inv.Freight + inv.TaxFreight;
                        lstinv.Add(inv);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("ExceptionError: " + ex.Message);
                    }
                    return lstinv;
                }
            }
            catch (Exception ex)
            {
                mesError = ex.Message;
                log.Error(ex);
                return null;
            }
        }

        public List<T> ConvertTableToListObj<T>(DataTable dt)
        {
            var columnNames = dt.Columns.Cast<DataColumn>()
                    .Select(c => c.ColumnName)
                    .ToList();
            var properties = typeof(T).GetProperties();

            var a = dt.AsEnumerable().ToList();
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

        public CancelModels ConvertToCancelModel(string filePath, ref string mesError)
        {
            try
            {
                CancelModels model = new CancelModels();

                DateTime dateVal;
                DataSet dSet = new DataSet();
                dSet.ReadXml(filePath);
                DataRow infoRow = dSet.Tables["CancelInvoice"].Rows[0];
                string whcode = infoRow["WarehouseNumber"].ToString();
                var warehouse = mapping.WarehouseMapping.FirstOrDefault(c => c.Warehouse.Contains(whcode));
                if (warehouse != null)
                {
                    model.codeTax = warehouse.Taxcode;
                }
                else
                {
                    mesError += " - warehouse code invalid";
                }
                model.invNo = infoRow["TaxInvoiceNumber"].ToString();
                model.additionalReferenceDesc = infoRow["OrderNumber"].ToString();
                if (DateTime.TryParseExact(infoRow["NTSDate"].ToString(), "dd-MMM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateVal))
                    model.dateIssue = dateVal.ToString("yyyyMMddHHmmss");
                else
                {
                    mesError += " - sai định dạng ngày hóa đơn";
                }
                if (DateTime.TryParseExact(infoRow["OrderCancellationDate"].ToString(), "dd-MMM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateVal))
                    model.addRefDate = dateVal.ToString("yyyyMMddHHmmss");
                else
                {
                    mesError += " - sai định dạng ngày hủy";
                }
                return model;
            }
            catch (Exception ex)
            {
                throw new Exception("ExceptionError: " + ex.Message);
            }
        }

        public void WriteNewResult(string filePath, APIResult result, DateTime arisingdate, string pattern, string serial)
        {
            // đọc xml
            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);

            // tạo node mới
            XmlElement issueDateElement = doc.CreateElement("IssueDate");
            // gán dữ liệu vào node
            issueDateElement.InnerText = arisingdate.ToString("dd/MM/yyyy");
            // append node vào root node
            doc.DocumentElement.AppendChild(issueDateElement);

            // tạo node mới
            XmlElement patternElement = doc.CreateElement("Pattern");
            // gán dữ liệu vào node
            patternElement.InnerText = pattern;
            // append node vào root node
            doc.DocumentElement.AppendChild(patternElement);

            // tạo node mới
            XmlElement serialElement = doc.CreateElement("Serial");
            // gán dữ liệu vào node
            serialElement.InnerText = serial;
            // append node vào root node
            doc.DocumentElement.AppendChild(serialElement);

            if (result.result != null)
            {
                // tạo node mới
                XmlElement invnoElement = doc.CreateElement("InvNo");
                // gán dữ liệu vào node
                invnoElement.InnerText = result.result.invoiceNo;
                // append node vào root node
                doc.DocumentElement.AppendChild(invnoElement);
            }
            else
            {
                // tạo node mới
                XmlElement errorcodeElement = doc.CreateElement("ErrorCode");
                // gán dữ liệu vào node
                errorcodeElement.InnerText = result.errorCode;
                // append node vào root node
                doc.DocumentElement.AppendChild(errorcodeElement);

                // tạo node mới
                XmlElement errordescElement = doc.CreateElement("ErrorDescription");
                // gán dữ liệu vào node
                errordescElement.InnerText = result.description;
                // append node vào root node
                doc.DocumentElement.AppendChild(errordescElement);
            }

            // save lại file
            doc.Save(filePath);

        }

        public void WriteCancelResult(string filePath, APIResult result)
        {
            // đọc xml
            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);

            // tạo node mới
            XmlElement errorcodeElement = doc.CreateElement("ErrorCode");
            // gán dữ liệu vào node
            errorcodeElement.InnerText = result.errorCode;
            // append node vào root node
            doc.DocumentElement.AppendChild(errorcodeElement);

            // tạo node mới
            XmlElement errordescElement = doc.CreateElement("ErrorDescription");
            // gán dữ liệu vào node
            errordescElement.InnerText = result.description;
            // append node vào root node
            doc.DocumentElement.AppendChild(errordescElement);

            // save lại file
            doc.Save(filePath);
        }

        public void WriteErrorResult(string filePath, string mesError)
        {
            // đọc xml
            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);

            // tạo node mới
            XmlElement errorparseElement = doc.CreateElement("ErrorParse");
            // gán dữ liệu vào node
            errorparseElement.InnerText = "Lỗi: " + mesError;
            // append node vào root node
            doc.DocumentElement.AppendChild(errorparseElement);

            // save lại file
            doc.Save(filePath);
        }

        public void WriteExceptionResult(string filePath, string mesError)
        {
            // đọc xml
            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);

            // tạo node mới
            XmlElement exceptionElement = doc.CreateElement("Exception");
            // gán dữ liệu vào node
            exceptionElement.InnerText = "Lỗi:" + mesError;
            // append node vào root node
            doc.DocumentElement.AppendChild(exceptionElement);

            // save lại file
            doc.Save(filePath);
        }

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

                if (invoice.Freight != 0)
                {
                    ItemInfo freight = new ItemInfo();
                    freight.selection = "2";
                    freight.isIncreaseItem = "false";
                    freight.itemName = "Phí giao hàng/Freight";
                    freight.taxPercentage = mapping.WarehouseMapping.FirstOrDefault(c => c.Warehouse.Contains(invoice.Warehouse)).TaxFreight.ToString();
                    freight.itemTotalAmountWithoutTax = invoice.Freight.ToString();
                    freight.taxAmount = invoice.TaxFreight.ToString();
                    freight.lineNumber = "1";
                    if (isAdjust)
                    {
                        freight.itemName = "Điều chỉnh giảm tiền hàng, tiền thuế của hàng hóa/dịch vụ: Phí giao hàng/Freight";
                        freight.adjustmentTaxAmount = "1";
                        freight.isIncreaseItem = "false";
                    }
                    lstItem.Add(freight);
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
                    keyLabel = "Tên_Name",
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

        public static APIResult SendInvoice(ApiInfo apiInfo, InvoiceModels invoice, string taxcode)
        {
            string userPass = apiInfo.Username + ":" + apiInfo.Password;
            string apiLink = apiInfo.Url + @"/InvoiceAPI/InvoiceWS/createInvoice/" + taxcode;
            string autStr = Base64Encode(userPass);
            string contentType = "application/json";


            string data = JsonConvert.SerializeObject(invoice);
            string result = Request(apiLink, data, autStr, "POST", contentType);
            var resultConverted = ParseResult<APIResult>(result);
            return resultConverted;
        }

        public APIResult CancelInvoice(ApiInfo apiInfo, CancelModels model)
        {
            string userPass = apiInfo.Username + ":" + apiInfo.Password;
            string apiLink = apiInfo.Url + @"/InvoiceAPI/InvoiceWS/cancelTransactionInvoice?supplierTaxCode=" + model.codeTax + "&invoiceNo=" + model.invNo + "&strIssueDate=" + model.dateIssue + "&additionalReferenceDesc=" + model.additionalReferenceDesc + "&additionalReferenceDate=" + model.addRefDate;
            string autStr = Base64Encode(userPass);
            string contentType = "application/json";

            APIResult result = new APIResult();
            var message = Request(apiLink, "", autStr, "GET", "");
            result = ParseResult<APIResult>(message);
            return result;
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
        private static void SendFailedMail(string fileName, string ordernumber, string errorCode, string errorDesc, string errorParse, int type)
        {
            //thông in mail
            var mailFile = Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]) + "\\Config\\mailinfo.json";
            var mailData = File.ReadAllText(mailFile);
            var mailInfo = JsonConvert.DeserializeObject<MailInfo>(mailData);

            string subject;
            string body;
            if (type == 1)
            {
                if (string.IsNullOrEmpty(errorParse))
                {
                    subject = "Issue invoice failed: " + ordernumber;
                    body = "There was an error issue invoice: " + ordernumber + ", XML file name: " + fileName + ". <br> Error code: " + errorCode + ". <br> Error description: " + errorDesc;
                }
                else
                {
                    subject = "Issue invoice failed: " + ordernumber;
                    body = "There was an error issue invoice: " + ordernumber + ", XML file name: " + fileName + ". <br> Error: " + errorParse + ".";
                }
            }
            else if (type == 2)
            {
                if (string.IsNullOrEmpty(errorParse))
                {
                    subject = "Adjust invoice failed: " + ordernumber;
                    body = "There was an error adjust invoice: " + ordernumber + ", XML file name: " + fileName + ". <br> Error code: " + errorCode + ". <br> Error description: " + errorDesc;
                }
                else
                {
                    subject = "Adjust invoice failed: " + ordernumber;
                    body = "There was an error adjust invoice: " + ordernumber + ", XML file name: " + fileName + ". <br> Error: " + errorParse + ".";
                }
            }
            else
            {
                if (string.IsNullOrEmpty(errorParse))
                {
                    subject = "Cancel invoice failed: " + ordernumber;
                    body = "There was an error cancel invoice: " + ordernumber + ", XML file name: " + fileName + ". <br> Error code: " + errorCode + ". <br> Error description: " + errorDesc;
                }
                else
                {
                    subject = "Cancel invoice failed: " + ordernumber;
                    body = "There was an error cancel invoice: " + ordernumber + ", XML file name: " + fileName + ". <br> Error: " + errorParse + ".";
                }
            }

            var lstTo = mailInfo.To.Split(';').ToList();
            MailMessage message = new MailMessage(mailInfo.From, lstTo[0], subject, body);
            message.BodyEncoding = Encoding.UTF8;
            message.IsBodyHtml = true;
            for (int i = 1; i < lstTo.Count; i++)
            {
                message.To.Add(lstTo[i]);
            }

            SmtpClient client = new SmtpClient(mailInfo.Host, int.Parse(mailInfo.Port));
            client.Credentials = new NetworkCredential(mailInfo.Username, mailInfo.Password);
            bool ssl = true;
            client.EnableSsl = bool.TryParse(mailInfo.EnableSsl, out ssl) ? ssl : true;
            client.Send(message);
        }
    }
}
