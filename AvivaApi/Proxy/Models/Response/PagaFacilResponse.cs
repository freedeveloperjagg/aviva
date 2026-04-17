using AvivaLibrary.Models;

namespace AvivaApi.Proxy.Models.Response
{
    /// <summary>
    /// Here is the object retrieved from the provider,
    /// it should be rearange to get the values necessaries.
    /// </summary>
    public class PagaFacilResponse
    {
        public string OrderId { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Method { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public List<PagaFacilFee> Fees { get; set; } = [];
        public List<Product> Products { get; set; } = [];

        public OrderCreated ConvertPago(string providerName, OrderPago order)
        {
            OrderCreated orderCreated = new ()
            {
                Amount = this.Amount,
                Method = ConvertMethod(this.Method),
                ProviderOrderId = this.OrderId,
                DateCreated = DateTime.Now,
                Fees = ConvertListOfFees(this.Fees),
                Products = ConvertListOfProducts(this.Products, order.Products),
                Status = ConvertStatus(this.Status),
                ProviderName = providerName
            };
            return orderCreated;
        }

        private string ConvertStatus(string status)
        {
            return status.ToUpperInvariant();
        }

        private static List<Product> ConvertListOfProducts(List<Product> pfProds, List<Product> avProds)
        {
            List<Product> prods = [];
            foreach (var internalProd in avProds)
            {
                var pProveedor = pfProds.SingleOrDefault(x => x.Name.Trim() == internalProd.Name.Trim()) ?? throw new ApplicationException($"The product {internalProd.Name} does not exists in the original order");
                Product composeProd = new()
                {
                    UnitPrice = pProveedor.UnitPrice,
                    Name = internalProd.Name,
                    Id = internalProd.Id,
                    Status = internalProd.Status,
                    Details = internalProd.Details
                };
                prods.Add(composeProd);
            }
            return prods;
        }

        private static List<Fee> ConvertListOfFees(List<PagaFacilFee> pffees)
        {
            List<Fee> avFees = [];
            foreach (var fee in pffees)
            {
                avFees.Add(new Fee() { Amount = fee.Amount, Name = fee.Name });
            }

            return avFees;
        }

        /// <summary>
        /// Conver the method of payment string to internal aviva
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        private static string ConvertMethod(string method)
        {
            return method.ToUpperInvariant();
        }
    }
}

