using Bytescout.PDFExtractor;
using Parse.Core;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Reduce.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            string xml = null;
            //xml = ReadMicrosFile(@"D:\Sample\Micros\cp_vds.43");
            //File.WriteAllText(@"D:\Sample\Micros\cp_vds.43.xml", xml);
            //string result = ApiHelper.PublishInvoice(xml, "01GTKT0/001");
            //xml = ReadOperaFile(@"D:\Sample\Opera\4.pdf");

            /* parse Micros*/
            //string path = @"E:\WORK\ESC\MayInAo\HD_sample\Micros\cp_all.43";
            //xml = ReadMicrosFile(path);

            /* parse Opera*/
            string path = @"D:\SVN\ESC\HDDT\MayInAo\HD_sample\Opera\20.pdf";
            xml = ReadOperaFile(path);
        }

        static string ReadOperaFile(string path)
        {
            string xmlpath = PdfUtils.PdfToXml(path);
            // read stream to string
            
            string data = File.ReadAllText(xmlpath);
          //  var service = new Parse.Hayatt.OperaDNGParseService();
            string xml = "";// service.GetXmlData(data);
            return xml;
        }        
    }
}
