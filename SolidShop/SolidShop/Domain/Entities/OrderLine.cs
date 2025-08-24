using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidShop.Domain.Entities
{
    public class OrderLine
    {
        public Product Product { get; init; } = default!;
        public int Quantity { get; init; }
        public decimal LineTotal => Product.UniPrice*Quantity
    }
}
