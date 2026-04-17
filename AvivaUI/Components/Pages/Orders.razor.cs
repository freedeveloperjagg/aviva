using AvivaLibrary.Models.Responses;
using AvivaUI.Components.Layout.Dialogs;
using AvivaUI.Services;
using Radzen;

namespace AvivaUI.Components.Pages;

public partial class Orders(IOrderPagoServices xpagoService)
{
    private List<OrderResponse> orders = [];
    private OrderResponse? selectedOrder;
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

        await dialogService.OpenAsync<OrderDetailsDialog>($"Order #{order.Id}",
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
        try
        {

            if (order.Status == "Paid") return;

            bool? confirmed = await dialogService.Confirm(
                $"Mark Order #{order.Id} — {order.ProviderName} — as paid?",
                "Confirm Payment",
                new ConfirmOptions
                {
                    OkButtonText = "Yes, pay now",
                    CancelButtonText = "Cancel"
                });

            if (confirmed == true)
            {
                // Add here the payment call to API
                await pagoServices.PaidOrder(order.Id);

                // Refresh the orders


                order.Status = "Paid";

                notificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Success,
                    Summary = "Payment recorded",
                    Detail = $"Order #{order.Id} has been marked as paid.",
                    Duration = 3000
                });

                StateHasChanged();
            }
        }
        catch (Exception ex)
        {
            notificationService.Notify(new NotificationMessage
            {
                Severity = NotificationSeverity.Error,
                Summary = "Paid Unsuccesful",
                Detail = $"Order #{order.Id} was not paid: {ex.Message}",
                Duration = 113000
            });
        }
    }

    private async Task CancelPayment(OrderResponse order)
    {
        try
        {

            if (order.Status == "CANCELLED") return;

            bool? confirmed = await dialogService.Confirm(
                $"Mark Order #{order.Id} — {order.ProviderName} — as Cancelled?",
                "Confirm Cancellation",
                new ConfirmOptions
                {
                    OkButtonText = "Yes, Cancel now",
                    CancelButtonText = "Cancel"
                });

            if (confirmed == true)
            {
                // Call cancellation Service.................
                await pagoServices.CancelOrder(order.Id);

                // This allow to a response is screen directly.
                order.Status = "CANCELLED";

                notificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Success,
                    Summary = "Cancellation success",
                    Detail = $"Order #{order.Id} has been marked as cancelled.",
                    Duration = 3000
                });

                StateHasChanged();
            }

        }
        catch (Exception ex)
        {
            notificationService.Notify(new NotificationMessage
            {
                Severity = NotificationSeverity.Error,
                Summary = "Cancellation Fail",
                Detail = $"Order #{order.Id} was not marked as cancelled: {ex.Message}",
                Duration = 53000
            });
        }
    }

    /// <summary>
    /// Check if it is pay or it is cancelled.
    /// </summary>
    /// <param name="status"></param>
    /// <returns></returns>
    private static bool CheckIsPaidorCancelled(string status)
    {
        bool active = status.Equals("PAID", StringComparison.InvariantCultureIgnoreCase)
            || status.Equals("CANCELLED", StringComparison.InvariantCultureIgnoreCase);
        return active;
    }
       

    //private static bool CheckIsCancelled(string status)
    //{
    //    return status.Equals("CANCELLED", StringComparison.CurrentCultureIgnoreCase);
    //}
}
