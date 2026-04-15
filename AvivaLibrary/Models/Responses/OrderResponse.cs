namespace AvivaLibrary.Models.Responses
{
    /// <summary>
    /// {
    ///  "orderId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    ///  "amount": 0,
    ///  "status": "None",
    ///  "method": "None",
    ///  "fees": [
    ///    {
    ///      "name": "string",
    ///      "amount": 0
    ///    }
    ///  ],
    ///  "products": [
    ///    {
    ///      "name": "string",
    ///      "unitPrice": 0
    ///    }
    ///  ],
    ///  "createdDate": "2026-04-15T04:04:14.616Z",
    ///  "createdBy": "string"
    ///}
    /// </summary>
    public class OrderResponse
    {
        public string OrderId { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Method { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public List<Fee> Fees { get; set; } =[];
        public List<Product> Products { get; set; } = [];        

        /// <summary>
        /// This name should be added by the API to know from  
        /// what provider the order comes.
        /// </summary>
        public string ProviderName { get; set;} = string.Empty;
    }
}
