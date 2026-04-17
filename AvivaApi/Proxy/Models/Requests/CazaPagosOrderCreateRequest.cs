using AvivaLibrary.Models;

namespace AvivaApi.Proxy.Models.Requests
{
    public class CazaPagosOrderPagoRequest
    {
        public string Method { get; set; } = string.Empty;
        public List<CazaPagosProduct> Products { get; set; } = [];


        public static CazaPagosOrderPagoRequest Factory(OrderPago order)
        {

            CazaPagosOrderPagoRequest req = new()
            {
                Method = ConvertInputMethod(order.Method),
                Products = ConvertInputProducts(order.Products)
            };

            return req;
        }

        private static List<CazaPagosProduct> ConvertInputProducts(List<Product> products)
        {
            List<CazaPagosProduct> cpproducts = [];
            foreach (Product product in products)
            {
                CazaPagosProduct p = new()
                {
                    Name = product.Name,
                    UnitPrice = product.UnitPrice,
                };
                cpproducts.Add(p);
            }

            return cpproducts;
        }

        private static string ConvertInputMethod(string method)
        {
            string met = method.ToUpperInvariant();
            switch (met)
            {
                case "CREDIT":
                    {
                        return "CreditCard";
                    }
                case "TRANSFER":
                    {
                        return "Transfer";
                    }
                default:
                    {
                        throw new ArgumentException($"Payment Method: {met} not supported by Caza Pagos");
                    }
            }
        }
    }
}
