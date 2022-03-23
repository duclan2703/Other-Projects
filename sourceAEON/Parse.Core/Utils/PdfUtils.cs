using Bytescout.PDFExtractor;
using FX.Core;
using Parse.Core.Implement;
using Parse.Core.IService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Parse.Core
{
    public class PdfUtils
    {
        public static string PdfToXml(string filepath)
        {
            string folder = AppDomain.CurrentDomain.BaseDirectory + "XML_DATA";
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            string path = string.Format("{0}\\{1}.xml", folder, Guid.NewGuid().ToString("N"));
            using (XMLExtractor extractor = new XMLExtractor())
            {
                extractor.RegistrationName = "demo";
                extractor.RegistrationKey = "demo";
                // Load sample PDF document 
                extractor.LoadDocumentFromFile(filepath);
                extractor.TrimSpaces = true;
                extractor.ExtractColumnByColumn = true;
                extractor.SaveXMLToFile(path);
                return path;
            }
        }
        public static string GetKey(string filename)
        {
            //SHA1 sha1 = SHA1Managed.Create();
            //byte[] buff = File.ReadAllBytes(filename);
            //string fkey = Convert.ToBase64String(sha1.ComputeHash(buff));
            //return fkey;     

            return Guid.NewGuid().ToString();
        }
    }
}
