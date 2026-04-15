using AvivaLibrary.Models;
using AvivaLibrary.Models.Responses;
using AvivaUI.Proxies;


namespace AvivaUI.Services
{
    public class OrderPagoServices(IOrderPagoProxy xproxy) : IOrderPagoServices
    {
        IOrderPagoProxy proxy = xproxy;

        public async Task<OrderResponse?> SendOrderPago(OrderPago pago)
        {
            OrderResponse? response = await proxy.SendOrderPago(pago);
            return response;
        }
    }
}
