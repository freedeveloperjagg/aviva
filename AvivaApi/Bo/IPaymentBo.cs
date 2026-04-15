using AvivaLibrary.Models;
using AvivaLibrary.Models.Requests;
using AvivaLibrary.Models.Responses;

namespace AvivaApi.Bo
{
    public interface IPaymentBo
    {
        Task<OrderResponse?> CreatePaymentAsync(OrderPago order);
        Task<OrderResponse?> GetOrderAsync(string id, string provider);
        Task<List<OrderResponse>> GetOrdersAsync();
        Task PayOrderAsync(ChangeOrderRequest request);
        Task CancelOrderAsync(ChangeOrderRequest request);
       
    }
}