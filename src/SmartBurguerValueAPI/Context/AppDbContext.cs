using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SmartBurguerValueAPI.Models;
using SmartBurguerValueAPI.Models.Products;

namespace SmartBurguerValueAPI.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<EnterpriseEntity> Enterprises { get; set; }
        //public DbSet<UsersEntity> Users { get; set; }
        public DbSet<UnityTypesProductsEntity> UnityTypes { get; set; }
        public DbSet<IngredientsEntity> Ingredients { get; set; }
        public DbSet<ProductsEntity> Products { get; set; }
        public DbSet<ProductsIngredientsEntity> ProductIngredients { get; set; }
        public DbSet<ComboEntity> Combos { get; set; }
        public DbSet<ComboProduct> ComboProducts { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<UsersEntity>()
            //    .HasIndex(u => u.Email)
            //    .IsUnique();

            modelBuilder.Entity<ProductsIngredientsEntity>()
                .HasOne(pi => pi.Product)
                .WithMany(p => p.ProductIngredients)
                .HasForeignKey(pi => pi.ProductId);

            modelBuilder.Entity<ProductsIngredientsEntity>()
                .HasOne(pi => pi.Ingredient)
                .WithMany(i => i.ProductIngredients)
                .HasForeignKey(pi => pi.IngredientId);

            modelBuilder.Entity<ComboProduct>()
                .HasOne(cp => cp.Combo)
                .WithMany(c => c.ComboProducts)
                .HasForeignKey(cp => cp.ComboId);

            modelBuilder.Entity<ComboProduct>()
                .HasOne(cp => cp.Product)
                .WithMany(p => p.ComboProducts)
                .HasForeignKey(cp => cp.ProductId);
        }
    }
}
