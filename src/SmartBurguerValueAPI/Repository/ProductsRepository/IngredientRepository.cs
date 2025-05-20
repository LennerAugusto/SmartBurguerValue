using Microsoft.EntityFrameworkCore;
using SmartBurguerValueAPI.Context;
using SmartBurguerValueAPI.DTOs.Products;
using SmartBurguerValueAPI.Interfaces.IProducts;
using SmartBurguerValueAPI.Models.Products;
using SmartBurguerValueAPI.Pagination;
using SmartBurguerValueAPI.Repository.Base;

namespace SmartBurguerValueAPI.Repository.ProductsRepository
{
    public class IngredientRepository : RepositoryBase<IngredientsEntity>, IIngredientRepository
    {
        public IngredientRepository(AppDbContext context) : base(context)
        {
        }
        public async Task<PagedList<IngredientDTO>> GetAllIngredientByEnterpriseId(PaginationParamiters paramiters, Guid enterpriseId)
        {
            var query = _context.Ingredients // ou _context.Set<IngredientEntity>()
                .Where(x => x.EnterpriseId == enterpriseId)
                .AsNoTracking()
                .Select(x => new IngredientDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    PurchaseQuantity = x.PurchaseQuantity,
                    PurchaseDate = x.PurchaseDate,
                    PurchasePrice = x.PurchasePrice,
                    UnitOfMeasureId = x.UnitOfMeasureId,
                    EnterpriseId = x.EnterpriseId
                });

            return PagedList<IngredientDTO>.ToPagedList(query, paramiters.PageNumber, paramiters.PageSize);
        }

    }
}
