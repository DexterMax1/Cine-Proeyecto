namespace Dealer.Shared
{
    public record ServiceResponse<T>(bool Success = false, string Message = "", T? Data = default);
}