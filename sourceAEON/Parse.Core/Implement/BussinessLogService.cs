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
    public class BussinessLogService : BaseService<BussinessLog, int>, IBussinessLogService
    {
        public BussinessLogService(string sessionFactoryConfigPath)
           : base(sessionFactoryConfigPath)
        { }

        public List<BussinessLog> GetByPaging(int pageIndex, int pageSize)
        {
            if (pageIndex == 0)
                return Query.OrderByDescending(x => x.CreateDate).ToList();
            else
            {
                return Query.OrderByDescending(x => x.CreateDate).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            }
        }
    }
}
