
namespace AvivaLibrary.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public int ItemsCount { get; set; }//
        public string Name { get; set; } = string.Empty;//
        public decimal Amount { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public List<Fee> Fees { get; set; } = new List<Fee>();
        public List<Product> Products { get; set; } = new List<Product>(); 
        public string Status { get; set; } = string.Empty;
        public bool Paid { get; set; }



    }
}
