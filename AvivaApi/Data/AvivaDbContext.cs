using AvivaLibrary.Models;
using AvivaLibrary.Models.Responses;
using Microsoft.EntityFrameworkCore;

public class AvivaDbContext : DbContext
{
    // Products
    public DbSet<Product> Products => Set<Product>();

    // Companies for Payment ............................................
    public DbSet<EntidadDePago> EntidadesDePago { get; set; }
    public DbSet<Rule> Rules { get; set; }
    public DbSet<Limit> Limites { get; set; }

    public DbSet<OrderCreated> OrdersCreated { get; set;  }

    public DbSet<Fee> Fees { get; set; }

    public AvivaDbContext(DbContextOptions<AvivaDbContext> options)
        : base(options)
    {
    }

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


        // ── EntidadDePago ────────────────────────────────────────────
        modelBuilder.Entity<EntidadDePago>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Nombre)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.HasMany(e => e.Rules)
                  .WithOne()
                  .HasForeignKey(r => r.EntidadDePagoId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // ── Rule ─────────────────────────────────────────────────────
        modelBuilder.Entity<Rule>(entity =>
        {
            entity.HasKey(r => r.Id);

            entity.Property(r => r.TipoPago)
                  .IsRequired()
                  .HasMaxLength(20);

            entity.HasMany(r => r.Limites)
                  .WithOne()
                  .HasForeignKey(l => l.RuleId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // ── Limit ─────────────────────────────────────────────────────
        modelBuilder.Entity<Limit>(entity =>
        {
            entity.HasKey(l => l.Id);

            entity.Property(l => l.Charge)
                  .HasColumnType("decimal(18,2)");

            // char is not natively supported in SQLite — store as string(1)
            entity.Property(l => l.Type)
                  .HasConversion<string>()
                  .HasMaxLength(1);
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
                }
            );

            // Seed the payment tables
            if (!EntidadesDePago.Any())
            {
                EntidadesDePago.AddRange(
                      new EntidadDePago { Id = 1, Nombre = "PagaFacil" },
                      new EntidadDePago { Id = 2, Nombre = "CazaPagos" }
                    );
            }

            if (!Rules.Any())
            {
                Rules.AddRange(
                new Rule { Id = 1, TipoPago = "CC", EntidadDePagoId = 1 },
                new Rule { Id = 2, TipoPago = "Cash", EntidadDePagoId = 1 },
                new Rule { Id = 3, TipoPago = "CC", EntidadDePagoId = 2 },
                new Rule { Id = 4, TipoPago = "Transfer", EntidadDePagoId = 2 }
                );
            }
            if (!Limites.Any())
            {
                Limites.AddRange(

                // Rule 1 — CC / PagaFacil
                new Limit { Id = 1, RuleId = 1, Min = 0, Max = 9999999, Charge = 1, Type = 'P' },
                // Rule 2 — Cash / PagaFacil
                new Limit { Id = 4, RuleId = 2, Min = 0, Max = 9999999, Charge = 15.0m, Type = 'C' },
       
                // Rule 3 — CC / CazaPagos
                new Limit { Id = 5, RuleId = 3, Min = 0, Max = 1500, Charge = 2.0m, Type = 'P' },
                new Limit { Id = 6, RuleId = 3, Min = 1501, Max = 5000, Charge = 1.5m, Type = 'P' },
                new Limit { Id = 7, RuleId = 3, Min = 5000, Max = 9999999, Charge = 0.5m, Type = 'P' },
                // Rule 4 — Transfer / CazaPagos
                new Limit { Id = 8, RuleId = 4, Min = 0, Max = 500, Charge = 5.00m, Type = 'C' },
                new Limit { Id = 9, RuleId = 4, Min = 501, Max = 1000, Charge = 2.5m, Type = 'P' },
                new Limit { Id = 10, RuleId = 4, Min = 1001, Max = 9999999, Charge = 2.0m, Type = 'P' }

                );
            }

            SaveChanges();
        }
    }



}
