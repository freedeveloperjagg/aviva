using AvivaLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace AvivaApi.Data
{
    public class PaymentDbContext : DbContext
    {
        //public PaymentDbContext(DbContextOptions<PaymentDbContext> options) : base(options) { }

        //public DbSet<EntidadDePago> EntidadesDePago { get; set; }
        //public DbSet<Rule> Rules { get; set; }
        //public DbSet<Limit> Limites { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    // ── EntidadDePago ────────────────────────────────────────────
        //    modelBuilder.Entity<EntidadDePago>(entity =>
        //    {
        //        entity.HasKey(e => e.Id);

        //        entity.Property(e => e.Nombre)
        //              .IsRequired()
        //              .HasMaxLength(100);

        //        entity.HasMany(e => e.Rules)
        //              .WithOne()
        //              .HasForeignKey(r => r.EntidadDePagoId)
        //              .OnDelete(DeleteBehavior.Cascade);
        //    });

        //    // ── Rule ─────────────────────────────────────────────────────
        //    modelBuilder.Entity<Rule>(entity =>
        //    {
        //        entity.HasKey(r => r.Id);

        //        entity.Property(r => r.TipoPago)
        //              .IsRequired()
        //              .HasMaxLength(20);

        //        entity.HasMany(r => r.Limites)
        //              .WithOne()
        //              .HasForeignKey(l => l.RuleId)
        //              .OnDelete(DeleteBehavior.Cascade);
        //    });

            //// ── Limit ─────────────────────────────────────────────────────
            //modelBuilder.Entity<Limit>(entity =>
            //{
            //    entity.HasKey(l => l.Id);

            //    entity.Property(l => l.Charge)
            //          .HasColumnType("decimal(18,2)");

            //    // char is not natively supported in SQLite — store as string(1)
            //    entity.Property(l => l.Type)
            //          .HasConversion<string>()
            //          .HasMaxLength(1);
            //});

           
       // }

        //public static void Seed(PaymentDbContext context)
        //{
        //    if (context.EntidadDePago.Any()) return; // Database already seeded

        //    var p1 = new EntidadDePago { Nombre = "PagaFacil" };
        //    p1.Rules.Add(CreateRule("CASH", 0, 0, "15"));
        //    p1.Rules.Add(CreateRule("CC", 0, 0, "1%"));

        //    var p2 = new EntidadDePago { Nombre = "CazaPagos" };
        //    p2.Rules.Add(CreateRule("CC", 0, 1500, "2%"));
        //    p2.Rules.Add(CreateRule("CC", 1501, 5000, "1.5%"));
        //    p2.Rules.Add(CreateRule("CC", 5001, 0, "0.5%"));
        //    p2.Rules.Add(CreateRule("TRANS", 0, 500, "5"));
        //    p2.Rules.Add(CreateRule("TRANS", 501, 1000, "2.5%"));
        //    p2.Rules.Add(CreateRule("TRANS", 1000, 0, "2.0%"));

        //    context.EntidadesDePago.AddRange(p1, p2);
        //    context.SaveChanges();
        //}

        //private static Rule CreateRule(string tipo, int min, int max, string chargeStr)
        //{
        //    bool isPercent = chargeStr.Contains('%');
        //    decimal val = decimal.Parse(chargeStr.Replace("%", ""), System.Globalization.CultureInfo.InvariantCulture);

        //    var rule = new Rule { TipoPago = tipo };
        //    rule.Limites.Add(new Limit
        //    {
        //        Min = min,
        //        Max = max,
        //        Charge = val,
        //        Type = isPercent ? 'P' : 'F' // 'P' for percentage, 'F' for fixed/pesos
        //    });
        //    return rule;
        //}

    }





} 
