using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SolidShop.Domain.Contracts;
using SolidShop.Domain.Entities;

namespace SolidShop.Infrastructure;
public class EmailNotifier : INotifier
{
    public Task NotifyAsync(Order order, string message, CancellationToken ct = default)
    {
        Console.WriteLine($"[EMAIL] To: {order.CustomerName} | {message}");
        return Task.CompletedTask;
    }
}