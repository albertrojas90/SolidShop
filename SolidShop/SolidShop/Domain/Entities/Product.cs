using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidShop.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; init; } = string.Empty;
        public decimal UnitPrice {  get; init; }

    }
}
