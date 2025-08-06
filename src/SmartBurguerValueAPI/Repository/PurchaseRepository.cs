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
        private readonly IUnityOfWork _UnityOfWork;
        public PurchaseRepository(AppDbContext context, IUnityOfWork unityOfWork) : base(context)
        {
            _UnityOfWork = unityOfWork;
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

            await UpdateProductsAsync(dto.EnterpriseId, dto);
            await UpdateComboAsync(dto.EnterpriseId, dto);
            return entity;
        }

        public async Task UpdateProductsAsync(Guid enterpriseId, PurchaseDTO purchase)
        {
            var products = await _UnityOfWork.ProductRepository.GetAllProductsByEnterpriseId(enterpriseId);

            foreach (var product in products)
            { 
                if (product.Ingredients == null || !product.Ingredients.Any())
                    continue;

                var matchedIngredients = product.Ingredients
                    .Where(ingredient => purchase.PurchaseItems
                        .Any(purchaseItem => purchaseItem.IngredientId == ingredient.Id))
                    .ToList();

                if (matchedIngredients != null)
                {
                  await _UnityOfWork.ProductRepository.UpdateProductAsync(product);
                }
            }

        }
        public async Task UpdateComboAsync(Guid enterpriseId, PurchaseDTO purchase)
        {
            var combos = await _UnityOfWork.ComboRepository.GetAllCombosByEnterpriseIdAsync(enterpriseId);

            foreach (var combo in combos)
            {
                if (combo.Products == null || !combo.Products.Any())
                    continue;

                foreach (var product in combo.Products)
                {
                    if (product.Ingredients == null || !product.Ingredients.Any())
                        continue;

                    foreach (var ingredient in product.Ingredients)
                    {
                        var purchaseItem = purchase.PurchaseItems
                            .Where(p => p.IngredientId == ingredient.Id);

                        if (purchaseItem != null)
                        {
                           await _UnityOfWork.ComboRepository.UpdateComboAsync(combo);
                        }
                    }
                }

            }

        }

    }
}
