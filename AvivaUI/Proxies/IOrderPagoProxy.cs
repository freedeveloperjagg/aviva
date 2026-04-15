using AvivaLibrary.Models;
using AvivaLibrary.Models.Responses;

namespace AvivaUI.Proxies
{
    public interface IOrderPagoProxy
    {
        /// <summary>
        /// Send the request to the API for all payment Orders
        /// </summary>
        /// <returns>
        /// The list of payment orders
        /// </returns>
        Task<List<OrderResponse>> GetAllOrdersFromProviders();
        
        Task<OrderResponse?> SendOrderPago(OrderPago pago);
    }
}