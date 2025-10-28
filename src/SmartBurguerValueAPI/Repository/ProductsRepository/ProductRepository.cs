using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SmartBurguerValueAPI.Context;
using SmartBurguerValueAPI.DTOs.Products;
using SmartBurguerValueAPI.Interfaces;
using SmartBurguerValueAPI.Interfaces.IProducts;
using SmartBurguerValueAPI.IRepository.IProducts;
using SmartBurguerValueAPI.Models.Products;
using SmartBurguerValueAPI.Pagination;
using SmartBurguerValueAPI.Repository.Base;

namespace SmartBurguerValueAPI.Repository.ProductsRepository
{
    public class ProductRepository : RepositoryBase<ProductsEntity>, IProductRepository
    {
        private readonly IUnityOfWork _UnityOfWork;

        public ProductRepository(AppDbContext context, IUnityOfWork unityOfWork) : base(context)
        {
            _UnityOfWork = unityOfWork;
        }

        public async Task<List<ProductDTO>> GetAllProductsByEnterpriseId( Guid enterpriseId)
        {
            var query = _context.Products
                .Where(x => x.EnterpriseId == enterpriseId)
                .Include(i => i.ProductIngredients)
                .OrderBy(x => x.Id)
                .Select(x => new ProductDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    DesiredMargin = x.DesiredMargin,
                    SellingPrice = x.SellingPrice,
                    SuggestedPrice = x.SuggestedPrice,
                    CPV = x.CPV,
                    CMV = x.CMV,
                    Markup = x.Markup,
                    Margin = x.Margin,
                    ImageUrl = x.ImageUrl,
                    ProductType = x.ProductType,
                    EnterpriseId = enterpriseId,
                    IsActive = x.IsActive,
                    Ingredients = x.ProductIngredients
                    .Select(i => new ProductIngredientDTO
                    {
                        Id = i.IngredientId,
                        Name = i.Name,
                        Quantity = i.QuantityUsedInBase,
                        BaseUnit = i.Ingredient.UnitOfMeasure.BaseUnit
                    }).ToList()
                }).ToList();

            return query;
        }

        public async Task<Guid> CreateProductAsync(ProductDTO dto)
        {
            var product = new ProductsEntity
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Description = dto.Description,
                EnterpriseId = dto.EnterpriseId,
                ImageUrl = dto.ImageUrl,
                DesiredMargin = dto.DesiredMargin,
                SellingPrice = dto.SellingPrice,
                SuggestedPrice = dto.SuggestedPrice,
                ProductType = dto.ProductType,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow,
                IsActive = dto.IsActive
            };

            decimal totalCost = 0;

            foreach (var item in dto.Ingredients)
            {
                var purchase = await _UnityOfWork.PurchaseItemRepository.GetPurchaseItemRencentlyByIngredientId(item.Id);
                if (purchase == null)
                    throw new Exception($"Purchase item not found for ingredient: {item.Id}");

                var cost = (purchase.UnitPrice * item.Quantity) / purchase.UnityOfMensure.ConversionFactor;
                totalCost += cost;

                var productIngredient = new ProductsIngredientsEntity
                {
                    Id = Guid.NewGuid(),
                    Name = item.Name,
                    ProductId = product.Id,
                    IngredientId = item.Id,
                    QuantityUsedInBase = item.Quantity,
                    DateCreated = DateTime.UtcNow,
                    DateUpdated = DateTime.UtcNow,
                    IsActive = true
                };

                await _UnityOfWork.ProductsIngredientRepository.Create(productIngredient);
            }

            product.CPV = totalCost;

            if (!product.SellingPrice.HasValue && product.DesiredMargin.HasValue && product.CPV.HasValue)
            {
                var marginDecimal = product.DesiredMargin.Value / 100m;
                product.SellingPrice = Math.Round(product.CPV.Value / (1m - marginDecimal), 2);
            }

            if (!product.DesiredMargin.HasValue && product.SellingPrice.HasValue && product.CPV.HasValue)
            {
                product.DesiredMargin = Math.Round((1m - (product.CPV.Value / product.SellingPrice.Value)) * 100m, 2);
            }

            if (product.DesiredMargin.HasValue && product.CPV.HasValue)
            {
                var marginDecimal = product.DesiredMargin.Value / 100m;
                product.SuggestedPrice = Math.Round(product.CPV.Value / (1m - marginDecimal), 2);
            }
            else
            {
                product.SuggestedPrice = null;
            }

            if (product.SellingPrice.HasValue && product.CPV.HasValue && product.SellingPrice.Value > 0)
            {
                product.CMV = Math.Round((product.CPV.Value / product.SellingPrice.Value) * 100m, 2);
            }
            else
            {
                product.CMV = null;
            }

            if (product.CPV.HasValue && product.CPV.Value > 0 && product.SellingPrice.HasValue)
            {
                product.Markup = Math.Round(((product.SellingPrice.Value / product.CPV.Value) - 1m), 2);
            }
            else
            {
                product.Markup = null;
            }

            if (product.SellingPrice.HasValue && product.SellingPrice.Value > 0 && product.CPV.HasValue)
            {
                product.Margin = Math.Round(((product.SellingPrice.Value - product.CPV.Value) / product.SellingPrice.Value) * 100m, 2);
            }
            else
            {
                product.Margin = null;
            }

