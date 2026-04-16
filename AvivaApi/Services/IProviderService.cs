using AvivaLibrary.Models;

namespace AvivaApi.Services
{
    public interface IProviderService
    {
        Task<List<OrderCreated>> GetOrdersAsync();
        Task<OrderCreated?> GetOrderByIdAsync(int id);
        Task<OrderCreated?> CreateOrderAsync(string providerName, OrderPago orderPago);
        
        /// <summary>
        /// Cancel the order using the internal Id
        /// This cancel the order in the provider and internally.
        /// </summary>
        /// <param name="id">internal ID</param>
        /// <returns>nothing</returns>
        Task CancelOrderAsync(int id);
        Task PayOrderAsync(int id);
    }
}
