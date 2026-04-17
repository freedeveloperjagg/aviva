using AvivaLibrary.Models;
using Microsoft.EntityFrameworkCore;
namespace AvivaApi.Data
{
    public class AvivaDbContext(DbContextOptions<AvivaDbContext> options) : DbContext(options)
    {
        // Products
        public DbSet<Product> Products => Set<Product>();

        // Orders Created storage
        public DbSet<OrderCreated> OrdersCreated { get; set; }
        public DbSet<Fee> Fees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Product definitions..................
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(p => p.Id);

                entity.Property(p => p.Name)
                      .IsRequired()
                      .HasMaxLength(200);

                entity.Property(p => p.Details)
                      .HasMaxLength(500);

                entity.Property(p => p.Status)
                      .HasDefaultValue("Available");

                entity.Property(p => p.UnitPrice)
                      .HasPrecision(18, 2);
            });

            // ------- OrderCreated ---------------------------
            modelBuilder.Entity<OrderCreated>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Amount).HasPrecision(18, 2);
                entity.Property(e => e.ProviderOrderId).IsRequired();

                // 1. One-to-Many with Fees (Fees ARE deleted with the Order)
                entity.HasMany(e => e.Fees)
                      .WithOne()
                      .OnDelete(DeleteBehavior.Cascade);

                // 2. Many-to-Many with Products (Products ARE NOT deleted)
                entity.HasMany(p => p.Products)
                      .WithMany() // No navigation property back in Product class
                      .UsingEntity<Dictionary<string, object>>(
                          "OrderProductJoin", // Name of the Join Table in memory
                          j => j.HasOne<Product>().WithMany().OnDelete(DeleteBehavior.Restrict),
                          j => j.HasOne<OrderCreated>().WithMany().OnDelete(DeleteBehavior.Cascade));
            });

            // ------- Fees ---------------------------
            modelBuilder.Entity<Fee>(entity =>
            {
                entity.HasKey(f => f.Id);

                entity.Property(f => f.Name)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(f => f.Amount)
                      .HasPrecision(18, 2);
            });

        }

        public void Seed()
        {
            if (!Products.Any())
            {
                Products.AddRange(
                    new Product
                    {
                        Id = 1,
                        Name = "Laptop",
                        Details = "15-inch display, 16GB RAM",
                        UnitPrice = 1299.99m,
                        Status = "Not Available"

                    },
                    new Product
                    {
                        Id = 2,
                        Name = "Dell Inspiron 5050",
                        Details = "32GB /512 SSD / Intel i7",
                        UnitPrice = 2350.00m,
                        Status = "Available"

                    },
                    new Product
                    {

                        Id = 3,
                        Name = "Lenovo 3450",
                        Details = "16GB /512 SSD / Intel i5",
                        UnitPrice = 2350.00m,
                        Status = "Not Available"

                    },
                    new Product
                    {
                        Id = 4,
                        Name = "Head Set 450",
                        Details = "Logitech Low Noise",
                        UnitPrice = 350.00m,
                        Status = "Available"

                    },
                    new Product
                    {
                        Id = 5,
                        Name = "Desk Flexible",
                        Details = "Adjustable height, ergonomic design",
                        UnitPrice = 3500.00m,
                        Status = "Available"

                    },

                    new Product
                    {
                        Id = 6,
                        Name = "Wireless Mouse",
                        Details = "Ergonomic design",
                        UnitPrice = 29.99m,
                        Status = "Available"

                    },
                    new Product
                    {
                        Id = 7,
                        Name = "Mechanical Keyboard",
                        Details = "RGB backlight",
                        UnitPrice = 89.50m,
                        Status = "Available"
                    },
                       new Product
                       {
                           Id = 8,
                           Name = "Modulo Lunar Apollo 25",
                           Details = "With working toilet",
                           UnitPrice = 100089.50m,
                           Status = "Available"
                       },
                       new Product
                       {
                           Id = 9,
                           Name = "Aleko Model 1989",
                           Details = "With some mechanical problem",
                           UnitPrice = 300089.50m,
                           Status = "Available"
                       }
                );
                SaveChanges();
            }
        }

    }

}
