using AvivaLibrary.Models;
namespace AvivaApi.Proxy
{
    public interface IProxy
    {
        /// <summary>
        /// Enter the provider identificator in the system
        /// </summary>
        public string ProviderName { get; }

        Task<OrderCreated> CreateOrderAsync(OrderPago orderPago);
        //Task<OrderCreated?> GetOrderAsync(string orderProviderId);
        //Task<List<OrderCreated>> GetOrdersAsync();
        Task PayOrderAsync(string orderProviderId);
        Task CancelOrderAsync(string orderProviderId);
    }
}
