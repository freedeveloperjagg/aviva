using AvivaLibrary.Models;
using AvivaLibrary.Models.Requests;
using AvivaLibrary.Models.Responses;

namespace AvivaApi.Services
{
    public interface IProviderService
    {
        Task<List<OrderResponse>> GetOrdersAsync();
        Task<OrderResponse?> GetOrderAsync(string id, string provider);
        Task<OrderResponse?> CreateOrderAsync(EntidadDePago provider, OrderPago orderPago);
        Task CancelOrderAsync(ChangeOrderRequest request);
        Task PayOrderAsync(ChangeOrderRequest request);
    }
}
