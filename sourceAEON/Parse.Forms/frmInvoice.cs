using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Parse.Core.Domain;
using EInvoice.Utils;
using System.Globalization;
using log4net;
using Parse.Core.IService;
using FX.Core;
using Parse.Forms.CustomUC;
using System.Configuration;
using ViettelAPI.Models;
using Parse.Core;
using ViettelAPI;
using Newtonsoft.Json;
using static Parse.Core.Common;

namespace Parse.Forms
{
    public partial class frmInvoice : DevExpress.XtraEditors.XtraForm
    {
        public InvoiceVAT Invoice = new InvoiceVAT();
        BindingSource Bindingsource = new BindingSource();
        private readonly ILog log = LogManager.GetLogger(typeof(frmInvoice));
        IMainForm Main;
        public frmInvoice(IMainForm main, InvoiceVAT inv, bool type)
        {
            InitializeComponent();
            Main = main;
            this.Invoice = inv;
            if (!type)
                this.Text = "Tạo lập hóa đơn";
            else
            {
                this.Text = "Thông tin chi tiết hóa đơn " + this.Invoice.No;
            }
        }

        private void frmInvoice_Load(object sender, EventArgs e)
        {
            LoadProTypeCombobox();
            LoadTaxCombobox();
            LoadData();
        }

        // loại sản phẩm
        public void LoadProTypeCombobox()
        {
            cboType.DataSource = Parse.Core.Common.lstProductType;
            cboType.DisplayMember = "Value";
            cboType.ValueMember = "Key";
            cboType.PopulateColumns();
            cboType.Columns[0].Visible = false;
        }

        public void LoadTaxCombobox()
        {
            cbVATRate.Properties.DataSource = Parse.Core.Common.lstTax;
            cbVATRate.Properties.DisplayMember = "Value";
            cbVATRate.Properties.ValueMember = "Key";
            cbVATRate.Properties.PopulateColumns();
            cbVATRate.Properties.DropDownRows = Parse.Core.Common.lstTax.Count() < 10 ? Parse.Core.Common.lstTax.Count : 10;
            cbVATRate.Properties.Columns[0].Visible = false;
        }

        // Binding invoice in form
        public void LoadData()
        {
            if (this.Invoice != null)
            {
                //Customer Information
                txtBuyer.Text = this.Invoice.Buyer;
                txtCusCode.Text = this.Invoice.CusCode;
                txtCusName.Text = this.Invoice.CusName;
                txtCusPhone.Text = this.Invoice.CusPhone;
                txtCusComName.Text = this.Invoice.CusComName;
                txtCusTaxCode.Text = this.Invoice.CusTaxCode;
                txtCusAddress.Text = this.Invoice.CusAddress;
                txtCusEmail.Text = this.Invoice.CusEmail;
                txtFolioOrigin.Text = this.Invoice.FolioOrigin;
                txtCusBankName.Text = this.Invoice.CusBankName;
                txtCusBankNo.Text = this.Invoice.CusBankNo;
                txtStaffId.Text = this.Invoice.StaffId;
                txtDeliveryId.Text = this.Invoice.DeliveryId;
                // txtRoomNo.Text = this.Invoice.RoomNo;

                //DateTime arrivalDate;
                //var success = DateTime.TryParseExact(s: this.Invoice.ArrivalDate, formats: new string[] { "dd.MM.yy", "dd/MM/yy", "dd-MM-yy", "dd.MM.yyyy", "dd/MM/yyyy", "dd-MM-yyyy" }, provider: CultureInfo.InvariantCulture, style: DateTimeStyles.None, result: out arrivalDate);
                //if (success)
                //    dtArrivalDate.EditValue = arrivalDate;

                //DateTime departureDate;
                //var _success = DateTime.TryParseExact(s: this.Invoice.DepartureDate, formats: new string[] { "dd.MM.yy", "dd/MM/yy", "dd-MM-yy", "dd.MM.yyyy", "dd/MM/yyyy", "dd-MM-yyyy" }, provider: CultureInfo.InvariantCulture, style: DateTimeStyles.None, result: out departureDate);
                //if (_success)
                //    dtDepartureDate.EditValue = departureDate;

                //txtBookingNo.Text = this.Invoice.BookingNo;

                txtTotal.Text = this.Invoice.Total.ToString();
                //txtServiceCharge.Text = this.Invoice.ServiceCharge.ToString();
                //txtServiceSpecial.Text = this.Invoice.ServiceSpecial.ToString();
                txtVATAmount.Text = this.Invoice.VATAmount.ToString();
                txtDiscountAmount.Text = this.Invoice.DiscountAmount.ToString();
                txtAmountInvoice.Text = this.Invoice.Amount.ToString();
                txtAmountInWord.Text = this.Invoice.AmountInWords;

                //set Enabled
                btnReleased.Enabled = this.Invoice.Id > 0 ? true : false;
                //txtServiceSpecial.ReadOnly = this.Invoice.Id > 0 ? true : false;
                //if (this.Invoice.Id > 0)
                //{
                // che do edit
                //chkTTDB.Enabled = false;
                //}
                //else
                //{
                // che do tao moi
                // chkTTDB.Enabled = true;
                // chkTTDB.Checked = this.Invoice.ServiceSpecial > 0 ? true : false;
                //}
                //chkTTDB.Enabled = this.Invoice.Id > 0 ? false : true;
                //chkTTDB.Checked = this.Invoice.ServiceSpecial > 0 ? true : false;

                cbVATRate.EditValue = this.Invoice.VATRate;
                //Product Information
                Bindingsource.DataSource = this.Invoice.Products;
                gridProduct.DataSource = Bindingsource;
            }
        }

