using FX.Data;
using Parse.Core.Domain;
using Parse.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parse.Core.Services
{
    public interface IHDDTService : IBaseService<HDDT, int>
    {
        List<HDDT> GetListHDDTByDate(DateTime processtime, string comtaxcode);
        void UpdateHDDT(HDDT hd, APIResult result, InvoiceVAT inv, ref int invSuccess, ref int invError, ref string errorList);
        void UpdateCancelHDDT(HDDT hd, APIResult result, ref int invSuccess, ref int invError, ref string errorList);
        HDDT GetByFkey(string fkey);
    }
}
