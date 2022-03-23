using Parse.Core.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parse.Core.IService
{
    public interface IParserService
    {
        void FileProcessing(string filePath, string pattern, string serial, ref int invSuccess, ref int invTotal, ref string mesError);
    }
}
