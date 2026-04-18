using AvivaLibrary.Models;

namespace AvivaApi.Proxy.Models.Response
{
    public class CazaPagosResponse
    {
        public string OrderId { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Method { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public List<CazaPagosFee> Fees { get; set; } = [];
        public List<CazaPagosProduct> Products { get; set; } = [];
        public List<CazaPagosTaxes> Taxes { get; set; } = [];
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; } = string.Empty;

        public OrderCreated ConvertPago(string providerName, OrderPago order)
        {
            OrderCreated orderCreated = new()
            {
                Amount = this.Amount,
                Method = ConvertMethod(this.Method),
                ProviderOrderId = this.OrderId,
                DateCreated = this.CreatedDate,
                Fees = ConvertListOfFees(this.Fees),
                Products = ConvertListOfProducts(this.Products, order.Products),
                Status = ConvertStatus(this.Status),
                ProviderName = providerName
            };
            orderCreated.Fees.AddRange(ConvertListOfTaxes(this.Taxes));
            return orderCreated;
        }

        private static string ConvertStatus(string status)
        {
            return status.ToUpperInvariant();
        }

        private static List<Fee> ConvertListOfTaxes(List<CazaPagosTaxes> taxes)
        {
            List<Fee> avFees = [];
            foreach (var fee in taxes)
            {
                avFees.Add(new Fee() { Amount = fee.Amount, Name = fee.Tax });
            }

            return avFees;
        }

        /// <summary>
        /// Convert the provider list of product to your products object
        /// </summary>
        /// <param name="czProd"></param>
        /// <param name="avProd"></param>
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>
        private static List<Product> ConvertListOfProducts(List<CazaPagosProduct> czProd, List<Product> avProd)
        {
            List<Product> prods = [];
            foreach (var internalProd in avProd)
            {
                var pProveedor = czProd.SingleOrDefault(x => x.Name.Trim() == internalProd.Name.Trim())
                        ?? throw new ApplicationException($"The product {internalProd.Name} does not exists in the original order");
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

        /// <summary>
        /// Convert the list of Fee of ypur proveedor in a list of fee of your products
        /// In this case there is nothing to do.
        /// </summary>
        /// <param name="cpfees"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private static List<Fee> ConvertListOfFees(List<CazaPagosFee> cpfees)
        {
            List<Fee> avFees = [];
            foreach (var fee in cpfees)
            {
                avFees.Add(new Fee() { Amount = fee.Amount, Name = fee.Title });
            }

            return avFees;
        }

        /// <summary>
        /// Use this to convert the internal method in the target
        /// with the standard method in the application
        /// credit,cash,transfer
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        private static string ConvertMethod(string method)
        {
            switch (method.ToLowerInvariant())
            {
                case "creditcard":
                    {
                        return "CREDIT";
                    }
                default:
                    {
                        return method.ToUpperInvariant();
                    }
            }
        }
    }
}
