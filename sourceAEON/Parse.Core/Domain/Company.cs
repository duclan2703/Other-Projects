using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parse.Core.Domain
{
    public class Company
    {        
        public virtual int Id { get; set; }
        public virtual string InvPattern { get; set; }
        public virtual string Name { get; set; }
        public virtual string TaxCode { get; set; }
        public virtual string Address { get; set; }
        public virtual string BankName { get; set; }
        public virtual string BankNumber { get; set; }
        public virtual string Email { get; set; }
        public virtual string Fax { get; set; }
        public virtual string Phone { get; set; }        
        public virtual string UserName { get; set; }
        public virtual string PassWord { get; set; }                
        public virtual string Domain { get; set; }
        public virtual string Code { get; set; }
        public virtual string InvSerial { get; set; }

        private IDictionary<string, string> _Config = new Dictionary<string, string>();
        public virtual IDictionary<string, string> Config
        {
            get { return _Config; }
            set { _Config = value; }
        }
    }
}
