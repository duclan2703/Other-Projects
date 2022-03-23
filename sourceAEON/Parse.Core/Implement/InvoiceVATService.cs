using FX.Core;
using FX.Data;
using log4net;
using Parse.Core.Domain;
using Parse.Core.IService;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parse.Core.Implement
{
    public class InvoiceVATService : BaseService<InvoiceVAT, int>, IInvoiceVATService
    {
        static ILog log = LogManager.GetLogger(typeof(InvoiceVATService));
        public InvoiceVATService(string sessionFactoryConfigPath)
           : base(sessionFactoryConfigPath)
        { }

        public bool DeleteInvoice(InvoiceVAT Inv, out string message)
        {
            message = "";
            IProductInvService prodSrc = IoC.Resolve<IProductInvService>();
            try
            {
                BeginTran();
                Delete(Inv.Id);
                if (Inv.Products.Count > 0)
                {
                    foreach (var prod in Inv.Products)
                    {
                        prodSrc.Delete(prod.Id);
                    }
                }
                CommitTran();
                return true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                RolbackTran();
                return false;
            }
        }

        public bool DeleteInvoices(List<InvoiceVAT> Invs, out string message)
        {
            message = "";
            IProductInvService prodSrc = IoC.Resolve<IProductInvService>();
            try
            {
                BeginTran();
                foreach (var inv in Invs)
                {
                    Delete(inv.Id);
                    if (inv.Products.Count > 0)
                    {
                        foreach (var prod in inv.Products)
                        {
                            prodSrc.Delete(prod.Id);
                        }
                    }
                }
                CommitTran();
                return true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                RolbackTran();
                return false;
            }
        }
        public List<InvoiceVAT> GetPublishSuccess(int pageIndex, int pageSize, out int total)
        {
            var query = Query;
            query = Query.Where(c => c.Publish == PublishStatus.Success);
            total = query.Count();
            query = query.OrderByDescending(x => x.Id);
            var list = query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            UnbindSession(list);
            return list;
        }

        public List<InvoiceVAT> GetUnPublish(int pageIndex, int pageSize, out int total)
        {
            var query = Query;
            query = Query.Where(c => c.Publish != PublishStatus.Success);
            total = query.Count();
            query = query.OrderByDescending(x => x.Id);
            var list = query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            UnbindSession(list);
            return list;
        }

        public List<InvoiceVAT> GetUnPublish()
        {
            var query = Query;
            query = query.Where(c => c.Publish != PublishStatus.Success);
            var list = query.ToList();
            UnbindSession(list);
            return list;
        }

        public bool UpdateInvoice(List<InvoiceVAT> lstInv, out string message)
        {
            message = "";
            IProductInvService prodSrc = IoC.Resolve<IProductInvService>();
            if (lstInv == null || lstInv.Count() == 0) return false;
            if (lstInv.Count > 100)
                isStateLess = true;
            try
            {
                var stopwatch = Stopwatch.StartNew();
                BeginTran();
                foreach (var inv in lstInv)
                {
                    CreateNew(inv);
                    foreach (var prod in inv.Products)
                    {
                        prod.InvoiceID = inv.Id;
                        prod.Quantity = prod.Quantity > 0 ? prod.Quantity : 1;
                        prod.Price = prod.Price;
                        prodSrc.CreateNew(prod);
                    }
                }
                CommitTran();
                log.Error(String.Format("Thoi gian xu ly:{0} Milliseconds", stopwatch.ElapsedMilliseconds));
                return true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                RolbackTran();
                return false;
            }
            finally { isStateLess = false; }
        }

        public bool UpdateInvoice(InvoiceVAT inv, out string message)
        {
            message = "";
            IProductInvService prodSrc = IoC.Resolve<IProductInvService>();
            try
            {
                var stopwatch = Stopwatch.StartNew();
                BeginTran();
                CreateNew(inv);
                foreach (var prod in inv.Products)
                {
                    prod.InvoiceID = inv.Id;
                    prod.Quantity = prod.Quantity > 0 ? prod.Quantity : 1;
                    prod.Price = prod.Price > 0 ? prod.Price : prod.Amount;
                    prodSrc.CreateNew(prod);
                }
                CommitTran();
                log.Error(String.Format("Thoi gian xu ly:{0} Milliseconds", stopwatch.ElapsedMilliseconds));
                return true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                RolbackTran();
                return false;
            }
            finally { isStateLess = false; }
        }

        public bool UpdateNewInvoice(InvoiceVAT OInvoice, out string message)
        {
            message = "";
            BeginTran();
            try
            {
                OInvoice.Fkey = GetFkey(OInvoice);
                if (OInvoice.Products == null || OInvoice.Products.Count == 0)
                {
                    message = "Hóa đơn không có sản phẩm xin vui lòng nhập lại!";
                    return false;
                }
                //if (CheckOldInvFolioNo(OInvoice.FolioNo, OInvoice.Id))
                //{
                //    message = "Số hóa đơn đã tồn tại!";
                //    return false;
                //}
                IProductInvService prodSrv = IoC.Resolve<IProductInvService>();
                IList<ProductInv> lDelete = prodSrv.Query.Where(p => p.InvoiceID == OInvoice.Id).ToList();
                foreach (var p in lDelete)
                {
                    prodSrv.Delete(p);
                }
                //save sản phẩm trong hóa đơn                
                foreach (var p in OInvoice.Products)
                {
                    p.InvoiceID = OInvoice.Id;
                    prodSrv.CreateNew(p);
                }
                Update(OInvoice);
                CommitTran();
                return true;
            }
            catch (Exception ex)
            {
                message = "Chưa tạo được hóa đơn!";
                RolbackTran();
                return false;
            }
        }

        public bool CreateNewInvoice(InvoiceVAT NewInvoice, out string message)
        {
            message = "";
            try
            {
                NewInvoice.Fkey = GetFkey(NewInvoice);
                if (NewInvoice.VATRate == -1)
                    NewInvoice.FolioNo = NewInvoice.FolioOrigin + "_KoThue";
                else
                    NewInvoice.FolioNo = NewInvoice.FolioOrigin + "_" + NewInvoice.VATRate.ToString();
                if (NewInvoice.Products == null || NewInvoice.Products.Count == 0)
                {
                    message = "Hóa đơn không có sản phẩm xin vui lòng nhập lại!";
                    return false;
                }
                //if (CheckFolioNo(NewInvoice.FolioNo))
                //{
                //    message = "Số hóa đơn đã tồn tại!";
                //    return false;
                //}
                BeginTran();
                IProductInvService prodSrv = IoC.Resolve<IProductInvService>();
                InvoiceVAT invNew = CreateNew(NewInvoice);
                //save sản phẩm trong hóa đơn                
                foreach (var p in NewInvoice.Products)
                {
                    p.InvoiceID = invNew.Id;
                    prodSrv.CreateNew(p);
                }
                CommitTran();
                return true;
            }
            catch (Exception ex)
            {
                message = "Chưa tạo được hóa đơn!";
                RolbackTran();
                return false;
            }
        }

        public string GetFkey(InvoiceVAT invoice)
        {
            string codeTax = AppContext.Current.company.TaxCode;

            return codeTax + "-" + invoice.FolioNo + "-" + string.Format("{0:yyyyMMdd}", invoice.ArisingDate);
        }
        //bool CheckFolioNo(string folioNo)
        //{
        //    IInvoiceVATService service = IoC.Resolve<IInvoiceVATService>();
        //    var lstInv = service.Query.Where(p => p.FolioNo == folioNo);
        //    return lstInv.Count() > 0 ? true : false;
        //}
        //bool CheckOldInvFolioNo(string folioNo, int id)
        //{
        //    IInvoiceVATService service = IoC.Resolve<IInvoiceVATService>();
        //    var lstInv = service.Query.Where(p => p.FolioNo == folioNo && p.Id != id);
        //    return lstInv.Count() > 0 ? true : false;
        //}
    }
}
