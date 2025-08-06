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
    //        var product = await _context.Products
    //            .Include(p => p.ProductIngredients)
    //                .ThenInclude(pi => pi.Ingredient)
    //                    .ThenInclude(i => i.InventoryItem)
    //                        .ThenInclude(ii => ii.PurchaseItems)
    //                            .ThenInclude(p => p.UnityOfMensure)
    //            .FirstOrDefaultAsync(p => p.Id == productId && p.IsActive);

    //        if (product == null)
    //            throw new Exception("Produto não encontrado");

    //        decimal? unitCost = product.CPV;

    //        //foreach (var pi in product.ProductIngredients)
    //        //{
    //        //    var item = pi.Ingredient.InventoryItem;
    //        //    var purchases = item?.PurchaseItems?.Where(p => p.IsActive).ToList();

    //        //    if (purchases == null || !purchases.Any())
    //        //        continue;

    //        //    var totalQty = purchases.Sum(i => i.Quantity * i.UnityOfMensure.ConversionFactor);
    //        //    var totalVal = purchases.Sum(i => i.Quantity * i.UnitPrice);

    //        //    if (totalQty == 0)
    //        //        continue;

    //        //    var avgCost = totalVal / totalQty;
    //        //    unitCost += avgCost * pi.QuantityUsedInBase;
    //        //}

    //        decimal? sellingPrice = product.SellingPrice;
    //        decimal? desiredMargin = product.DesiredMargin;

    //        decimal? markup = ((sellingPrice - product.CPV) * product.CPV)*100;
    //        decimal? margin = sellingPrice > 0 ? (sellingPrice - unitCost) / sellingPrice : null;
    //        decimal? cmv = sellingPrice > 0 ? unitCost / sellingPrice : null;

    ////        decimal? suggestedPrice = (desiredMargin.HasValue && unitCost > 0)
    ////? Math.Round(unitCost / (1m - desiredMargin.Value), 2)
    ////: null;

    //        decimal finalPrice = suggestedPrice ?? 0m; // ou outro valor padrão

    //        var analysis = new ProductCostAnalysisEntity
    //        {
    //            Id = Guid.NewGuid(),
    //            ProductId = productId,
    //            EnterpriseId = product.EnterpriseId,
    //            AnalisysDate = DateTime.UtcNow.Date,
    //            UnitCost = unitCost,
    //            SellingPrice = sellingPrice,
    //            Markup = markup,
    //            Margin = margin,
    //            CPV = unitCost,
    //            CMV = cmv,
    //            SellingPriceSuggested = suggestedPrice,
    //            DateCreated = DateTime.UtcNow,
    //            DateUpdated = DateTime.UtcNow,
    //            IsActive = true
    //        };

    //        await _context.ProductCostAnalysis.AddAsync(analysis);
    //        await _context.SaveChangesAsync();

            return null;
        }


    }
}
