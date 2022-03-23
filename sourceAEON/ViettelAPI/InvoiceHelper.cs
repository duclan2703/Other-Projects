using FX.Core;
using log4net;
using Parse.Core.Domain;
using Parse.Core.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViettelAPI.Models;

namespace ViettelAPI
{
    public class InvoiceHelper
    {

        static ILog log = LogManager.GetLogger(typeof(InvoiceHelper));
        private static IInvoiceVATService InvSrc = IoC.Resolve<IInvoiceVATService>();
        static IBussinessLogService logService = IoC.Resolve<IBussinessLogService>();

        public static void UpdatePublishResult(List<InvoiceVAT> ListInv, APIResults results)
        {
            foreach (APIResult rs in results.createInvoiceOutputs)
            {
                InvoiceVAT inv = ListInv.Where(x => x.Fkey == rs.transactionUuid).SingleOrDefault();
                if (!string.IsNullOrEmpty(rs.errorCode))
                {
                    inv.Publish = Parse.Core.PublishStatus.Error;
                    inv.MessageError = string.Format("{0}: {1}", rs.errorCode, rs.description);
                }
                else
                {
                    inv.Publish = Parse.Core.PublishStatus.Success;
                    inv.MessageError = "";
                    inv.No = rs.result.invoiceNo;
                    inv.Serial = rs.result.invoiceNo.Substring(0, 6);
                }

                InvSrc.Update(inv);
                InvSrc.CommitChanges();
            }
        }

    }
}
