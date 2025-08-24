using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidShop.Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public List<OrderLine> Lines { get; } = new();

        public decimal Subtotal => Lines.Sum(l => l.LineTotal);
    }
}
