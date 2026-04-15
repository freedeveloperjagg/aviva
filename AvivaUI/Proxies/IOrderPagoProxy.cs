using AvivaLibrary.Models;
using AvivaLibrary.Models.Responses;

namespace AvivaUI.Proxies
{
    public interface IOrderPagoProxy
    {
        Task<OrderResponse?> SendOrderPago(OrderPago pago);
    }
}