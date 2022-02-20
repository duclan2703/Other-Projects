using FX.Data;
using Parse.Core.Domain;
using Parse.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parse.Core.ServiceImps
{
    public class EInvoice_LogService : BaseService<EInvoice_Log, decimal>, IEInvoice_LogService
    {
        public EInvoice_LogService(string sessionFactoryConfigPath, string connectionString = null) : base(sessionFactoryConfigPath, connectionString)
        {
        }

        public EInvoice_Log GetByFkey(int fkey)
        {
            return Query.FirstOrDefault(c => c.DocEntry == fkey);
        }
    }
}
