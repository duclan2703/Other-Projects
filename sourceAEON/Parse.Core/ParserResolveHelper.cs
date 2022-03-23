using FX.Core;
using Parse.Core.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parse.Core
{
    public class ParserResolveHelper
    {
        public static IParserService Resolve(string serviceName)
        {
            IParserService service = IoC.Resolve<IParserService>(serviceName);
            return service;
        }
    }
}
