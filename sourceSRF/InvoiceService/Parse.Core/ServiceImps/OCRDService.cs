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
    public class OCRDService : BaseService<OCRD, string>, IOCRDService
    {
        public OCRDService(string sessionFactoryConfigPath, string connectionString = null) : base(sessionFactoryConfigPath, connectionString)
        {
        }
    }
}
