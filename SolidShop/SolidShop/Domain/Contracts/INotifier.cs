using SolidShop.Domain.Contracts;
using SolidShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidShop.Domain.Contracts
{
    public interface INotifier
    {
        Task NotifyAsync(Order order, PaymentResult payment, CancellationToken ct);
    }
}

