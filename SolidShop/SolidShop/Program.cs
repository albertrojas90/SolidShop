using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SolidShop.Domain.Contracts;
using SolidShop.Domain.Entities;
using SolidShop.Infrastructure;
using SolidShop.Infrastructure.Discounts;
using SolidShop.Services;
using System.Threading;

var builder = WebApplication.CreateBuilder(args);

// --- Registrar servicios de dominio
builder.Services.AddSingleton<IPriceCalculator, PriceCalculator>();
builder.Services.AddSingleton<InMemoryOrderRepository>(); // repositorio concreto

// Registrar interfaces segregadas apuntando al mismo singleton
builder.Services.AddSingleton<IOrderReader>(sp => sp.GetRequiredService<InMemoryOrderRepository>());
builder.Services.AddSingleton<IOrderWriter>(sp => sp.GetRequiredService<InMemoryOrderRepository>());

// Notifiers: inyectar como colección
builder.Services.AddSingleton<INotifier, EmailNotifier>();
builder.Services.AddSingleton<INotifier, SmsNotifier>();

// Discount policies: inyectar como colección
builder.Services.AddSingleton<IDiscountPolicy, NoDiscountPolicy>();
builder.Services.AddSingleton<IDiscountPolicy, BulkQuantityPolicy>();
builder.Services.AddSingleton<IDiscountPolicy, BlackFridayPolicy>();

// Payment processor
builder.Services.AddSingleton<IPaymentProcesor, CreditCardProcessor>();

// Servicio de dominio
builder.Services.AddScoped<orderService>();

var app = builder.Build();

// Endpoint: crear y pagar orden
app.MapPost("/api/orders", async (OrderRequest request, orderService orderService, CancellationToken ct) =>
{
    // Construye un Order simple con una sola línea
    var order = new Order
    {
        CustomerName = request.CustomerName
    };

    // Creamos un Product sencillo y una linea con cantidad 1
    var product = new Product
    {
        Id = 1,
        Name = request.ProductName ?? "Item",
        UnitPrice = request.Amount // Nota: unifica con UnitPrice en Product
    };

    order.Lines.Add(new OrderLine { Product = product, Quantity = 1 });

    // Llamamos al método existente de tu service
    var (savedOrder, total, payment) = await orderService.CreateAndPayAsync(order, request.Currency ?? "USD", ct);

    // Respuesta con detalles del pago
    return Results.Ok(new
    {
        savedOrder.Id,
        savedOrder.CustomerName,
        Subtotal = savedOrder.Subtotal,
        Total = total,
        PaymentSuccess = payment.Success,
        payment.AuthorizationCode,
        payment.Message
    });
});

app.Run();

// DTO usado por Postman
public record OrderRequest(string CustomerName, decimal Amount, string? Currency = "USD", string? ProductName = "Item");
