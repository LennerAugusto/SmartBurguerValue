using AutoMapper;
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
        private readonly IMapper _map;
        public PurchaseRepository(AppDbContext context, IUnityOfWork unityOfWork, IMapper map) : base(context)
        {
            _UnityOfWork = unityOfWork;
            _map = map;
        }
        public async Task<List<PurchaseDTO>> GetAllPurchasesByEnterpriseId(Guid enterpriseId)
        {
            var query = await _context.Purchase
                .Where(x => x.EnterpriseId == enterpriseId)
                .OrderBy(x => x.Id)
                .ToListAsync();
            return _map.Map<List<PurchaseDTO>>(query);
        }
        public async Task<PurchaseDTO> GetPurchaseById(Guid purchaseId)
        {
            var purchase = _context.Purchase
                .Where(x => x.Id == purchaseId)
                .Include(x => x.Items)
                .ThenInclude(x=> x.UnityOfMensure)
                .FirstOrDefault();
            return _map.Map<PurchaseDTO>(purchase);
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
                Items = dto.Items.Select(i => new PurchaseItemEntity
                {
                    Id = Guid.NewGuid(),
                    PurchaseId = purchaseId,
                    NameItem = i.NameItem,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    IngredientId = i.IngredientId,
                    UnityOfMensureId = i.UnityOfMensureId,
                    InventoryItemId = i.InventoryItemId,
                    IsActive = true,
                }).ToList()
            };

            _context.Purchase.Add(entity);
            await _context.SaveChangesAsync();

            await UpdateProductsAsync(dto.EnterpriseId, dto);
            await UpdateComboAsync(dto.EnterpriseId, dto);
            return entity;
        }

        public async Task UpdatePurchaseAsync(PurchaseDTO dto)
        {
            var purchase = await _context.Purchase
                .FirstOrDefaultAsync(p => p.Id == dto.Id);

            if (purchase == null)
                throw new Exception("Compra não encontrada.");

            decimal total = 0;

            var oldItems = await _context.PurchaseItem
                .Where(pi => pi.PurchaseId == purchase.Id)
                .ToListAsync();

            _context.PurchaseItem.RemoveRange(oldItems);

            foreach (var item in dto.Items)
            {
                var purchaseItem = new PurchaseItemEntity
                {
                    Id = Guid.NewGuid(),
                    PurchaseId = purchase.Id,
                    IngredientId = item.IngredientId,
                    InventoryItemId = item.InventoryItemId,
                    UnityOfMensureId = item.UnityOfMensureId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    DateCreated = DateTime.UtcNow,
                    DateUpdated = DateTime.UtcNow,
                    NameItem = item.NameItem,
                    IsActive = true
                };

                total += item.Quantity * item.UnitPrice;

                await _context.PurchaseItem.AddAsync(purchaseItem);
            }

            purchase.SupplierName = dto.SupplierName;
            purchase.PurchaseDate = dto.PurchaseDate;
            purchase.IsActive = dto.IsActive;
            purchase.TotalAmount = total;
            purchase.DateUpdated = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task UpdateProductsAsync(Guid enterpriseId, PurchaseDTO purchase)
        {
            var products = await _UnityOfWork.ProductRepository.GetAllProductsByEnterpriseId(enterpriseId);

            foreach (var product in products)
            {
                if (product.Ingredients == null || !product.Ingredients.Any())
                    continue;

                var matchedIngredients = product.Ingredients
                    .Where(ingredient => purchase.Items
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
                        var purchaseItem = purchase.Items
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
