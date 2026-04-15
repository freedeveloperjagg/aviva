using AvivaLibrary.Models;
using AvivaLibrary.Models.Responses;

namespace AvivaApi.Proxy
{
    public interface IPaymentProxy
    {
        /// <summary>
        /// Create a Order.....
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="orderPago"></param>
        /// <returns></returns>
        Task<OrderResponse?> CreateOrderAsync(EntidadDePago provider, OrderPago orderPago);
    }
}
