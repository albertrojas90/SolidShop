using SolidShop.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SolidShop.Domain.Entities;

//Calcula el precio de un pedido.
//Procesa el pago con un procesador externo.
//Si falla → notifica error al cliente.
//Si tiene éxito → guarda el pedido y notifica éxito.

namespace SolidShop.Services
{
    public class orderService
    {
        private readonly IPriceCalculator _priceCalculator;
        private readonly IOrderWriter _orderWriter;
        private readonly IEnumerable<INotifier> _notifiers;
        private readonly IPaymentProcesor _payment;

        public orderService(
            IPriceCalculator priceCalculator,
            IOrderWriter orderWriter,
            IEnumerable<INotifier> notifiers,
            IPaymentProcesor payment)
        {
            _priceCalculator = priceCalculator;
            _orderWriter = orderWriter;
            _notifiers = notifiers;
            _payment = payment;
        }
        //metodo para crear y pagar pedido
        public async Task<(Order order, decimal total, PaymentResult payment)> CreateAndPayAsync(
       Order order, string currency, CancellationToken ct = default)
        {
            var (_, _, total) = _priceCalculator.Compute(order);

            var paymentResult = await _payment.ProcessAsync(order, new PaymentData(total, currency, $"ORD-{Guid.NewGuid():N}"), ct);

            if (!paymentResult.Success)
            {
                await NotifyAsync(order, $"Pago rechazado: {paymentResult.Message}", ct);
                return (order, total, paymentResult);
            }

            _orderWriter.Add(order);
            await NotifyAsync(order, $"Pedido {order.Id} confirmado. Total: {total:0.00} {currency}", ct);
            return (order, total, paymentResult);
        }

        private async Task NotifyAsync(Order order, string message, CancellationToken ct)
        {
            foreach (var n in _notifiers)
                await n.NotifyAsync(order, message, ct);
        }

    }
}


   