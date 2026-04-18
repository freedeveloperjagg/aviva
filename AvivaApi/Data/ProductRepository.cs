using AvivaLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace AvivaApi.Data
{
    public class ProductRepository
    {
        private readonly AvivaDbContext db;

        public ProductRepository(AvivaDbContext xdb)
        {
            db = xdb;
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await db.Products.ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await db.Products.FindAsync(id);
        }

        public async Task<Product> AddAsync(Product product)
        {
            db.Products.Add(product);
            await db.SaveChangesAsync();
            return product;
        }

        public async Task<Product?> UpdateAsync(Product product)
        {
            var existing = await db.Products.FindAsync(product.Id);
            if (existing == null)
                return null;

            existing.Name = product.Name;
            existing.Details = product.Details;
            existing.Status = product.Status;
            existing.UnitPrice = product.UnitPrice;

            await db.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = await db.Products.FindAsync(id);
            if (product == null)
                return false;

            db.Products.Remove(product);
            await db.SaveChangesAsync();
            return true;
        }
    }

}
