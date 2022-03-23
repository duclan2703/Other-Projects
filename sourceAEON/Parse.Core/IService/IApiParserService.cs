using Parse.Core.Domain;
using Parse.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parse.Core.IService
{
    public interface IApiParserService
    {
        List<InvoiceModels> ConvertToAPIModel(List<InvoiceVAT> invoices, Company company);
    }
}
