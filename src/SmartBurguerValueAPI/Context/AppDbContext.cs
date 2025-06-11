using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SmartBurguerValueAPI.Models;
using SmartBurguerValueAPI.Models.Products;

namespace SmartBurguerValueAPI.Context
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
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
        public DbSet<ComboProductEntity> ComboProducts { get; set; }
        public DbSet<FixedCostEntity> FixedCosts{ get; set; }
        public DbSet <SalesGoalEntity> SalesGoal{ get; set; }
        public DbSet <DailyEntryEntity> DailyEntry { get; set; }
        public DbSet <EmployeeEntity> Employees { get; set; }
        public DbSet <EmployeeWorkScheduleEntity> EmployeesWorkSchedule{get; set;}
        public DbSet <PurchaseEntity> Purchase{get; set;}
        public DbSet <PurchaseItemEntity> PurchaseItem{get; set;}
        public DbSet <InventoryItemEntity> InventoryItem{get; set;}

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

            modelBuilder.Entity<ComboProductEntity>()
                .HasOne(cp => cp.Combo)
                .WithMany(c => c.ComboProducts)
                .HasForeignKey(cp => cp.ComboId);

            modelBuilder.Entity<ComboProductEntity>()
                .HasOne(cp => cp.Product)
                .WithMany(p => p.ComboProducts)
                .HasForeignKey(cp => cp.ProductId);
        }

    }
}
