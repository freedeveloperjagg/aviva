using AvivaLibrary.Models;
using AvivaLibrary.Models.Responses;
using System.Buffers.Text;
using System.Text;
using System.Text.Json;

namespace AvivaUI.Proxies
{
    public class OrderPagoProxy(
        IHttpClientFactory xfactory,
        CConfig xconfig) : IOrderPagoProxy
    {
        private readonly IHttpClientFactory factory = xfactory;
        private readonly CConfig cconfig = xconfig;
        private readonly JsonSerializerOptions options = new()
        {
            PropertyNameCaseInsensitive = true,
        };

        public async Task<OrderResponse?> SendOrderPago(OrderPago pago)
        {
            string json = JsonSerializer.Serialize(pago);
            HttpContent payload = new StringContent(json, Encoding.UTF8, "application/json");

            var client = factory.CreateClient();
            client.BaseAddress = new Uri(cconfig.ApiAddress);

            var result = await client.PostAsync("api/Payment", payload);
            string content = await result.Content.ReadAsStringAsync();
            if (result.IsSuccessStatusCode)
            {
                OrderResponse? order = JsonSerializer.Deserialize<OrderResponse>(content, options);
                return order;
            }

            throw new ApplicationException($"Error reading the proxy in {client.BaseAddress}, Error: {content}");
        }

    }
}
