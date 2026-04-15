using AvivaLibrary.Models;
using AvivaLibrary.Models.Requests;
using AvivaLibrary.Models.Responses;

namespace AvivaApi.Proxy
{
    public interface IPaymentProxy
    {
        Task CancelOrderAsync(ChangeOrderRequest request);

        /// <summary>
        /// Create a Order.....
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="orderPago"></param>
        /// <returns></returns>
        Task<OrderResponse?> CreateOrderAsync(EntidadDePago provider, OrderPago orderPago);
        Task<OrderResponse?> GetOrderAsync(string id, string provider);
        Task<List<OrderResponse>> GetOrdersAsync(ProveedorSetting prov);
        Task PayOrderAsync(ChangeOrderRequest request);
    }
}
