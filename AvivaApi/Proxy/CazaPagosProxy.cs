using AvivaApi.Proxy.Models;
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

        // All in upper cases
        private const string ProviderName = "CAZAPAGOS";

        public async Task<OrderCreated> CreateOrderAsync(OrderPago orderPago)
        {
            // Create the payload
            string json = JsonSerializer.Serialize(orderPago);
            HttpContent payload = new StringContent(json, Encoding.UTF8, "application/json");

            HttpClient client = factory.CreateClient("Provider");

            // Get the Url for the service
            string url = string.Empty;
            foreach (var item in cconfig.ProveedoresSettings)
            {
                if (item.Nombre.Equals(ProviderName, StringComparison.InvariantCultureIgnoreCase))
                {
                    url = item.Url;
                    payload.Headers.Add("x-api-key", item.Key);
                    break;
                }
            }

            if (url == string.Empty) { throw new ApplicationException($"Provider {ProviderName} not register"); }

            client.BaseAddress = new Uri(url);
            var result = await client.PostAsync("Order", payload);
            string content = await result.Content.ReadAsStringAsync();
            if (result.IsSuccessStatusCode)
            {
                Models.CazaPagosResponse? order = JsonSerializer.Deserialize<Models.CazaPagosResponse>(content, options);
                if (order != null)
                {
                    return order.ConvertPago(ProviderName);
                }

                throw new ApplicationException($"Order Created Returned null. something is very wrong in proxy in {ProviderName}:{url}.");

            }
            throw new ApplicationException($"Error reading the proxy in {url}, Error: {content}");
        }

        public async Task<OrderCreated?> GetOrderAsync(string orderProviderId)
        {
            var prov = cconfig.ProveedoresSettings.SingleOrDefault(x => x.Nombre.Equals(ProviderName, StringComparison.InvariantCultureIgnoreCase)) ?? throw new ArgumentException($"Provider Unknow when try to get order Id {orderProviderId}");
            HttpClient client = factory.CreateClient("Provider");
            client.DefaultRequestHeaders.Add("x-api-key", prov.Key);
            client.BaseAddress = new Uri(prov.Url);

            var httpResponse = await client.GetAsync("Order/orderProviderId");
            string content = await httpResponse.Content.ReadAsStringAsync();
            if (httpResponse.IsSuccessStatusCode)
            {
                CazaPagosResponse? order = JsonSerializer.Deserialize<CazaPagosResponse>(content, options);
                if (order == null) return null;
                OrderCreated ord = order.ConvertPago(ProviderName);
                return ord;
            }
            throw new ApplicationException($"Access the Provider {prov.Nombre} fail with: {content}");
        }

        /// <summary>
        /// NOTE: This method is not more used. the information is got from the DB
        /// </summary>
        /// <returns></returns>
        public async Task<List<OrderCreated>> GetOrdersAsync()
        {
            var prov = cconfig.ProveedoresSettings.SingleOrDefault(x => x.Nombre.Equals(ProviderName, StringComparison.InvariantCultureIgnoreCase)) ?? throw new ArgumentException($"Provider Unknow when try to get orders");
            HttpClient client = factory.CreateClient("Provider");
            client.DefaultRequestHeaders.Add("x-api-key", prov.Key);
            client.BaseAddress = new Uri(prov.Url);

            var httpResponse = await client.GetAsync("Order");
            string content = await httpResponse.Content.ReadAsStringAsync();
            if (httpResponse.IsSuccessStatusCode)
            {
                List<CazaPagosResponse>? orders = JsonSerializer.Deserialize<List<CazaPagosResponse>>(content, options);
                if (orders == null) return [];
                List<OrderCreated> ord = [];
                foreach (var orderItem in orders)
                {
                    ord.Add(orderItem.ConvertPago(ProviderName));
                }
                return ord;

            }
            throw new ApplicationException($"Access the Provider {prov.Nombre} fail with: {content}");
        }

        public async Task PayOrderAsync(string orderProviderId)
        {
            // Get the provider by Name
            ProveedorSetting? prov = cconfig.ProveedoresSettings.FirstOrDefault(x => x.Nombre == ProviderName) ?? throw new ApplicationException($"Provider unknow for {orderProviderId} ");
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
            ProveedorSetting? prov = cconfig.ProveedoresSettings.FirstOrDefault(x => x.Nombre == ProviderName) ?? throw new ApplicationException($"Provider unknow for {orderProviderId} ");
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
