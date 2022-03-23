using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViettelAPI.Models
{
    public class PDFFileResponse
    {
        public string errorCode { get; set; }
        public string description { get; set; }
        public string fileName { get; set; }
        public byte[] fileToBytes { get; set; }
    }
}
