using AvivaLibrary.Models;
namespace AvivaApi.Proxy
{
    public interface IProxy
    {
        Task<OrderCreated> CreateOrderAsync(OrderPago orderPago);
        Task<OrderCreated?> GetOrderAsync(string orderProviderId);
        Task<List<OrderCreated>> GetOrdersAsync();
        Task PayOrderAsync(string orderProviderId);
        Task CancelOrderAsync(string orderProviderId);
    }
}
