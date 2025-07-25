using Microsoft.EntityFrameworkCore;
using SmartBurguerValueAPI.Context;
using SmartBurguerValueAPI.DTOs;
using SmartBurguerValueAPI.DTOs.Products;
using SmartBurguerValueAPI.Interfaces;
using SmartBurguerValueAPI.Models;
using SmartBurguerValueAPI.Pagination;
using SmartBurguerValueAPI.Repository.Base;

namespace SmartBurguerValueAPI.Repository
{
    public class PurchaseRepository : RepositoryBase<PurchaseEntity>, IPurchaseRepository
    {
        public PurchaseRepository(AppDbContext context) : base(context)
        {
        }
        public async Task<List<PurchaseDTO>> GetAllPurchasesByEnterpriseId(Guid enterpriseId)
        {
            var query = await _context.Purchase
                .Where(x => x.EnterpriseId == enterpriseId)
                .OrderBy(x => x.Id)
                .Select(x => new PurchaseDTO
                {
                    Id = x.Id,
                    SupplierName = x.SupplierName,
                    PurchaseDate = x.PurchaseDate,
                    TotalAmount = x.TotalAmount,
                    PurchaseItems = x.Items.Select(i => new PurchaseItemDTO
                    {
                        NameItem = i.NameItem,
                        Quantity = i.Quantity,
                        UnitPrice = i.UnitPrice,
                    }).ToList()
                }).ToListAsync();
            return query;
        }

        public async Task<PurchaseEntity> CreatePurchase(PurchaseDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var purchaseId = dto.Id != Guid.Empty ? dto.Id : Guid.NewGuid();

            var entity = new PurchaseEntity
            {
                Id = purchaseId,
                SupplierName = dto.SupplierName,
                PurchaseDate = dto.PurchaseDate,
                TotalAmount = dto.TotalAmount,
                IsActive = dto.IsActive,
                DateCreated = dto.DateCreated != default ? dto.DateCreated : DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow,
                EnterpriseId = dto.EnterpriseId,
                Items = dto.PurchaseItems.Select(i => new PurchaseItemEntity
                {
                    Id = Guid.NewGuid(),
                    PurchaseId = purchaseId, 
                    NameItem = i.NameItem,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    IngredientId = i.IngredientId,
                    UnityOfMensureId = i.UnityOfMeasureId,
                    InventoryItemId = i.InventoryItemId
                }).ToList()
            };

            _context.Purchase.Add(entity);
            await _context.SaveChangesAsync();

            return entity;
        }
        public async Task<PurchaseItemEntity> GetPurchaseItemRencentlyByIngredientId(Guid IngredientId)
        {
            return await _context.PurchaseItem
                .Where(x => x.IngredientId == IngredientId)
                .OrderByDescending(x => x.DateCreated)
                .FirstOrDefaultAsync();
        }
    }
}
