using AvivaLibrary.Models;
using AvivaLibrary.Models.Requests;
using AvivaLibrary.Models.Responses;
using System.Text;
using System.Text.Json;

namespace AvivaApi.Proxy
{
    public class PaymentProxy(
        IHttpClientFactory xfactory,
        CConfig xconfig
        ) : IPaymentProxy
    {
        private readonly IHttpClientFactory factory = xfactory;
        private readonly CConfig cconfig = xconfig;
        private readonly JsonSerializerOptions options = new()
        {
            PropertyNameCaseInsensitive = true,
        };

        public async Task<OrderResponse?> CreateOrderAsync(EntidadDePago provider, OrderPago orderPago)
        {
            // Create the payload
            string json = JsonSerializer.Serialize(orderPago);
            HttpContent payload = new StringContent(json, Encoding.UTF8, "application/json");


            HttpClient client = factory.CreateClient("Provider");

            // Get the Url for the service
            string url = string.Empty;
            foreach (var item in cconfig.ProveedoresSettings)
            {
                if (item.Nombre == provider.Nombre)
                {
                    url = item.Url;
                    payload.Headers.Add("x-api-key", item.Key);
                    break;
                }
            }

            if (url == string.Empty) { throw new ApplicationException($"Provider {provider.Nombre} not register"); }

            client.BaseAddress = new Uri(url);
            var result = await client.PostAsync("Order", payload);
            string content = await result.Content.ReadAsStringAsync();
            if (result.IsSuccessStatusCode)
            {
                OrderResponse? order = JsonSerializer.Deserialize<OrderResponse>(content, options);
                return order;
            }
            throw new ApplicationException($"Error reading the proxy in {url}, Error: {content}");
        }

        /// <summary>
        /// Get all Orders, this shall go thorught all provider to get the orders.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<OrderResponse>> GetOrdersAsync(ProveedorSetting prov)
        {
            HttpClient client = factory.CreateClient("Provider");
            client.DefaultRequestHeaders.Add("x-api-key", prov.Key);
            client.BaseAddress = new Uri(prov.Url);

            var httpResponse = await client.GetAsync("Order");
            string content = await httpResponse.Content.ReadAsStringAsync();
            if (httpResponse.IsSuccessStatusCode)
            {
                List<OrderResponse>? orders = JsonSerializer.Deserialize<List<OrderResponse>>(content,options);
                if (orders == null) return [];
                foreach (var order in orders) { order.ProviderName = prov.Nombre; }
                return orders;

            }
            throw new ApplicationException($"Access the Provider {prov.Nombre} fail with: {content}");
           
        }

        /// <summary>
        /// Get a order give nthe Id and the provider.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="provider"></param>
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>
        public async Task<OrderResponse?> GetOrderAsync(string id, string provider)
        {
            // Get the provider by Name
            ProveedorSetting? prov = cconfig.ProveedoresSettings.FirstOrDefault( x=> x.Nombre == provider);
            if (prov == null)
            {
                throw new ApplicationException($"Provider unknow {provider}");
            }
            
            HttpClient client = factory.CreateClient("Provider");
            client.DefaultRequestHeaders.Add("x-api-key", prov.Key);
            client.BaseAddress = new Uri(prov.Url);

            var httpResponse = await client.GetAsync($"Order/{id}");
            string content = await httpResponse.Content.ReadAsStringAsync();
            if (httpResponse.IsSuccessStatusCode)
            {
                OrderResponse? order = JsonSerializer.Deserialize<OrderResponse>(content, options);
                if (order == null) return null;
                order.ProviderName = prov.Nombre; 
                return order;

            }
            throw new ApplicationException($"Access the Provider {prov.Nombre} fail with: {content}");

        }


        public async Task PayOrderAsync(ChangeOrderRequest request)
        {
            // Get the provider by Name
            ProveedorSetting? prov = cconfig.ProveedoresSettings.FirstOrDefault(x => x.Nombre == request.ProveedorName);
            if (prov == null)
            {
                throw new ApplicationException($"Provider unknow {request.ProveedorName}");
            }

            HttpClient client = factory.CreateClient("Provider");
            client.DefaultRequestHeaders.Add("x-api-key", prov.Key);
            client.BaseAddress = new Uri(prov.Url);

            HttpResponseMessage httpResponse = await client.PutAsync($"Pay?id={request.IdOrder}", null);
            string content = await httpResponse.Content.ReadAsStringAsync();
            if (httpResponse.IsSuccessStatusCode)
            {
                return;
            }

            throw new ApplicationException($"Access to Pay in the Provider {prov.Nombre} fail with:{httpResponse.ReasonPhrase} {content}");

        }

        /// <summary>
        /// This cancel the order in the selected provider
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task CancelOrderAsync(ChangeOrderRequest request)
        {
            // Get the provider by Name
            ProveedorSetting? prov = cconfig.ProveedoresSettings.FirstOrDefault(x => x.Nombre == request.ProveedorName);
            if (prov == null)
            {
                throw new ApplicationException($"Provider unknow {request.ProveedorName}");
            }

            HttpClient client = factory.CreateClient("Provider");
            client.DefaultRequestHeaders.Add("x-api-key", prov.Key);
            client.BaseAddress = new Uri(prov.Url);
            
            HttpResponseMessage httpResponse = await client.PutAsync($"Cancel?id={request.IdOrder}",null);
            string content = await httpResponse.Content.ReadAsStringAsync();
            if (httpResponse.IsSuccessStatusCode)
            {
                return;
            }

            throw new ApplicationException($"Access to Cancel in the Provider {prov.Nombre} fail with:{httpResponse.ReasonPhrase} {content}");
        }
    }
}
