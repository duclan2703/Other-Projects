using FX.Data;
using Parse.Core.Domain;
using Parse.Core.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parse.Core.Implement
{
    public class SapUploadService : BaseService<SapUpload, string>, ISapUploadService
    {
        public SapUploadService(string sessionFactoryConfigPath) : base(sessionFactoryConfigPath)
        {
        }
    }
}
