
namespace AvivaLibrary.Models.Requests
{
    public class ChangeOrderRequest
    {
        /// <summary>
        /// Id Order to cancel
        /// </summary>
        public string IdOrder { get; set; } = string.Empty;

        /// <summary>
        /// Proveedor where the order was created
        /// </summary>
        public string ProveedorName { get; set; } = string.Empty;
    }
}
