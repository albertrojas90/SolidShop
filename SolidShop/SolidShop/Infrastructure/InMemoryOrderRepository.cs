using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SolidShop.Domain.Contracts;
using SolidShop.Domain.Entities;

namespace SolidShop.Infrastructure
{
    public class InMemoryOrderRepository : IOrderReader, IOrderWriter
    {
        private readonly List<Order> _orders = new();
        private int _seq = 1;

        public void Add(Order order)
        {
            order.Id = _seq++;
            _orders.Add(order);
        }
        public void Update(Order order)
        {
            var idx = _orders.FindIndex(o => o.Id == order.Id);
            if (idx >= 0) _orders[idx] = order;
        }
        public IReadOnlyList<Order> GetAll() => _orders.AsReadOnly();

        public Order? GetById(int id) => _orders.FirstOrDefault(o => o.Id == id);

    }

}

