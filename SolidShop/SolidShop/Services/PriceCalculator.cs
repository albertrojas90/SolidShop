using SolidShop.Domain.Contracts;
using SolidShop.Domain.Entities;
using SolidShop.Infrastructure.Discounts;
using System.Collections.Generic;
using System.Linq;

namespace SolidShop.Services;

public class PriceCalculator : IPriceCalculator
{
    private readonly IEnumerable<Domain.Contracts.IDiscountPolicy> _discountPolicies;

    public PriceCalculator(IEnumerable<Domain.Contracts.IDiscountPolicy> discountPolicies)
    {
        _discountPolicies = discountPolicies;
    }

    public decimal CalculateTotal(Product product, int quantity)
    {
        decimal subtotal = product.UnitPrice * quantity;
        decimal totalDiscount = _discountPolicies.Sum(p => p.ApplyDiscount(product, quantity));
        return subtotal - totalDiscount;
    }

    public (decimal subtotal, decimal discount, decimal total) Compute(Order order)
    {
        throw new NotImplementedException();
    }
}




/*using SolidShop.Domain.Contracts;
using SolidShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidShop.Services;

public class PriceCalculator : IPriceCalculator
{
    private readonly IEnumerable<IDiscountPolicy> _policies;

    public PriceCalculator(IEnumerable<IDiscountPolicy> policies)
    {
        _policies = policies;
    }

    public (decimal subtotal, decimal discount, decimal total) Compute(Order order) 
    {
        var subtotal = order.Subtotal;
        var discount = _policies.DefaultIfEmpty().Max(p => p?.CalculateDiscount(order) ?? 0m);
        var total = Math.Max(0m, subtotal - discount);
        return (subtotal, discount, total);

    }
}*/

