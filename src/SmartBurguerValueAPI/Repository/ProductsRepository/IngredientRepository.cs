using Microsoft.EntityFrameworkCore;
using SmartBurguerValueAPI.Context;
using SmartBurguerValueAPI.DTOs.Products;
using SmartBurguerValueAPI.Interfaces.IProducts;
using SmartBurguerValueAPI.Models;
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
        public async Task CreateIngredient(IngredientsEntity entity)
        {
            var now = DateTime.UtcNow;

            entity.DateCreated = entity.DateCreated == default ? now : entity.DateCreated;
            entity.DateUpdated = now;

            var existingItem = await _context.Set<InventoryItemEntity>()
                .FirstOrDefaultAsync(i =>
                    i.Name.ToLower().Trim() == entity.Name.ToLower().Trim() &&
                    i.EnterpriseId == entity.EnterpriseId &&
                    i.NameCategory == "Ingredient" &&
                    i.IsActive);
            if (existingItem != null)
            {
                entity.InventoryItemId = existingItem.Id;
            }
            else
            {
                var inventoryItem = new InventoryItemEntity
                {
                    Id = Guid.NewGuid(),
                    Name = entity.Name,
                    NameCategory = "Ingredient",
                    UnityOfMensureId = entity.UnitOfMeasureId,
                    EnterpriseId = entity.EnterpriseId,
                    DateCreated = now,
                    DateUpdated = now,
                    IsActive = true
                };

                await _context.Set<InventoryItemEntity>().AddAsync(inventoryItem);
                entity.InventoryItemId = inventoryItem.Id;
            }
            await _context.Set<IngredientsEntity>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }



    }
}
