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
    public interface IPXKService : IBaseService<PXK, int>
    {
        List<PXK> GetListPXKByDate(DateTime processtime, string comtaxcode);
        void UpdatePXK(PXK pxk, APIResult result, InvoiceVAT inv, ref int invSuccess, ref int invError, ref string errorList);
        void UpdateCancelPXK(PXK pxk, APIResult result, ref int invSuccess, ref int invError, ref string errorList);
        PXK GetByFkey(string fkey);
    }
}
