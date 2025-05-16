using Microsoft.EntityFrameworkCore;
using SmartBurguerValueAPI.Context;
using SmartBurguerValueAPI.Interfaces.IProducts;
using SmartBurguerValueAPI.Models.Products;
using SmartBurguerValueAPI.Repository.Base;

namespace SmartBurguerValueAPI.Repository.ProductsRepository
{
    public class ProductIngredientRepository : RepositoryBase<ProductsIngredientsEntity>, IProductIngredientsRepository
    {
        private readonly AppDbContext _context;
        public ProductIngredientRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task RemoveAllByProductIdAsync(Guid productId)
        {
            var list = await _context.ProductIngredients
                .Where(pi => pi.ProductId == productId)
                .ToListAsync();

            _context.ProductIngredients.RemoveRange(list);
            await _context.SaveChangesAsync();
        }

        public async Task AddAsync(ProductsIngredientsEntity productIngredient)
        {
            await _context.ProductIngredients.AddAsync(productIngredient);
        }
    }
}
