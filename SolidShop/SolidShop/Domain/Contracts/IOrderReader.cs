using SolidShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidShop.Domain.Contracts
{
    public interface IOrderReader
    {
        Order? GetById(int id);
        IReadOnlyList<Order> GetAll();
    }
}
