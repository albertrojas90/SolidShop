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
        decimal CalculateDiscount(Order order);
    }
}
