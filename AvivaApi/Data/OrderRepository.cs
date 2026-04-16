using AvivaLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace AvivaApi.Data
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AvivaDbContext _context;

        public OrderRepository(AvivaDbContext context)
        {
            _context = context;
        }

        // INSERT: Returns the new ID
        public async Task<int> InsertAsync(OrderCreated order)
        {
            // Link to existing products in DB
            var productIds = order.Products.Select(p => p.Id).ToList();
            order.Products = await _context.Products
                .Where(p => productIds.Contains(p.Id))
                .ToListAsync();

            // Ensure Fees are treated as new entries
            foreach (var fee in order.Fees) { fee.Id = 0; }

            _context.OrdersCreated.Add(order);
            await _context.SaveChangesAsync();
            return order.Id;
        }

        // UPDATE: Full update based on ID
        public async Task UpdateAsync(int id, OrderCreated updatedOrder)
        {
            var existing = await _context.OrdersCreated
                .Include(o => o.Fees)
                .Include(o => o.Products)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (existing == null) throw new KeyNotFoundException($"Order {id} not found.");

            // Update scalars
            existing.Amount = updatedOrder.Amount;
            existing.Method = updatedOrder.Method;
            existing.Status = updatedOrder.Status;
            existing.ProviderOrderId = updatedOrder.ProviderOrderId;
            existing.ProviderName = updatedOrder.ProviderName;
            existing.UpdateCreated = DateTime.UtcNow;

            // Sync Fees (Delete old, add new)
            _context.Fees.RemoveRange(existing.Fees);
            foreach (var fee in updatedOrder.Fees)
            {
                fee.Id = 0;
                existing.Fees.Add(fee);
            }

            // Sync Products (Many-to-Many)
            existing.Products.Clear();
            var productIds = updatedOrder.Products.Select(p => p.Id).ToList();
            existing.Products = await _context.Products.Where(p => productIds.Contains(p.Id)).ToListAsync();

            await _context.SaveChangesAsync();
        }

        // RETRIEVE BY ID
        public async Task<OrderCreated?> GetByIdAsync(int id)
        {
            return await _context.OrdersCreated
                .Include(o => o.Fees)
                .Include(o => o.Products)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        // RETRIEVE ALL
        public async Task<List<OrderCreated>> GetAllAsync()
        {
            return await _context.OrdersCreated
                .Include(o => o.Fees)
                .Include(o => o.Products)
                .ToListAsync();
        }

        // RETRIEVE BY PROVIDER
        public async Task<IEnumerable<OrderCreated>> GetByProviderAsync(string providerName)
        {
            return await _context.OrdersCreated
                .Include(o => o.Fees)
                .Include(o => o.Products)
                .Where(o => o.ProviderName == providerName)
                .ToListAsync();
        }

        // CANCEL
        public async Task CancelAsync(int id)
        {
            await UpdateStatusAsync(id, "CANCELLED");
        }

        // PAID
        public async Task PaidAsync(int id)
        {
            await UpdateStatusAsync(id, "PAID");
        }

        // UPDATE STATUS ONLY
        public async Task UpdateStatusAsync(int id, string newStatus)
        {
            var order = await _context.OrdersCreated.FindAsync(id);
            if (order != null)
            {
                order.Status = newStatus;
                order.UpdateCreated = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }
    }
}
