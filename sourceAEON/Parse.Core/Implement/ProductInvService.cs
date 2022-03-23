using FX.Data;
using Parse.Core.Domain;
using Parse.Core.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parse.Core.Implement
{
    public class ProductInvService : BaseService<ProductInv, int>, IProductInvService
    {
        public ProductInvService(string sessionFactoryConfigPath)
           : base(sessionFactoryConfigPath)
        { }

        public List<ProductInv> GetByInvoiceID(int invoiceID)
        {
            return Query.Where(x => x.InvoiceID == invoiceID).ToList();
        }
    }
}