            await _UnityOfWork.ProductRepository.Create(product);

            return product.Id;
        }

        public async Task UpdateProductAsync(ProductDTO dto)
        {
            var product = await _UnityOfWork.ProductRepository.GetByIdAsync(dto.Id);
            if (product == null)
                throw new Exception("Product not found");

            product.Name = dto.Name;
            product.ProductType = dto.ProductType;
            product.Description = dto.Description;
            product.ImageUrl = dto.ImageUrl;
            product.SellingPrice = dto.SellingPrice ?? product.SellingPrice;
            product.DesiredMargin = dto.DesiredMargin ?? product.DesiredMargin;
            product.SuggestedPrice = dto.SuggestedPrice ?? product.SuggestedPrice;
            product.DateUpdated = DateTime.UtcNow;
            product.IsActive = dto.IsActive;
            await _UnityOfWork.ProductsIngredientRepository.RemoveAllByProductIdAsync(product.Id);

            decimal totalCost = 0;

            foreach (var item in dto.Ingredients)
            {
                var purchase = await _UnityOfWork.PurchaseItemRepository.GetPurchaseItemRencentlyByIngredientId(item.Id);
                if (purchase == null)
                    throw new Exception($"Purchase item not found for ingredient: {item.Id}");

                var cost = (purchase.UnitPrice * item.Quantity) / purchase.UnityOfMensure.ConversionFactor;
                totalCost += cost;

                var productIngredient = new ProductsIngredientsEntity
                {
                    Id = Guid.NewGuid(),
                    Name = item.Name,
                    ProductId = product.Id,
                    IngredientId = item.Id,
                    QuantityUsedInBase = item.Quantity,
                    DateCreated = DateTime.UtcNow,
                    DateUpdated = DateTime.UtcNow,
                    IsActive = true
                };

                await _UnityOfWork.ProductsIngredientRepository.AddAsync(productIngredient);
            }

            product.CPV = totalCost;

            if (!product.SellingPrice.HasValue && product.DesiredMargin.HasValue && product.CPV.HasValue)
            {
                var marginDecimal = product.DesiredMargin.Value / 100m;
                product.SellingPrice = Math.Round(product.CPV.Value / (1m - marginDecimal), 2);
            }

            if (!product.DesiredMargin.HasValue && product.SellingPrice.HasValue && product.CPV.HasValue)
            {
                product.DesiredMargin = Math.Round((1m - (product.CPV.Value / product.SellingPrice.Value)) * 100m, 2);
            }

            if (product.DesiredMargin.HasValue && product.CPV.HasValue)
            {
                var marginDecimal = product.DesiredMargin.Value / 100m;
                product.SuggestedPrice = Math.Round(product.CPV.Value / (1m - marginDecimal), 2);
            }
            else
            {
                product.SuggestedPrice = null;
            }

            if (product.SellingPrice.HasValue && product.CPV.HasValue && product.SellingPrice.Value > 0)
            {
                product.CMV = Math.Round((product.CPV.Value / product.SellingPrice.Value) * 100m, 2);
            }
            else
            {
                product.CMV = null;
            }

            if (product.CPV.HasValue && product.CPV.Value > 0 && product.SellingPrice.HasValue)
            {
                product.Markup = Math.Round(((product.SellingPrice.Value / product.CPV.Value) - 1m), 2);
            }
            else
            {
                product.Markup = null;
            }

            if (product.SellingPrice.HasValue && product.SellingPrice.Value > 0 && product.CPV.HasValue)
            {
                product.Margin = Math.Round(((product.SellingPrice.Value - product.CPV.Value) / product.SellingPrice.Value) * 100m, 2);
            }
            else
            {
                product.Margin = null;
            }

            _UnityOfWork.ProductRepository.Update(product);
        }

        public async Task<ProductDTO> GetByIdWithIngredientsAsync(Guid productId)
        {
            var productEntity = await _context.Products
                .Include(p => p.ProductIngredients)
                 .ThenInclude(pi => pi.Ingredient.UnitOfMeasure)
                .FirstOrDefaultAsync(p => p.Id == productId);

            if (productEntity == null)
                return null;

            var productDto = new ProductDTO
            {
                Id = productEntity.Id,
                Name = productEntity.Name,
                Description = productEntity.Description,
                EnterpriseId = productEntity.EnterpriseId,
                ImageUrl = productEntity.ImageUrl,
                DesiredMargin = productEntity.DesiredMargin,
                SellingPrice = productEntity.SellingPrice,
                SuggestedPrice = productEntity.SuggestedPrice,
                ProductType = productEntity.ProductType,
                CPV  = productEntity.CPV,
                CMV = productEntity.CMV,
                Margin =productEntity.Margin,
                Markup = productEntity.Markup,
                DateCreated = DateTime.UtcNow,
                DateUpdate = DateTime.UtcNow,
                IsActive = productEntity.IsActive,
                Ingredients = productEntity.ProductIngredients.Select(i => new ProductIngredientDTO
                {
                    Id = i.IngredientId,
                    Name = i.Name,
                    Quantity = i.QuantityUsedInBase, 
                    BaseUnit = i.Ingredient.UnitOfMeasure.BaseUnit,
                }).ToList()
            };

            return productDto;
        }

    }
}
