using AvivaApi.Data;
using AvivaLibrary.Models;

namespace AvivaApi.Facade
{
    /// <summary>
    /// Manage the access to the provider register in the DB and the fee calculation logic.
    /// </summary>
    /// <param name="rep"></param>
    public class PaymentCompaniesFacade(IPaymentRepository rep) : IPaymentCompaniesFacade
    {
        readonly IPaymentRepository repository = rep;

        public async Task<List<EntidadDePago>> GetAllProvidersAsync()
        {
            var providers = await repository.GetAllProvidersAsync();
            return providers.ToList();
        }

        public async Task<EntidadDePago?> GetProviderByNameAsync(string nombre)
        {
            var provider = await repository.GetProviderByNameAsync(nombre);
            return provider;
        }

        public async Task<decimal?> CalculateFeeAsync(string providerName, string paymentType, decimal amount)
        {
            var fee = await repository.CalculateFeeAsync(providerName, paymentType, amount);
            return fee;
        }


    }
}
