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
    public class ConfigService : BaseService<Config, int>, IConfigService
    {
        public ConfigService(string sessionFactoryConfigPath)
           : base(sessionFactoryConfigPath)
        { }

    }
}
