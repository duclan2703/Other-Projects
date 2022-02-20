using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parse.Core.Domain
{
    public class Company
    {
        public string TaxCode { get; set; }
        public string StartTime { get; set; }
        public string ProcessTime { get; set; }
        public string Url { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Pattern { get; set; }
        public string Serial { get; set; }
    }
}
