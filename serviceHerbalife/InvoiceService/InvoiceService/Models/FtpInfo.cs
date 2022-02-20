using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceService.Models
{
    public class FtpInfo
    {
        public string FtpLink { get; set; }
        public string FtpUsername { get; set; }
        public string FtpPassword { get; set; }
        public string NewIssueXMLPath { get; set; }
        public string IssueSuccessPath { get; set; }
        public string IssueFailedPath { get; set; }
        public string ReIssuePath { get; set; }
        public string AdjustXMLPath { get; set; }
        public string AdjustSuccessPath { get; set; }
        public string AdjustFailedPath { get; set; }
        public string CancelXMLPath { get; set; }
        public string CancelSuccessPath { get; set; }
        public string CancelFailedPath { get; set; }
    }
}
