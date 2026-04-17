using AvivaLibrary.Models;
using AvivaLibrary.Models.Responses;
using AvivaUI.Proxies;


namespace AvivaUI.Services
{
    public class OrderPagoServices(IOrderPagoProxy xproxy) : IOrderPagoServices
    {
        IOrderPagoProxy proxy = xproxy;

        public async Task CancelOrder(int id)
        {
            await proxy.CancelOrder(id);
        }

        public async Task<List<OrderResponse>> GetAllOrdersFromProviders()
        {
            List<OrderResponse> response = await proxy.GetAllOrdersCreated();
            return response;
        }

        /// <summary>
        /// Pay the order in server
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task PaidOrder(int id)
        {
            await proxy.PaidOrder(id);
        }

        public async Task<OrderResponse?> SendOrderPago(OrderPago pago)
        {
            OrderResponse? response = await proxy.SendOrderPago(pago);
            return response;
        }
    }
}
