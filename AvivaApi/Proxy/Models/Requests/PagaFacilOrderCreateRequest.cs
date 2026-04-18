using AvivaLibrary.Models;

namespace AvivaApi.Proxy.Models.Requests
{
    /// <summary>
    /// Specific class to go to Create Payment in Paga Facil
    /// </summary>
    public class PagaFacilOrderCreateRequest
    {
        public string Method { get; set; } = string.Empty;
        public List<PagaFacilProduct> Products { get; set; } = [];

        public static PagaFacilOrderCreateRequest Factory(OrderPago order)
        {

            PagaFacilOrderCreateRequest req = new()
            {
                Method = ConvertInputMethod(order.Method),
                Products = ConvertInputProducts(order.Products)
            };

            return req;
        }

        private static List<PagaFacilProduct> ConvertInputProducts(List<Product> products)
        {
            List<PagaFacilProduct> pfproducts = [];
            foreach (Product product in products)
            {
                PagaFacilProduct p = new()
                {
                    Name = product.Name,
                    UnitPrice = product.UnitPrice,
                };
                pfproducts.Add(p);
            }

            return pfproducts;
        }

        private static string ConvertInputMethod(string method)
        {
            string met = method.ToUpperInvariant();
            switch (met)
            {
                case "CASH":
                    {
                        return "Cash";
                    }
                case "CREDIT":
                    {
                        return "Card";
                    }
                //case "TRANSFER":
                //    {
                //        return "Transfer";
                //    }
                default:
                    {
                        throw new ArgumentException($"Payment Method: {met} not supported by Paga Facil");
                    }
            }
        }
    }
}
