namespace SolidShop.Domain.Contracts
{
    public record PaymentResult(bool Success, string AuthorizationCode, string Message);
    
}
