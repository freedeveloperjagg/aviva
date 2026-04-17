
using System.ComponentModel.DataAnnotations;

namespace AvivaLibrary.Models
{
    public class OrderCreated
    {
        /// <summary>
        /// Internal Id
        /// </summary>
        [Required]
        public int Id { get; set; }
        
        /// <summary>
        /// Provider Order Id 
        /// </summary>
        public string ProviderOrderId { get; set; } = string.Empty;

        /// <summary>
        /// This name should be added by the API to know from  
        /// what provider the order comes.
        /// </summary>
        public string ProviderName { get; set; } = string.Empty;

        /// <summary>
        /// Amount order
        /// </summary>
        public decimal Amount { get; set; }
        
        /// <summary>
        /// Method of payment can be CASH / CREDIT / TRANSFER.
        /// </summary>
        public string Method { get; set; } = string.Empty;
        
        /// <summary>
        /// Status of the order , PAID, CANCELLED, Etc.
        /// </summary>
        public string Status { get; set; } = string.Empty;
        
        /// <summary>
        /// Fees inside the order
        /// </summary>
        public List<Fee> Fees { get; set; } = [];
        
        /// <summary>
        /// Poduct inside the 
        /// </summary>
        public List<Product> Products { get; set; } = [];

        /// <summary>
        /// Creating Date. to add locally
        /// </summary>
        public DateTime DateCreated { get; set;}

        /// <summary>
        /// Creating Date. to add locally
        /// </summary>
        public DateTime UpdateCreated { get; set; }

        /// <summary>
        /// Who created to order, not to fill now
        /// </summary>
        public string CreatedBy { get; set; } = string.Empty;

    }
}
