namespace AvivaLibrary.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Details { get; set; } = string.Empty;
        public string Status { get; set; } = "Active";
        public decimal UnitPrice { get; set; }
    }

}
