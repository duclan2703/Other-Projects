using System;

namespace Parse.Core.Domain
{
    public interface IProductInv
    {
        string Code { get; set; }
        string Name { get; set; }
        string Unit { get; set; }
        string Remark { get; set; }
        Decimal Quantity { get; set; }
        Decimal Price { get; set; }
        Decimal Total { get; set; }
        Decimal Amount { get; set; }
    }
}
