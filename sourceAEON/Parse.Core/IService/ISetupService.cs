using FX.Data;
using Parse.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parse.Core.IService
{
    public interface ISetupService : IBaseService<Setup, int>
    {
        Setup GetbyCode(string code);
    }
}
