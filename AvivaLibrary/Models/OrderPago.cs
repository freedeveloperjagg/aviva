
namespace AvivaLibrary.Models
{
    public class OrderPago
    {
        public int Id { get; set; } 
        public  string Method { get; set; } = "cash";
        public List<Product> Products { get; set; } = [];
        public bool Success { get; set; }
    }
}
