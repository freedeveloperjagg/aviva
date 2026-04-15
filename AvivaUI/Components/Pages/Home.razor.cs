using AvivaLibrary.Models;
using AvivaUI.Components.Layout.Dialogs;
using AvivaUI.Services;
using Radzen;
using Radzen.Blazor;

namespace AvivaUI.Components.Pages
{
    public partial class Home(
        IProductServices productService,
        IOrderPagoServices xpagoService)
    {
        readonly IProductServices pservices = productService;
        readonly IOrderPagoServices pagoService = xpagoService;
        public List<Product> products = new();
        public RadzenDataGrid<Product> grid = default!;
        private IList<Product>? selectedProducts = new List<Product>();

        private bool HasSelection => selectedProducts?.Count > 0;

        /// <summary>
        /// Initialization also here the program read the available products.
        /// </summary>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {
            // TODO: Replace with real service call e.g. await ProductService.GetAllAsync()
            base.OnInitialized();
            try
            {
                products = await pservices.GetAllProductsAsync();

            }
            catch (Exception ex)
            {
                notificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    CloseOnClick = true,
                    Summary = "Get Products",
                    Detail = $"Error getting the products  {ex.Message}.",
                    Duration = 15000
                });
            }          
    
        }

        private void OnRowSelect(Product row) => StateHasChanged();
        private void OnRowDeselect(Product row) => StateHasChanged();

        private void ToggleRow(Product row, bool selected)
        {
            selectedProducts ??= new List<Product>();
            if (selected) { if (!selectedProducts.Contains(row)) selectedProducts.Add(row); }
            else { selectedProducts.Remove(row); }
            StateHasChanged();
        }

        private void SelectAll(bool selectAll)
        {
            selectedProducts = selectAll ? new List<Product>(products) : new List<Product>();
            StateHasChanged();
        }

        private async Task CreateOrderAsync()
        {
            if (selectedProducts == null || selectedProducts.Count == 0) return;

            try
            {
                // Prepare the dialog to be open in mode Creation
                OrderPago order = new()
                {
                    Products = selectedProducts.ToList()
                };

                var parameters = new Dictionary<string, object?>
                {
                   {"orderPago", order  },
                   {"operation","create" }
                };

                var options = new DialogOptions()
                {
                    Width = "800px",
                    Resizable = true,
                    Draggable = true,
                    CloseDialogOnEsc = true,
                    CloseDialogOnOverlayClick = true,
                    ShowClose = true,
                };

                var result = await dialogService.OpenAsync<CreateOrderDialog>("Confirm Order", parameters, options);
                if (result == null)
                    return; // User cancelled the dialog                            

                if (result.Success)
                {
                    // Send to Api the order creation
                    OrderPago op = (OrderPago)result;
                    var resu = await pagoService.SendOrderPago(op);
                    if (resu != null)
                    {
                        // Show Notification with the result of the operation
                        notificationService.Notify(new NotificationMessage
                        {
                            Severity = NotificationSeverity.Success,
                            CloseOnClick = true,
                            Summary = "Order Created",
                            Detail = $"Order {resu.OrderId} to be pay in {resu.Method}, Amount {resu.Amount} created successfully.",
                            Duration = 5000
                        });
                    }
                    else
                    {
                        notificationService.Notify(new NotificationMessage
                        {
                            Severity = NotificationSeverity.Warning,
                            CloseOnClick = true,
                            Summary = "Order Return NULL",
                            Detail = $"Something happen. Check Order Page to see if your order was created successfully.",
                            Duration = 5000
                        });
                    }

                }

            }
            catch (Exception ex)
            {
                notificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Summary = "Exception Creating Order",
                    Detail = ex.Message,
                    Duration = 5000
                });
            }









            //// TODO: Navigate to order creation page passing selected IDs
            //NavManager.NavigateTo($"/createorder?ids={string.Join(",", selectedProducts.Select(p => p.Id))}");

            //NotificationService.Notify(new NotificationMessage
            //{
            //    Severity = NotificationSeverity.Success,
            //    Summary = "Order initiated",
            //    Detail = $"{selectedProducts.Count} product(s) added to order.",
            //    Duration = 3000
            //});
        }

        private static BadgeStyle GetBadgeStyle(string status) => status switch
        {
            "Active" => BadgeStyle.Success,
            "Inactive" => BadgeStyle.Secondary,
            "Pending" => BadgeStyle.Warning,
            _ => BadgeStyle.Light
        };
    }
}


