using AvivaLibrary.Models;
using AvivaLibrary.Models.Responses;
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

        /// <summary>
        /// Mark as cancel a order.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>
        public async Task CancelOrder(int id)
        {
            var client = factory.CreateClient();
            client.BaseAddress = new Uri(cconfig.ApiAddress);

            var result = await client.PutAsync($"api/Payment/cancel?id={id}", null);
            string content = await result.Content.ReadAsStringAsync();
            if (result.IsSuccessStatusCode)
            {
                return;
            }

            throw new ApplicationException($"Error reading the proxy in {client.BaseAddress}, Error: {content}");
        }

        /// <summary>
        /// Get All payment orders from providers
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>
        public async Task<List<OrderResponse>> GetAllOrdersCreated()
        {
            var client = factory.CreateClient();
            client.BaseAddress = new Uri(cconfig.ApiAddress);

            var result = await client.GetAsync("api/Payment/Orders");
            string content = await result.Content.ReadAsStringAsync();
            if (result.IsSuccessStatusCode)
            {
                List<OrderResponse>? orders = JsonSerializer.Deserialize<List<OrderResponse>>(content, options);
                if (orders == null) return [];
                return orders;
            }

            throw new ApplicationException($"Get All PAyment Oders, Error reading the proxy in {client.BaseAddress}, Error: {content}");
        }

        public async Task PaidOrder(int id)
        {
            var client = factory.CreateClient();
            client.BaseAddress = new Uri(cconfig.ApiAddress);

            var result = await client.PutAsync($"api/Payment/pay?id={id}", null);
            string content = await result.Content.ReadAsStringAsync();
            if (result.IsSuccessStatusCode)
            {
                return;
            }
            throw new ApplicationException($"Error reading the proxy in {client.BaseAddress}, Error: {content}");
        }

        public async Task<OrderResponse?> SendOrderPago(OrderPago pago)
        {
            string json = JsonSerializer.Serialize(pago);
            HttpContent payload = new StringContent(json, Encoding.UTF8, "application/json");

            var client = factory.CreateClient();
            client.BaseAddress = new Uri(cconfig.ApiAddress);

            var result = await client.PostAsync("api/Payment/Create", payload);
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
