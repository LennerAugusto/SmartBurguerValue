using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
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
        private readonly IIngredientRepository _ingredientRepository;
        private readonly IProductIngredientsRepository _productIngredientsRepository;
        private readonly IProductRepository _productRepository;
        private readonly IPurchaseRepository _PurchaseRepository;
        public ProductRepository(AppDbContext context) : base(context)
        {
        }
        public async Task<List<ProductDTO>> GetAllProductsByEnterpriseId( Guid enterpriseId)
        {
            var query = _context.Products
                .Where(x => x.EnterpriseId == enterpriseId)
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
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow,
                IsActive = true
            };

            decimal totalCPV = 0;

            foreach (var item in dto.Ingredients)
            {
                var ingredient = await _PurchaseRepository.GetPurchaseItemRencentlyByIngredientId(item.IngredientId);
                if (ingredient == null)
                    throw new Exception($"Ingredient not found: {item.IngredientId}");

                var quantityInBase = item.Quantity * ingredient.UnityOfMensure.ConversionFactor;
                var unitPrice = ingredient.UnitPrice / (ingredient.Quantity * ingredient.UnityOfMensure.ConversionFactor);

                totalCPV += quantityInBase * unitPrice;

                var productIngredient = new ProductsIngredientsEntity
                {
                    Id = Guid.NewGuid(),
                    ProductId = product.Id,
                    IngredientId = item.IngredientId,
                    QuantityUsedInBase = quantityInBase,
                    DateCreated = DateTime.UtcNow,
                    DateUpdated = DateTime.UtcNow,
                    IsActive = true
                };

                await _productIngredientsRepository.Create(productIngredient);
            }

            product.CPV = totalCPV;

            if (product.DesiredMargin.HasValue && !product.SellingPrice.HasValue)
            {
                product.SellingPrice = product.CPV.HasValue
                    ? Math.Round(product.CPV.Value / (1m - product.DesiredMargin.Value), 2)
                    : null;
            }
            else if (product.SellingPrice.HasValue && !product.DesiredMargin.HasValue)
            {
                product.DesiredMargin = product.CPV.HasValue
                    ? Math.Round(1m - (product.CPV.Value / product.SellingPrice.Value), 4)
                    : null;
            }

            product.SuggestedPrice = (product.DesiredMargin.HasValue && product.CPV.HasValue)
                ? Math.Round(product.CPV.Value / (1m - product.DesiredMargin.Value), 2)
                : null;
            product.CMV = (product.SellingPrice.HasValue && product.SellingPrice.Value != 0 && product.CPV.HasValue)
                ? Math.Round(product.CPV.Value / product.SellingPrice.Value, 4)
                : null;


            await _productRepository.Create(product);

            return product.Id;
        }

        public async Task UpdateProductAsync(ProductDTO dto)
        {
            var product = await _productRepository.GetByIdAsync(dto.Id);
            if (product == null)
                throw new Exception("Product not found");

            product.Name = dto.Name;
            product.Description = dto.Description;
            product.ImageUrl = dto.ImageUrl;
            product.SellingPrice = dto.SellingPrice ?? product.SellingPrice;
            product.DesiredMargin = dto.DesiredMargin ?? product.DesiredMargin;
            product.SuggestedPrice = dto.SuggestedPrice ?? product.SuggestedPrice;
            product.DateUpdated = DateTime.UtcNow;

            await _productIngredientsRepository.RemoveAllByProductIdAsync(product.Id);

            decimal totalCPV = 0;

            foreach (var item in dto.Ingredients)
            {
                var ingredient = await _PurchaseRepository.GetPurchaseItemRencentlyByIngredientId(item.IngredientId);
                if (ingredient == null)
                    throw new Exception($"Ingredient not found: {item.IngredientId}");

                var quantityInBase = item.Quantity * ingredient.UnityOfMensure.ConversionFactor;
                var unitPrice = ingredient.UnitPrice / (ingredient.Quantity * ingredient.UnityOfMensure.ConversionFactor);

                totalCPV += quantityInBase * unitPrice;

                var productIngredient = new ProductsIngredientsEntity
                {
                    Id = Guid.NewGuid(),
                    ProductId = product.Id,
                    IngredientId = item.IngredientId,
                    QuantityUsedInBase = quantityInBase,
                    DateCreated = DateTime.UtcNow,
                    DateUpdated = DateTime.UtcNow,
                    IsActive = true
                };

                await _productIngredientsRepository.AddAsync(productIngredient);
            }

            product.CPV = totalCPV;

            if (product.DesiredMargin.HasValue && !product.SellingPrice.HasValue)
            {
                product.SellingPrice = product.CPV.HasValue
                    ? Math.Round(product.CPV.Value / (1m - product.DesiredMargin.Value), 2)
                    : null;
            }
            else if (product.SellingPrice.HasValue && !product.DesiredMargin.HasValue)
            {
                product.DesiredMargin = product.CPV.HasValue
                    ? Math.Round(1m - (product.CPV.Value / product.SellingPrice.Value), 4)
                    : null;
            }

            product.SuggestedPrice = (product.DesiredMargin.HasValue && product.CPV.HasValue)
                ? Math.Round(product.CPV.Value / (1m - product.DesiredMargin.Value), 2)
                : null;
            product.CMV = (product.SellingPrice.HasValue && product.SellingPrice.Value != 0 && product.CPV.HasValue)
                ? Math.Round(product.CPV.Value / product.SellingPrice.Value, 4)
                : null;

            _productRepository.Update(product);

        }

    }
}
