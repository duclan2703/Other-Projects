using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parse.AEON
{
    public class UploadError
    {
        public string FolioNo { get; set; }
        public List<ErrorDetail> ErrorList { get; set; }
    }
}
