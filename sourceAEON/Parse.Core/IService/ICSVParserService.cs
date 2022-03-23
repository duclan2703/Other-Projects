using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parse.Core.IService
{
    public interface ICSVParserService : IParserService
    {
        void ConvertCSVToData(string filePath, out DataTable dTable, ref string mesError);
    }
}
