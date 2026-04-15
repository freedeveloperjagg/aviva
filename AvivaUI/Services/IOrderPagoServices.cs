using AvivaLibrary.Models;
using AvivaLibrary.Models.Responses;

namespace AvivaUI.Services
{
    public interface IOrderPagoServices
    {
        /// <summary>
        /// Get all payment orders from all provider registered 
        /// </summary>
        /// <returns></returns>
        Task<List<OrderResponse>> GetAllOrdersFromProviders();
        
        /// <summary>
        /// Send a individual payment order, the underline logic in 
        /// the API select automatically the best provider
        /// </summary>
        /// <param name="pago"></param>
        /// <returns></returns>
        Task<OrderResponse?> SendOrderPago(OrderPago pago);
    }
}