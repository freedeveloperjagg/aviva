using AvivaApi.Proxy;
using AvivaLibrary.Models;
using AvivaLibrary.Models.Responses;
using System.Reflection;
using System.Xml.XPath;

namespace AvivaApi.Services
{
    public class ProviderService(IPaymentProxy xproxy) : IProviderService
    {
        readonly IPaymentProxy proxy = xproxy;


        public async Task<OrderResponse?> MakePaymentAsync(EntidadDePago provider, OrderPago orderPago)
        {
            OrderResponse? ordercreated = await proxy.CreateOrderAsync(provider, orderPago);
            return ordercreated;


            
        }
    }
}
