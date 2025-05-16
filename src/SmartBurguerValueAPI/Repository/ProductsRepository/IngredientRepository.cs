using Microsoft.EntityFrameworkCore;
using SmartBurguerValueAPI.Context;
using SmartBurguerValueAPI.DTOs.Products;
using SmartBurguerValueAPI.Interfaces.IProducts;
using SmartBurguerValueAPI.Models.Products;
using SmartBurguerValueAPI.Repository.Base;

namespace SmartBurguerValueAPI.Repository.ProductsRepository
{
    public class IngredientRepository : RepositoryBase<IngredientsEntity>, IIngredientRepository
    {
        public IngredientRepository(AppDbContext context) : base(context)
        {
        }
        public async Task<IEnumerable<IngredientDTO>> GetAllIngredientByEnterpriseId(Guid enterpriseid)
        {
            return await _context.Set<IngredientDTO>()
                .Where(x => x.EnterpriseId == enterpriseid)
                .AsNoTracking().ToListAsync();
        }
    }
}
