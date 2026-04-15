using AvivaLibrary.Models;
using AvivaLibrary.Models.Responses;
using AvivaUI.Proxies;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace AvivaUI.Services
{
    public class OrderPagoServices(IOrderPagoProxy xproxy) : IOrderPagoServices
    {
        IOrderPagoProxy proxy = xproxy;

        public async Task<List<OrderResponse>> GetAllOrdersFromProviders()
        {
            List<OrderResponse> response = await proxy.GetAllOrdersFromProviders();
            return response;
        }

        public async Task<OrderResponse?> SendOrderPago(OrderPago pago)
        {
            OrderResponse? response = await proxy.SendOrderPago(pago);
            return response;
        }
    }
}
