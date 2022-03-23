using Parse.Core.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parse.Core.IService
{
    public interface IAEONService : IParserService
    {
        void PrintFileMapping(int id, string filePath);
    }
}
