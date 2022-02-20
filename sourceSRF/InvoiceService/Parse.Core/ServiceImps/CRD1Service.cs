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
    public class CRD1Service : BaseService<CRD1, string>, ICRD1Service
    {
        public CRD1Service(string sessionFactoryConfigPath, string connectionString = null) : base(sessionFactoryConfigPath, connectionString)
        {
        }
    }
}
