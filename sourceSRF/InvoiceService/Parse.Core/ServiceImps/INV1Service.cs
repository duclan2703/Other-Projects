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
    public class INV1Service : BaseService<INV1, int>, IINV1Service
    {
        public INV1Service(string sessionFactoryConfigPath, string connectionString = null) : base(sessionFactoryConfigPath, connectionString)
        {
        }
    }
}
