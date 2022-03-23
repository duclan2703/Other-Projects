using FX.Data;
using Parse.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parse.Core.IService
{
    public interface IBussinessLogService : IBaseService<BussinessLog,int>
    {
        List<BussinessLog> GetByPaging(int pageIndex, int pageSize);
    }
}
