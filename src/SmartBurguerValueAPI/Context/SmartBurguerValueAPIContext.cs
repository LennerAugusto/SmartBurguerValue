using Microsoft.EntityFrameworkCore;
using SmartBurguerValueAPI.Models.Products;

namespace SmartBurguerValueAPI.Context
{
    public class SmartBurguerValueAPIContext : DbContext
    {
        public SmartBurguerValueAPIContext(DbContextOptions<SmartBurguerValueAPIContext>options) : base(options)
        {

        }
        public DbSet<CategoryProductsEntity> Clients { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CategoryProductsEntity>().HasKey(e => e.Id);
            //modelBuilder.Entity<ClientEntity>()
            //    .HasMany(c => c.OrderServices)
            //    .WithOne(o => o.Client)
            //    .HasForeignKey(o => o.ClientId)
            //    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
