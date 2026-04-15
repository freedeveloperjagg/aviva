using AvivaLibrary.Models;
using AvivaLibrary.Models.Responses;
using AvivaUI.Components.Layout.Dialogs;
using AvivaUI.Services;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace AvivaUI.Components.Pages;

public partial class Orders(IOrderPagoServices xpagoService)
{
    //[Inject] private NotificationService NotificationService { get; set; } = default!;
    //[Inject] private DialogService DialogService { get; set; } = default!;

    private List<OrderResponse> orders = new();
    private OrderResponse? selectedOrder;
    //private bool _showDetails;
    readonly IOrderPagoServices pagoServices = xpagoService;

    protected override async Task OnInitializedAsync()
    {
        // TODO: Replace with real service call e.g. await OrderService.GetAllAsync()
        try
        {
            orders = await pagoServices.GetAllOrdersFromProviders();
        }
        catch (Exception ex)
        {
            notificationService.Notify(new NotificationMessage
            {
                Severity = NotificationSeverity.Error,
                CloseOnClick = true,
                Summary = "Get Ordersreturn exception",
                Detail = $"Something happen: {ex.Message}.",
                Duration = 10000
            });
        }     
 
    }

    // ── Show Details dialog ──────────────────────────────────────────────────
    private async Task ShowDetails(OrderResponse order)
    {
        selectedOrder = order;

        await dialogService.OpenAsync<OrderDetailsDialog>($"Order #{order.OrderId}",
                new Dictionary<string, object?> { { "order", order } },
                new DialogOptions
                {
                    Width = "800px",
                    CloseDialogOnEsc = true,
                    ShowClose = true,
                    Resizable = true,
                    
                });
    }

    // ── Confirm Payment dialog ───────────────────────────────────────────────
    private async Task ConfirmPayment(OrderResponse order)
    {
        if (order.Status == "Paid") return;

        bool? confirmed = await dialogService.Confirm(
            $"Mark Order #{order.OrderId} — {order.ProviderName} — as paid?",
            "Confirm Payment",
            new ConfirmOptions
            {
                OkButtonText = "Yes, pay now",
                CancelButtonText = "Cancel"
            });

        if (confirmed == true)
        {
            order.Status = "Paid";

            notificationService.Notify(new NotificationMessage
            {
                Severity = NotificationSeverity.Success,
                Summary = "Payment recorded",
                Detail = $"Order #{order.OrderId} has been marked as paid.",
                Duration = 3000
            });

            StateHasChanged();
        }
    }

    private bool CheckIsPaid(string status)
    {
        return status.ToLowerInvariant() == "paid";

    }
}
