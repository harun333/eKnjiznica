using eKnjiznica.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace eKnjiznica.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProductCategory>()
                .HasKey(bc => new { bc.ProductId, bc.CategoryId });

            modelBuilder.Entity<ProductCategory>()
                .HasOne(bc => bc.Product)
                .WithMany(b => b.ProductCategories)
                .HasForeignKey(bc => bc.ProductId);

            modelBuilder.Entity<ProductCategory>()
                .HasOne(bc => bc.Category)
                .WithMany(c => c.ProductCategories)
                .HasForeignKey(bc => bc.CategoryId);
        }
    }
}
