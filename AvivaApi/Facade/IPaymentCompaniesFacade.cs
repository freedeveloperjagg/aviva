using AvivaLibrary.Models;

namespace AvivaApi.Facade
{
    public interface IPaymentCompaniesFacade
    {
        Task<decimal?> CalculateFeeAsync(string providerName, string paymentType, decimal amount);
        Task<List<EntidadDePago>> GetAllProvidersAsync();
        Task<EntidadDePago?> GetProviderByNameAsync(string nombre);
    }
}