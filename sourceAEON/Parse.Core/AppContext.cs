using Parse.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parse.Core
{
    public class AppContext
    {
        private static AppContext _Current;

        private Company _company;
        public Company company
        {
            get { return _company; }
            set { _company = value; }
        }
        public static AppContext Current
        {
            get { return _Current; }
        }

        public static void InitContext(Company com)
        {
            AppContext._Current = new AppContext()
            {
                _company = com,
            };
        }
    }
}
