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
        public async Task<List<IngredientDTO>> GetAllIngredientByEnterpriseId( Guid enterpriseId)
        {
            var query = _context.Ingredients 
                .Where(x => x.EnterpriseId == enterpriseId)
                .AsNoTracking()
                .Select(x => new IngredientDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    UnitOfMeasureId = x.UnitOfMeasureId,
                    UnitOfMeasureName = x.UnitOfMeasure != null ? x.UnitOfMeasure.Name : null, 
                    ConversionFactor= x.UnitOfMeasure != null ? x.UnitOfMeasure.ConversionFactor : null, 
                    EnterpriseId = x.EnterpriseId,
                    IsActive = x.IsActive,
                    DateCreated = x.DateCreated,
                    DateUpdate = x.DateUpdated,
                    InventoryItemId = x.InventoryItemId,
                });

            return await query.ToListAsync();
        }
        public async Task<IngredientDTO> CreateIngredient(IngredientsEntity entity)
        {
            var now = DateTime.UtcNow;
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "O objeto 'entity' não pode ser nulo.");
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

            entity.Id = Guid.NewGuid(); 

            await _context.Set<IngredientsEntity>().AddAsync(entity);
            await _context.SaveChangesAsync();

            return new IngredientDTO
            {
                Id = entity.Id,
                Name = entity.Name,
                UnitOfMeasureId = entity.UnitOfMeasureId,
                EnterpriseId = entity.EnterpriseId,
                InventoryItemId = entity.InventoryItemId,
                DateCreated = entity.DateCreated,
                DateUpdate = entity.DateUpdated,
                IsActive = entity.IsActive
            };
        }



    }
}
