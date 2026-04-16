using AvivaLibrary.Models;

namespace AvivaApi.Proxy.Models
{
    public class PagaFacilResponse
    {
        public string OrderId { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Method { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public List<Fee> Fees { get; set; } = [];
        public List<Product> Products { get; set; } = [];

        public OrderCreated ConvertPago(string providerName)
        {
            OrderCreated orderCreated = new OrderCreated()
            {
                Amount = this.Amount,
                Method = this.Method,
                ProviderOrderId = this.OrderId,
                DateCreated = DateTime.Now,
                Fees = this.Fees,
                Products = this.Products,
                Status = this.Status,
                ProviderName = providerName
            };
            return orderCreated;
        }
    }
}

