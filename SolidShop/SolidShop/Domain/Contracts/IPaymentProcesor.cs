using SolidShop.Domain.Contracts;
using SolidShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidShop.Domain.Contracts
{
    public record PaymentData(decimal Amount, string Currency, String Reference );
    public record PaymentResult(bool Success, string AuthorizationCode, string Message);
    public interface IPaymentProcesor
    {
        Task<PaymentResult> ProcessPayment(Order order, decimal amount, string currency, CancellationToken ct);
    }
}

