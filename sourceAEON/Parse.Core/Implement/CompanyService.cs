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
    public class CompanyService : BaseService<Company, int>, ICompanyService
    {
        public CompanyService(string sessionFactoryConfigPath)
           : base(sessionFactoryConfigPath)
        { }
        public Company GetCompanyByCode(string code)
        {
            return Query.FirstOrDefault(p => p.Code.ToLower() == code);
        }
    }
}
