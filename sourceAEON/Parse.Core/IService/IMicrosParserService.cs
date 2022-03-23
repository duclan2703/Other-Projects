using Parse.Core.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parse.Core.IService
{
    public interface IMicrosParserService : IParserService
    {
        void SplitMicrosFile(string filePath, string storeDir);

        /// <summary>
        /// parser file micros ra dữ liệu text
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        string ParseToText(string path);
    }
}
