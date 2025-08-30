using SolidShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidShop.Domain.Contracts
{
    public interface IDiscountPolicy
    {
        string Name { get;}

        decimal ApplyDiscount(Product product, int quantity);
        decimal CalculateDiscount(Order order);
    }
}