        // Get invoice khi Lưu
        public InvoiceVAT InnitData()
        {
            this.Invoice.Buyer = txtBuyer.Text;
            this.Invoice.CusCode = txtCusCode.Text;
            this.Invoice.CusName = txtCusName.Text;
            this.Invoice.CusPhone = txtCusPhone.Text;
            this.Invoice.CusComName = txtCusComName.Text;
            this.Invoice.CusTaxCode = txtCusTaxCode.Text;
            this.Invoice.CusAddress = txtCusAddress.Text;
            this.Invoice.CusEmail = txtCusEmail.Text;
            this.Invoice.FolioOrigin = txtFolioOrigin.Text;
            this.Invoice.CusBankName = txtCusBankName.Text;
            this.Invoice.CusBankNo = txtCusBankNo.Text;
            this.Invoice.StaffId = txtStaffId.Text;
            this.Invoice.DeliveryId = txtDeliveryId.Text;
            this.Invoice.VATRate = float.Parse(cbVATRate.EditValue.ToString());
            //this.Invoice.RoomNo = txtRoomNo.Text;
            //this.Invoice.ArrivalDate = dtArrivalDate.Text;
            // this.Invoice.DepartureDate = dtDepartureDate.Text;
            //this.Invoice.BookingNo = txtBookingNo.Text;

            this.Invoice.Total = txtTotal.Text != "" ? Convert.ToDecimal(txtTotal.Text) : 0;
            //this.Invoice.ServiceCharge = txtServiceCharge.Text != "" ? Convert.ToDecimal(txtServiceCharge.Text) : 0;
            //this.Invoice.ServiceSpecial = txtServiceSpecial.Text != "" ? Convert.ToDecimal(txtServiceSpecial.Text) : 0;
            this.Invoice.VATAmount = txtVATAmount.Text != "" ? Convert.ToDecimal(txtVATAmount.Text) : 0;
            this.Invoice.DiscountAmount = txtVATAmount.Text != "" ? Convert.ToDecimal(txtDiscountAmount.Text) : 0;
            this.Invoice.Amount = txtAmountInvoice.Text != "" ? Convert.ToDecimal(txtAmountInvoice.Text) : 0;
            this.Invoice.AmountInWords = txtAmountInWord.Text;
            //this.Invoice.PaymentMethod = cbPayMethod.EditValue != null ? cbPayMethod.EditValue.ToString() : "Khác";
            if (this.Invoice.Id == 0)
                this.Invoice.ArisingDate = DateTime.Now.Date;
            return this.Invoice;
        }

        private bool ValidateData()
        {
            if (txtFolioOrigin.Text == "" )
                return false;
            return true;
        }

