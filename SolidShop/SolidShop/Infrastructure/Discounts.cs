using SolidShop.Domain.Entities;


namespace SolidShop.Infrastructure.Discounts
{
    public interface IDiscountPolicy
    {
        decimal ApplyDiscount(Product product, int quantity);
    }

    public class NoDiscountPolicy : IDiscountPolicy
    {
        public decimal ApplyDiscount(Product product, int quantity) => 0;
    }

    public class BulkQuantityPolicy : IDiscountPolicy
    {
        public decimal ApplyDiscount(Product product, int quantity)
        {
            if (quantity >= 10)
                return product.UnitPrice * quantity * 0.10m; // 10% descuento
            return 0;
        }
    }

    public class BlackFridayPolicy : IDiscountPolicy
    {
        public decimal ApplyDiscount(Product product, int quantity)
        {
            return product.UnitPrice * quantity * 0.30m; // 30% descuento
        }
    }
}
