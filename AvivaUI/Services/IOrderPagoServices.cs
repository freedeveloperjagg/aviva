using AvivaLibrary.Models;
using AvivaLibrary.Models.Responses;

namespace AvivaUI.Services
{
    public interface IOrderPagoServices
    {
        Task<OrderResponse?> SendOrderPago(OrderPago pago);
    }
}