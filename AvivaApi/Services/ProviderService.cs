using AvivaApi.Proxy;
using AvivaLibrary.Models;
using AvivaLibrary.Models.Requests;
using AvivaLibrary.Models.Responses;
using System.Reflection;
using System.Xml.XPath;

namespace AvivaApi.Services
{
    public class ProviderService(
        IPaymentProxy xproxy,
        CConfig xconfig) : IProviderService
    {
        readonly IPaymentProxy proxy = xproxy;
        readonly CConfig cconfig = xconfig;

        public async Task<OrderResponse?> GetOrderAsync(string id, string provider)
        {
            OrderResponse? order = await proxy.GetOrderAsync(id, provider);
            return order;
        }        

        public async Task<List<OrderResponse>> GetOrdersAsync()
        {
            List<ProveedorSetting> proveedors = cconfig.ProveedoresSettings;
            List<OrderResponse> orders = [];
            foreach (ProveedorSetting prov in proveedors)
            {
                var ords = await proxy.GetOrdersAsync(prov);
                orders.AddRange(ords);
            }
                       
            return orders;
        }

        public async Task<OrderResponse?> CreateOrderAsync(EntidadDePago provider, OrderPago orderPago)
        {
            OrderResponse? response = await proxy.CreateOrderAsync(provider, orderPago);
            return response;
        }

        public async Task CancelOrderAsync(ChangeOrderRequest request)
        {
            await proxy.CancelOrderAsync(request);
        }

        public async Task PayOrderAsync(ChangeOrderRequest request)
        {
            await proxy.PayOrderAsync(request);
        }
    }
}
