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
    public class HDDTService : BaseService<HDDT, int>, IHDDTService
    {
        public HDDTService(string sessionFactoryConfigPath, string connectionString = null) : base(sessionFactoryConfigPath, connectionString)
        {
        }

        static ILog log = LogManager.GetLogger(typeof(HDDTService));
        public List<HDDT> GetListHDDTByDate(DateTime processtime, string comtaxcode)
        {
            var query = Query.Where(c => c.TaxCode == comtaxcode && c.CreatedDate.Date >= processtime  && c.Status != 1);
            return query.ToList();
        }

        public void UpdateHDDT(HDDT hd, APIResult result, InvoiceVAT inv, ref int invSuccess, ref int invError, ref string errorList)
        {
            try
            {
                BeginTran();
                hd.Pattern = inv.Pattern;
                hd.Serial = inv.Serial;
                if (!string.IsNullOrEmpty(result.errorCode))
                {
                    hd.ErrorCode = result.errorCode;
                    hd.ErrorDesc = result.description;
                    hd.Status = 2;
                    invError++;
                    errorList += " - " + hd.Fkey;
                }
                else
                {
                    hd.InvNo = result.result.invoiceNo;
                    hd.Status = 1;
                    invSuccess++;
                }
                Update(hd);
                CommitTran();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                RolbackTran();
            }
        }

        public void UpdateCancelHDDT(HDDT hd, APIResult result, ref int invSuccess, ref int invError, ref string errorList)
        {
            try
            {
                BeginTran();
                if (!string.IsNullOrEmpty(result.errorCode))
                {
                    hd.ErrorCode = result.errorCode;
                    hd.ErrorDesc = result.description;
                    hd.Status = 2;
                    invError++;
                    errorList += " - " + hd.Fkey;
                }
                else
                {
                    hd.Status = 1;
                    invSuccess++;
                }
                Update(hd);
                CommitTran();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                RolbackTran();
            }
        }

        public HDDT GetByFkey(string fkey)
        {
            return Query.FirstOrDefault(c => c.Fkey == fkey && c.Status == 1);
        }
    }
}
