using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartBurguerValueAPI.Context;
using SmartBurguerValueAPI.Interfaces;
using SmartBurguerValueAPI.Models;
using SmartBurguerValueAPI.Repository.Base;

namespace SmartBurguerValueAPI.Repository
{
    public class ProductCostAnalysisRepository : RepositoryBase<ProductCostAnalysisEntity>, IProductCostAnalysisRepository
    {
        public ProductCostAnalysisRepository(AppDbContext context) : base(context)
        {
        }
        public async Task<ProductCostAnalysisEntity> GetAnalyse(Guid productId)
        {
            var product = await _context.Products
           .Include(p => p.ProductIngredients)
               .ThenInclude(pi => pi.Ingredient)
                   .ThenInclude(i => i.InventoryItem)
                       .ThenInclude(ii => ii.PurchaseItems)
                           .ThenInclude(p => p.UnityOfMensure)
           .FirstOrDefaultAsync(p => p.Id == productId && p.IsActive);

            if (product == null) throw new Exception("Produto não encontrado");

            decimal unitCost = 0;

            foreach (var pi in product.ProductIngredients)
            {
                var item = pi.Ingredient.InventoryItem;
                var purchases = item.PurchaseItems.Where(p => p.IsActive);

                var totalQty = purchases.Sum(i => i.Quantity * i.UnityOfMensure.ConversionFactor);
                var totalVal = purchases.Sum(i => i.Quantity * i.UnitPrice);

                if (totalQty == 0) continue;

                var avgCost = totalVal / totalQty;
                unitCost += avgCost * pi.QuantityUsedInBase;
            }

            decimal? sellingPrice = product.SellingPrice;
            decimal? markup = unitCost > 0 ? (sellingPrice - unitCost) / unitCost : 0;
            decimal? margin = sellingPrice > 0 ? (sellingPrice - unitCost) / sellingPrice : 0;
            decimal? cmv = unitCost / sellingPrice;
            var analysis = new ProductCostAnalysisEntity
            {
                Id = Guid.NewGuid(),
                ProductId = productId,
                EnterpriseId = product.EnterpriseId,
                AnalisysDate = DateTime.UtcNow.Date,
                UnitCost = unitCost,
                SellingPrice = sellingPrice,
                Markup = markup,
                Margin = margin,
                CPV = unitCost,
                CMV = cmv,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow,
                IsActive = true
            };

            await _context.ProductCostAnalysis.AddAsync(analysis);
            await _context.SaveChangesAsync();

            return analysis;
        }
    }
}
