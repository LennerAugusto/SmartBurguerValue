using Microsoft.EntityFrameworkCore;
using SmartBurguerValueAPI.Models.Products;

namespace SmartBurguerValueAPI.Context
{
    public class SmartBurguerValueAPIContext : DbContext
    {
        public SmartBurguerValueAPIContext(DbContextOptions<SmartBurguerValueAPIContext> options) : base(options)
        {

        }
        public DbSet<CategoryProductsEntity> CategoryProducts { get; set; }
        public DbSet<UnityTypesProductsEntity> UnityTypesProducts { get; set; }
        public DbSet<ProductsEntity> Products { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CategoryProductsEntity>().HasKey(e => e.Id);

            modelBuilder.Entity<UnityTypesProductsEntity>().HasKey(e => e.Id);

            modelBuilder.Entity<ProductsEntity>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProductsEntity>()
               .HasOne(p => p.UnityTypes)
               .WithMany(c => c.Products)
               .HasForeignKey(p => p.UnityTypeId)
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
