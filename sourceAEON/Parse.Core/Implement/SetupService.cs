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
    public class SetupService : BaseService<Setup, int>, ISetupService
    {
        public SetupService(string sessionFactoryConfigPath)
           : base(sessionFactoryConfigPath)
        { }

        public Setup GetbyCode(string code)
        {
            return Query.Where(x => x.Code == code).SingleOrDefault();
        }
    }
}
