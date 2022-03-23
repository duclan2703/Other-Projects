using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parse.Core
{
    public class Common
    {
        public enum ProductType
        {
            Product,
            Free,
        }

        public static Dictionary<int, string> lstProductType = new Dictionary<int, string>()
        {
            {(int)ProductType.Product, "Hàng bán"},
            {(int)ProductType.Free, "Hàng khuyến mại"},
        };

        public static Dictionary<string, string> lstPayMethodType = new Dictionary<string, string>()
        {
            {"TM", "TM"},
            {"CK", "CK"},
            {"TM/CK", "TM/CK"},
            {"Khác", "Khác"},
        };

        public static Dictionary<float, string> lstTax = new Dictionary<float, string>()
        {
            {-1,"Không thuế"},
            {0, "0%"},
            {5, "5%"},
            {10, "10%"},
            {15, "15%"},
        };
    }
}
