using FX.Data;
using log4net;
using Parse.Core.Domain;
using Parse.Core.Models;
using Parse.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parse.Core.ServiceImps
{
    public class PXKService : BaseService<PXK, int>, IPXKService
    {
        public PXKService(string sessionFactoryConfigPath, string connectionString = null) : base(sessionFactoryConfigPath, connectionString)
        {
        }

        static ILog log = LogManager.GetLogger(typeof(PXKService));
        public List<PXK> GetListPXKByDate(DateTime processtime, string comtaxcode)
        {
            var query = Query.Where(c => c.TaxCode == comtaxcode && c.CreatedDate.Date >= processtime && c.Status != 1);
            return query.ToList();
        }

        public void UpdatePXK(PXK pxk, APIResult result, InvoiceVAT inv, ref int invSuccess, ref int invError, ref string errorList)
        {
            try
            {
                BeginTran();
                pxk.Pattern = inv.Pattern;
                pxk.Serial = inv.Serial;
                if (!string.IsNullOrEmpty(result.errorCode))
                {
                    pxk.ErrorCode = result.errorCode;
                    pxk.ErrorDesc = result.description;
                    pxk.Status = 2;
                    invError++;
                    errorList += " - " + pxk.Fkey;
                }
                else
                {
                    pxk.InvNo = result.result.invoiceNo;
                    pxk.Status = 1;
                    invSuccess++;
                }
                Update(pxk);
                CommitTran();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                RolbackTran();
            }
        }

        public void UpdateCancelPXK(PXK pxk, APIResult result, ref int invSuccess, ref int invError, ref string errorList)
        {
            try
            {
                BeginTran();
                if (!string.IsNullOrEmpty(result.errorCode))
                {
                    pxk.ErrorCode = result.errorCode;
                    pxk.ErrorDesc = result.description;
                    pxk.Status = 2;
                    invError++;
                    errorList += " - " + pxk.Fkey;
                }
                else
                {
                    pxk.Status = 1;
                    invSuccess++;
                }
                Update(pxk);
                CommitTran();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                RolbackTran();
            }
        }

        public PXK GetByFkey(string fkey)
        {
            return Query.FirstOrDefault(c => c.Fkey == fkey && c.Status == 1);
        }
    }
}
