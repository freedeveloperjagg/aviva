namespace AvivaLibrary.Models
{
    public class Rule
    {
        public int Id { get; set; }
        public string TipoPago { get; set; } = "CC"; // CC, Cash, TRANSFER, etc.
        public List<Limit> Limites { get; set; } = [];

        public int EntidadDePagoId { get; set; } // Foreign Key

    }
}
