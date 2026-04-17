namespace AvivaApi.Proxy.Models
{
    /// <summary>
    /// Class specific for Paga Facil
    /// in this case has the same names that our product.
    /// </summary>
    public class CazaPagosProduct
    {
        public string Name { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
    }
}