        // Lưu thông tin invoice
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if(!ValidateData())
                {
                    XtraMessageBox.Show("Số hóa đơn không được để trống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                IInvoiceVATService service = IoC.Resolve<IInvoiceVATService>();
                var invoice = InnitData();
                var message = "";
                bool result;
                if (invoice.Id > 0)
                    result = service.UpdateNewInvoice(invoice, out message);
                else
                    result = service.CreateNewInvoice(invoice, out message);

                if (result)
                {
                    service.UnbindSession();
                    ucInvoiceVAT uc = new ucInvoiceVAT(this.Main);
                    this.Main.AddUC(uc);
                    XtraMessageBox.Show("Cập nhật thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnReleased.Enabled = true;
                }
                else
                    throw new Exception(message);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Cập nhật thất bại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                log.Error(ex);
            }
        }

        // Tính số tiền bằng chữ
        private void txtAmountInvoice_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtAmountInvoice.Text))
            {
                var amount = Convert.ToDecimal(txtAmountInvoice.EditValue);
                txtAmountInWord.Text = Parse.Core.NumberToLeter.DocTienBangChu(amount);
            }
        }

        // Close form hóa đơn
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Sửa các textbox
        private void TextEdit_EditValueChanged(object sender, EventArgs e)
        {
            //if (lbCheckCal.Text != "OK")
            //{
            //    var total = !string.IsNullOrWhiteSpace(txtTotal.Text) ? Convert.ToDecimal(txtTotal.Text) : 0;
            //    var servicecharge = !string.IsNullOrWhiteSpace(txtServiceCharge.Text) ? Convert.ToDecimal(txtServiceCharge.Text) : 0;
            //    var vatAmount = !string.IsNullOrWhiteSpace(txtVATAmount.Text) ? Convert.ToDecimal(txtVATAmount.Text) : 0;
            //    var vatRate = !string.IsNullOrWhiteSpace(txtVATRate.Text) ? Convert.ToDecimal(txtVATRate.Text) : 0;
            //    var amountInv = !string.IsNullOrWhiteSpace(txtAmountInvoice.Text) ? Convert.ToDecimal(txtAmountInvoice.Text) : 0;

            //    var TextName = (TextEdit)sender;
            //    if (TextName.Name == "txtTotal")
            //    {
            //        txtAmountInvoice.Text = (total + servicecharge + vatAmount).ToString();
            //    }
            //    else if (TextName.Name == "txtServiceCharge")
            //    {
            //        txtAmountInvoice.Text = (total + servicecharge + vatAmount).ToString();
            //    }
            //    else if (TextName.Name == "txtVATRate")
            //    {
            //        txtVATAmount.Text = (vatRate * total / 100).ToString();
            //    }
            //    else if (TextName.Name == "txtVATAmount")
            //    {
            //        txtAmountInvoice.Text = (total + servicecharge + vatAmount).ToString();
            //    }
            //    else if (TextName.Name == "txtAmountInvoice")
            //    {
            //        if (!string.IsNullOrWhiteSpace(txtAmountInvoice.Text))
            //        {
            //            var amount = Convert.ToInt64(txtAmountInvoice.EditValue);
            //            txtAmountInWord.Text = NumberToLeter.DocTienBangChu(amount);
            //        }
            //    }
            //}
            //else
            //    lbCheckCal.Text = "ERROR";
        }

        // Tính ngược tiền
        private void btnRecal_Click(object sender, EventArgs e)
        {
            Calculator();

            //var amount = !string.IsNullOrEmpty(txtAmountInvoice.Text) ? Convert.ToDecimal(txtAmountInvoice.Text) : 0;
            //var total = Convert.ToDouble(amount);
            //double tax10 = total * invoice.VATRate / 100;
            //txtTotal.Text = Math.Round(total, MidpointRounding.AwayFromZero).ToString();

            //txtVATAmount.Text = Math.Round(tax10, MidpointRounding.AwayFromZero).ToString();
        }

        // Tính tiền sản phẩm trên gridview
        private void gridProduct_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.Column.FieldName == "Quantity")
            {
                var quantity = e.Value != null ? decimal.Parse(e.Value.ToString()) : 0;
                var price = viewProduct.GetFocusedRowCellValue("Price") != null ? decimal.Parse(viewProduct.GetFocusedRowCellValue("Price").ToString()) : 0;
                var total = quantity == 0 && price < 0 ? price : quantity * price;
                viewProduct.SetFocusedRowCellValue("Total", total.ToString());
                Calculator();
            }

            if (e.Column.FieldName == "Price")
            {
                var price = e.Value != null ? decimal.Parse(e.Value.ToString()) : 0;
                var quantity = viewProduct.GetFocusedRowCellValue("Quantity") != null ? decimal.Parse(viewProduct.GetFocusedRowCellValue("Quantity").ToString()) : 0;
                var total = quantity == 0 && price < 0 ? price : quantity * price;
                viewProduct.SetFocusedRowCellValue("Total", total.ToString());
                Calculator(); 
            }

            if (e.Column.FieldName == "DiscountAmount")
            {
                Calculator();
            }
        }

        // Tính tiền
        private void Calculator()
        {
            var invoice = this.Invoice;
            if (invoice != null)
            {
                double total = double.Parse(invoice.Products.Where(c => c.Type == (int)ProductType.Product).Sum(p => p.Total).ToString());
                double discount = double.Parse(invoice.Products.Sum(p => p.DiscountAmount).ToString());
                if (invoice.VATRate == -1)
                    invoice.VATRate = 0;
                double tax = Math.Round((total - discount) * invoice.VATRate / 100);
                double amount = Math.Round(total - discount + tax);
                txtTotal.Text = (total - discount).ToString();
                txtVATAmount.Text = tax.ToString();
                txtDiscountAmount.Text = discount.ToString();
                txtAmountInvoice.Text = amount.ToString();
                txtAmountInWord.Text = Parse.Core.NumberToLeter.DocTienBangChu((long)amount);
            }
        }

        // Xóa sản phẩm trên gridview
        private void btnDeleteRow_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                viewProduct.DeleteRow(viewProduct.FocusedRowHandle);
                Calculator();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        // Lưu và phát hành hóa đơn
        private void btnReleased_Click(object sender, EventArgs e)
        {
            var confirmResult = XtraMessageBox.Show("Bạn muốn phát hành hóa đơn?", "Thông báo", MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
                IBussinessLogService logService = IoC.Resolve<IBussinessLogService>();
                try
                {
                    // Lưu hóa đơn
                    IInvoiceVATService service = IoC.Resolve<IInvoiceVATService>();
                    var invoice = InnitData();
                    var message = "";
                    bool result;
                    if (invoice.Id > 0)
                    {
                        result = service.UpdateNewInvoice(invoice, out message);
                    }
                    else
                    {
                        XtraMessageBox.Show("Phát hành hóa đơn thất bại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (result)
                    {
                        service.UnbindSession();
                        List<InvoiceVAT> lstInvoice = new List<InvoiceVAT>();
                        lstInvoice.Add(invoice);
                        // Phát hành hóa đơn
                        // VIETTEL API
                        IApiParserService apiParserSer = IoC.Resolve<IApiParserService>();
                        var lstInvoiceApi = apiParserSer.ConvertToAPIModel(lstInvoice, AppContext.Current.company);
                        APIResults results = ViettelAPI.APIHelper.SendInvoices(lstInvoiceApi);
                        //Lưu vào db InvoiceConvert
                        InvoiceHelper.UpdatePublishResult(lstInvoice, results);

                        // Ghi log hóa đơn
                        BussinessLog Bussinesslog = new BussinessLog();
                        Bussinesslog.FileName = "Phát hành hóa đơn";
                        Bussinesslog.CreateDate = DateTime.Now;
                        if (string.IsNullOrEmpty(results.createInvoiceOutputs[0].errorCode))
                            Bussinesslog.Error = string.Format("Phát hành hóa đơn thành công - {0}", invoice.FolioNo);
                        else
                            Bussinesslog.Error = string.Format("Phát hành hóa đơn lỗi - {0}", invoice.FolioNo);
                        logService.CreateNew(Bussinesslog);
                        logService.CommitChanges();

                        XtraMessageBox.Show("Đã thực hiện phát hành hóa đơn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    else
                        throw new Exception(message);
                }
                catch (Exception ex)
                {
                    // Ghi log hóa đơn
                    BussinessLog Bussinesslog = new BussinessLog();
                    Bussinesslog.FileName = "Phát hành hóa đơn";
                    Bussinesslog.CreateDate = DateTime.Now;
                    Bussinesslog.Error = "Hóa đơn: " + Invoice.FolioNo + " + Lỗi: " + ex.Message;
                    logService.CreateNew(Bussinesslog);
                    logService.CommitChanges();
                    log.Error(ex);
                    XtraMessageBox.Show("Phát hành hóa đơn thất bại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    this.Close();
                    ucInvoiceVAT uc = new ucInvoiceVAT(this.Main);
                    this.Main.AddUC(uc);
                }
            }
            else
            {
                this.Close();
                ucInvoiceVAT uc = new ucInvoiceVAT(this.Main);
                this.Main.AddUC(uc);
            }
        }


        private void cbVATRate_EditValueChanged(object sender, EventArgs e)
        {
            this.Invoice.VATRate = float.Parse(cbVATRate.EditValue.ToString());
            Calculator();
        }
    }
}