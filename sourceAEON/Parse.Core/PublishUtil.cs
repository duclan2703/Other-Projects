using FX.Core;
using log4net;
using Parse.Core.Domain;
using Parse.Core.IService;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parse.Core
{
    public static class PublishUtil
    {
        static ILog log = LogManager.GetLogger(typeof(PublishUtil));
        //private static IInvoiceConvertService InvConvertSrc = IoC.Resolve<IInvoiceConvertService>();
        private static IInvoiceVATService InvSrc = IoC.Resolve<IInvoiceVATService>();
        public static void UpdatePublishResult(List<InvoiceVAT> ListInv, PublishResultConverted result)
        {
            InvSrc.BeginTran();
            try
            {
                foreach (Data item in result.data)
                {
                    InvoiceVAT inv = ListInv.Where(x => x.Fkey == item.Key).SingleOrDefault();
                    inv.Serial = item.InvSerial;
                    inv.No = item.InvNo;
                    inv.Publish = PublishStatus.Success;
                    inv.MessageError = "";
                    InvSrc.Update(inv);
                }
                InvSrc.CommitTran();
            }
            catch (Exception ex)
            {
                InvSrc.RolbackTran();
            }
        }
    }
}
