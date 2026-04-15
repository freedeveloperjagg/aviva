using AvivaLibrary.Models;

namespace AvivaApi.Data
{
    public interface IPaymentRepository
    {
        Task<IEnumerable<EntidadDePago>> GetAllProvidersAsync();
        Task<EntidadDePago?> GetProviderByNameAsync(string nombre);
        Task<decimal?> CalculateFeeAsync(string providerName, string paymentType, decimal amount);
    }

}
