using FX.Data;
using Parse.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parse.Core.Services
{
    public interface IEInvoice_LogService : IBaseService<EInvoice_Log, decimal>
    {
        EInvoice_Log GetByFkey(int fkey);
    }
}
