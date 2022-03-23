using FX.Data;
using Parse.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parse.Core.IService
{
    public interface IInvoiceVATService : IBaseService<InvoiceVAT, int>
    {
        bool isStateLess { get; set; }
        bool UpdateInvoice(List<InvoiceVAT> lstInv, out string message);
        bool UpdateInvoice(InvoiceVAT inv, out string message);
        List<InvoiceVAT> GetUnPublish(int pageIndex, int pageSize, out int total);
        List<InvoiceVAT> GetUnPublish();
        List<InvoiceVAT> GetPublishSuccess(int pageIndex, int pageSize, out int total);
        bool DeleteInvoice(InvoiceVAT Inv, out string message);
        bool DeleteInvoices(List<InvoiceVAT> Invs, out string message);
        bool UpdateNewInvoice(InvoiceVAT OInvoice, out string message);
        bool CreateNewInvoice(InvoiceVAT Invoice, out string message);
    }
}
