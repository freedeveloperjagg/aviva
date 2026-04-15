using AvivaLibrary.Models;
using AvivaLibrary.Models.Responses;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;

namespace AvivaUI.Components.Layout.Dialogs
{
    public partial class OrderDetailsDialog
    {
        [Parameter]
        public OrderResponse order { get; set; } = new();

         protected override void OnParametersSet()
        {
            // Ensure child collections are not null
            order.Products ??= [];
            order.Fees ??= [];
        }

        private void Close()
        {
            dialogService.Close();
        }

        private int ProductCount(List<Product> products)
        {
            return products.Count;
        }

        private bool IsPaid(string status)
        {
            return status.ToLowerInvariant() == "paid";
        }
    }
}
