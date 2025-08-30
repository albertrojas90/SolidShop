using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SolidShop.Domain.Contracts;
using SolidShop.Domain.Entities;


namespace SolidShop.Infrastructure;


    /// <summary>
    /// Simula un procesador de tarjetas de crédito.
    /// Cumple LSP: para flujos esperados (p. ej. monto inválido, moneda no soportada, rechazo)
    /// devuelve PaymentResult.Success = false en lugar de lanzar excepciones.
    /// </summary>
    public class CreditCardProcessor : IPaymentProcesor
    {
        /// <summary>
        /// Procesa el pago de forma asíncrona.
        /// - No lanza excepciones por rechazos o validaciones de negocio.
        /// - Devuelve PaymentResult con detalles sobre aprobación/rechazo.
        /// - Respeta el CancellationToken.
        /// </summary>
        public async Task<PaymentResult> ProcessAsync(Order order, PaymentData data, CancellationToken ct = default)
        {
            // Validaciones básicas (flujos esperados -> devolver PaymentResult con Success = false)
            if (data.Amount <= 0m)
                return new PaymentResult(false, string.Empty, "Monto inválido.");

            if (string.IsNullOrWhiteSpace(data.Currency))
                return new PaymentResult(false, string.Empty, "Moneda requerida.");

            // Respeta cancelación antes de simular I/O
            if (ct.IsCancellationRequested)
                return new PaymentResult(false, string.Empty, "Operación cancelada.");

            // Simular llamada a gateway externo (I/O)
            try
            {
                await Task.Delay(250, ct); // simula latencia de red / servicio externo
            }
            catch (OperationCanceledException)
            {
                return new PaymentResult(false, string.Empty, "Operación cancelada.");
            }

            // Soporte de moneda (ejemplo: solo USD)
            if (!string.Equals(data.Currency, "USD", StringComparison.OrdinalIgnoreCase))
                return new PaymentResult(false, string.Empty, $"Moneda no soportada: {data.Currency}");

            // Reglas de negocio sencillas:
            // - Límite máximo aceptable (simulación)
            if (data.Amount > 10_000m)
                return new PaymentResult(false, string.Empty, "Monto excede el límite permitido para tarjetas.");

            // - Simular un rechazo aleatorio pequeño (p. ej. 5% de probabilidad)
            var chance = Random.Shared.NextDouble();
            if (chance < 0.05)
                return new PaymentResult(false, string.Empty, "Transacción rechazada por el emisor.");

            // Si todo bien -> autorización simulada
            var authorization = $"CC-{DateTime.UtcNow:yyyyMMddHHmmss}-{Random.Shared.Next(1000, 9999)}";

            // Logging mínimo (solo consola en este ejemplo)
            Console.WriteLine($"[CreditCardProcessor] Aprobado | Auth={authorization} | Amount={data.Amount:0.00} {data.Currency} | Ref={data.Reference}");

            return new PaymentResult(true, authorization, "Aprobado");
        }
    }

