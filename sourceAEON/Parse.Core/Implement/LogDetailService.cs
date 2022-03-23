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
    public class LogDetailService : BaseService<LogDetail, int>, ILogDetailService
    {
        public LogDetailService(string sessionFactoryConfigPath) : base(sessionFactoryConfigPath)
        {
        }

        public List<LogDetail> GetByLogFile(int id)
        {
            return Query.Where(c => c.LogId == id).ToList();
        }
    }
}
