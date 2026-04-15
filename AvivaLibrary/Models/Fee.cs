namespace AvivaLibrary.Models
{
    public class Fee
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Amount { get; set; } = 0;
    }
}
