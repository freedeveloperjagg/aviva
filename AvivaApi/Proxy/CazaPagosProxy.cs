using AvivaApi.Proxy.Models.Requests;
using AvivaApi.Proxy.Models.Response;
using AvivaLibrary.Models;
using System.Text;
using System.Text.Json;

namespace AvivaApi.Proxy
{
    /// <summary>
    /// Proxy customized to CazaPagos Provider
    /// </summary>
    /// <param name="xfactory"></param>
    /// <param name="xconfig"></param>
    public class CazaPagosProxy(
        IHttpClientFactory xfactory,
        CConfig xconfig) : IProxy
    {
        private readonly IHttpClientFactory factory = xfactory;
        private readonly CConfig cconfig = xconfig;
        private readonly JsonSerializerOptions options = new()
        {
            PropertyNameCaseInsensitive = true,
        };

        public string ProviderName { get; } = "CAZAPAGOS";

        public async Task<OrderCreated> CreateOrderAsync(OrderPago orderPago)
        {
            // Create the object specific for caza pago
            var request = CazaPagosOrderPagoRequest.Factory(orderPago);
            
            // Create the payload
            string json = JsonSerializer.Serialize(request);
            HttpContent payload = new StringContent(json, Encoding.UTF8, "application/json");

            HttpClient client = factory.CreateClient("Provider");

            // Get the Url for the service
            string url = string.Empty;
            foreach (var item in cconfig.ProveedoresSettings)
            {
                if (item.Nombre.Equals(ProviderName, StringComparison.InvariantCultureIgnoreCase))
                {
                    url = item.Url;
                    client.DefaultRequestHeaders.Add("x-api-key", item.Key);
                    break;
                }
            }

            if (url == string.Empty) { throw new ApplicationException($"Provider {ProviderName} not register"); }

            client.BaseAddress = new Uri(url);
            var result = await client.PostAsync("Order", payload);
            string content = await result.Content.ReadAsStringAsync();
            if (result.IsSuccessStatusCode)
            {
                CazaPagosResponse? orderProvider = JsonSerializer.Deserialize<CazaPagosResponse>(content, options);
                if (orderProvider != null)
                {
                    // Conver to aviva response ( OrderCreate object )
                    return orderProvider.ConvertPago(ProviderName,orderPago);
                }

                throw new ApplicationException($"Order Created Returned null. something is very wrong in proxy in {ProviderName}:{url}.");

            }
            throw new ApplicationException($"Error reading the proxy in {url}, Error: {content}");
        }
            
        public async Task PayOrderAsync(string orderProviderId)
        {
            // Get the provider by Name
            var prov = cconfig.ProveedoresSettings.First(x => x.Nombre.ToUpperInvariant() == ProviderName);
            HttpClient client = factory.CreateClient("Provider");
            client.DefaultRequestHeaders.Add("x-api-key", prov.Key);
            client.BaseAddress = new Uri(prov.Url);

            HttpResponseMessage httpResponse = await client.PutAsync($"payment?id={orderProviderId}", null);
            string content = await httpResponse.Content.ReadAsStringAsync();
            if (httpResponse.IsSuccessStatusCode)
            {
                return;
            }

            throw new ApplicationException($"Access to Pay in the Provider {prov.Nombre} fail with:{httpResponse.ReasonPhrase} {content}");

        }

        public async Task CancelOrderAsync(string orderProviderId)
        {
            // Get the provider by Name
            var prov = cconfig.ProveedoresSettings.First(x => x.Nombre.ToUpperInvariant() == ProviderName);
            HttpClient client = factory.CreateClient("Provider");
            client.DefaultRequestHeaders.Add("x-api-key", prov.Key);
            client.BaseAddress = new Uri(prov.Url);
            
            HttpResponseMessage httpResponse = await client.PutAsync($"cancellation?id={orderProviderId}", null);
            string content = await httpResponse.Content.ReadAsStringAsync();
            if (httpResponse.IsSuccessStatusCode)
            {
                return;
            }

            throw new ApplicationException($"Access to Cancel Pago in the Provider {prov.Nombre} fail with:{httpResponse.ReasonPhrase} {content}");

        }
    }
}
