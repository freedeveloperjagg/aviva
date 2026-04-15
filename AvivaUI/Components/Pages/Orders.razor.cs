using AvivaLibrary.Models;
using AvivaUI.Components.Layout.Dialogs;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace AvivaUI.Components.Pages;

public partial class Orders
{
    [Inject] private NotificationService NotificationService { get; set; } = default!;
    [Inject] private DialogService DialogService { get; set; } = default!;

    private List<Order> _orders = new();
    private Order? _selectedOrder;
    private bool _showDetails;

    protected override void OnInitialized()
    {
        // TODO: Replace with real service call e.g. await OrderService.GetAllAsync()
        _orders = new List<Order>
        {
            new() { OrderId = 1001, CreatedDate = new DateTime(2026, 1, 5),  Name = "Office supplies bundle",    Amount = 213.94m, ItemsCount = 4,  Paid = true  },
            new() { OrderId = 1002, CreatedDate = new DateTime(2026, 1, 12), Name = "Developer workstation kit", Amount = 1862.99m,ItemsCount = 3,  Paid = true  },
            new() { OrderId = 1003, CreatedDate = new DateTime(2026, 1, 20), Name = "Accessories pack",          Amount = 94.93m,  ItemsCount = 3,  Paid = false },
            new() { OrderId = 1004, CreatedDate = new DateTime(2026, 2, 3),  Name = "Monitor + stand combo",     Amount = 493.00m, ItemsCount = 2,  Paid = true  },
            new() { OrderId = 1005, CreatedDate = new DateTime(2026, 2, 14), Name = "Peripherals order",         Amount = 154.49m, ItemsCount = 3,  Paid = false },
            new() { OrderId = 1006, CreatedDate = new DateTime(2026, 2, 28), Name = "Remote work kit",           Amount = 343.94m, ItemsCount = 5,  Paid = false },
            new() { OrderId = 1007, CreatedDate = new DateTime(2026, 3, 7),  Name = "Laptop accessories",        Amount = 83.99m,  ItemsCount = 2,  Paid = true  },
            new() { OrderId = 1008, CreatedDate = new DateTime(2026, 3, 15), Name = "Full desk setup",           Amount = 1612.94m,ItemsCount = 6,  Paid = false },
            new() { OrderId = 1009, CreatedDate = new DateTime(2026, 3, 22), Name = "Cable management kit",      Amount = 44.94m,  ItemsCount = 3,  Paid = true  },
            new() { OrderId = 1010, CreatedDate = new DateTime(2026, 4, 1),  Name = "Audio setup",               Amount = 268.50m, ItemsCount = 2,  Paid = false },
        };
    }

    // ── Show Details dialog ──────────────────────────────────────────────────
    private async Task ShowDetails(Order order)
    {
        _selectedOrder = order;

        await DialogService.OpenAsync<OrderDetailsDialog>($"Order #{order.OrderId}",
                new Dictionary<string, object?> { { "order", order } },
                new DialogOptions
                {
                    Width = "420px",
                    CloseDialogOnEsc = true,
                    ShowClose = true
                });
    }

    // ── Confirm Payment dialog ───────────────────────────────────────────────
    private async Task ConfirmPayment(Order order)
    {
        if (order.Paid) return;

        bool? confirmed = await DialogService.Confirm(
            $"Mark Order #{order.OrderId} — {order.Name} — as paid?",
            "Confirm Payment",
            new ConfirmOptions
            {
                OkButtonText = "Yes, pay now",
                CancelButtonText = "Cancel"
            });

        if (confirmed == true)
        {
            order.Paid = true;

            NotificationService.Notify(new NotificationMessage
            {
                Severity = NotificationSeverity.Success,
                Summary = "Payment recorded",
                Detail = $"Order #{order.OrderId} has been marked as paid.",
                Duration = 3000
            });

            StateHasChanged();
        }
    }
}
