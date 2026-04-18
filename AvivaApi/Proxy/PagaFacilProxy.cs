using AvivaApi.Proxy.Models.Requests;
using AvivaApi.Proxy.Models.Response;
using AvivaLibrary.Models;
using System.Text;
using System.Text.Json;

namespace AvivaApi.Proxy
{
    public class PagaFacilProxy(
        IHttpClientFactory xfactory,
        CConfig xconfig) : IProxy
    {
        private readonly IHttpClientFactory factory = xfactory;
        private readonly CConfig cconfig = xconfig;
        private readonly JsonSerializerOptions options = new()
        {
            PropertyNameCaseInsensitive = true,
        };

        public string ProviderName { get; } = "PAGAFACIL";

        public async Task<OrderCreated> CreateOrderAsync(OrderPago orderPago)
        {
            // Create specific class to access Paga Facil in Create Order
            var request = PagaFacilOrderCreateRequest.Factory(orderPago);

            // Create the payload
            string json = JsonSerializer.Serialize(request, options);
            HttpContent payload = new StringContent(json, Encoding.UTF8, "application/json");
            HttpClient client = HttpCreateClient(payload);

            var result = await client.PostAsync("Order", payload);
            string content = await result.Content.ReadAsStringAsync();
            if (result.IsSuccessStatusCode)
            {
                PagaFacilResponse? order = JsonSerializer.Deserialize<PagaFacilResponse>(content, options);
                if (order != null)
                {
                    // Return the Aviva object from the specific object from pago facil
                    return order.ConvertPago(ProviderName, orderPago);
                }

                throw new ApplicationException($"Order Created Returned null. something is very wrong in proxy in {ProviderName}:{client.BaseAddress}.");

            }
            throw new ApplicationException($"Error reading the proxy in {client.BaseAddress}Order, Error: {content}");
        }

        public async Task PayOrderAsync(string orderProviderId)
        {
            // Get the provider by Name
            var prov = cconfig.ProveedoresSettings.First(x => x.Nombre.Equals(ProviderName, StringComparison.InvariantCultureIgnoreCase));
            HttpClient client = factory.CreateClient("Provider");
            client.DefaultRequestHeaders.Add("x-api-key", prov.Key);
            client.BaseAddress = new Uri(prov.Url);

            HttpResponseMessage httpResponse = await client.PutAsync($"pay?id={orderProviderId}", null);
            string content = await httpResponse.Content.ReadAsStringAsync();
            if (httpResponse.IsSuccessStatusCode)
            {
                return;
            }

            throw new ApplicationException($"Access to Pay in the Provider {prov.Nombre} fail with:{httpResponse.ReasonPhrase} {content}");

        }

        /// <summary>
        /// Cancel a order....
        /// </summary>
        /// <param name="orderProviderId"></param>
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>
        public async Task CancelOrderAsync(string orderProviderId)
        {
            // Get the provider by Name
            var prov = cconfig.ProveedoresSettings.First(x => x.Nombre.Equals(ProviderName, StringComparison.InvariantCultureIgnoreCase));
            HttpClient client = factory.CreateClient("Provider");
            client.DefaultRequestHeaders.Add("x-api-key", prov.Key);
            client.BaseAddress = new Uri(prov.Url);

            HttpResponseMessage httpResponse = await client.PutAsync($"cancel?id={orderProviderId}", null);
            string content = await httpResponse.Content.ReadAsStringAsync();
            if (httpResponse.IsSuccessStatusCode)
            {
                return;
            }

            throw new ApplicationException($"Access to Cancel Pago in the Provider {prov.Nombre} fail with:{httpResponse.ReasonPhrase} {content}");
        }

        private HttpClient HttpCreateClient(HttpContent payload)
        {
            // Configure the Client for the service
            var client = factory.CreateClient("Provider");
            string url = string.Empty;
            foreach (var item in cconfig.ProveedoresSettings)
            {
                if (item.Nombre.Equals(ProviderName, StringComparison.InvariantCultureIgnoreCase))
                {
                    url = item.Url;
                    payload.Headers.Add("x-api-key", item.Key);
                    client.BaseAddress = new Uri(url);
                    break;
                }
            }
            if (url == string.Empty) { throw new ApplicationException($"Provider {ProviderName} not register"); }
            return client;
        }

    }

}
