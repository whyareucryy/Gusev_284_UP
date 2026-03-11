using Gusev284_UP.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace Gusev284_UP.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<MaterialType> MaterialTypes { get; set; }
        public DbSet<Workshop> Workshops { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductWorkshop> ProductWorkshops { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Настройка связей и ограничений (если нужно)

            // Уникальность артикула
            modelBuilder.Entity<Product>()
                .HasIndex(p => p.Article)
                .IsUnique();

            // Составной уникальный ключ для связи продукта и цеха (чтобы не было дубликатов)
            modelBuilder.Entity<ProductWorkshop>()
                .HasIndex(pw => new { pw.ProductId, pw.WorkshopId })
                .IsUnique();

            // Каскадное удаление: при удалении продукта удаляются связанные записи в ProductWorkshops
            modelBuilder.Entity<ProductWorkshop>()
                .HasOne(pw => pw.Product)
                .WithMany(p => p.ProductWorkshops)
                .HasForeignKey(pw => pw.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProductWorkshop>()
                .HasOne(pw => pw.Workshop)
                .WithMany(w => w.ProductWorkshops)
                .HasForeignKey(pw => pw.WorkshopId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}