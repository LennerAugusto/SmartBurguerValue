using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SmartBurguerValueAPI.Context;
using SmartBurguerValueAPI.DTOs.Products;
using SmartBurguerValueAPI.Interfaces;
using SmartBurguerValueAPI.Interfaces.IProducts;
using SmartBurguerValueAPI.IRepository.IProducts;
using SmartBurguerValueAPI.Models.Products;
using SmartBurguerValueAPI.Repository.Base;

namespace SmartBurguerValueAPI.Repository.ProductsRepository
{
    public class ProductRepository : RepositoryBase<ProductsEntity>, IProductRepository
    {
        private readonly IIngredientRepository _ingredientRepository;
        private readonly IProductIngredientsRepository _productIngredientsRepository;
        private readonly IProductRepository _productRepository;
        public ProductRepository(AppDbContext context) : base(context)
        {
        }
        public async Task<IEnumerable<ProductDTO>> GetAllProductsByEnterpriseId(Guid enterpriseId)
        {
            return await _context.Set<ProductDTO>()
                .Where(x => x.EnterpriseId == enterpriseId)
                .AsNoTracking().ToListAsync();
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
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow,
                IsActive = true
            };

            foreach (var item in dto.Ingredients)
            {
                var ingredient = await _ingredientRepository.GetById(item.IngredientId);
                if (ingredient == null)
                    throw new Exception($"Ingredient not found: {item.IngredientId}");

                var quantityInBase = item.Quantity * ingredient.UnitOfMeasure.ConversionFactor;

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

            return product.Id;
        }
        public async Task UpdateProductAsync(ProductDTO dto)
        {
            var product = await _productRepository.GetById(dto.Id);
            if (product == null)
                throw new Exception("Product not found");

            product.Name = dto.Name;
            product.Description = dto.Description;
            product.ImageUrl = dto.ImageUrl;
            product.DateUpdated = DateTime.UtcNow;

            await _productRepository.Update(product);

            await _productIngredientsRepository.RemoveAllByProductIdAsync(product.Id);

            // Add updated ingredients
            foreach (var item in dto.Ingredients)
            {
                var ingredient = await _ingredientRepository.GetById(item.IngredientId);
                if (ingredient == null)
                    throw new Exception($"Ingredient not found: {item.IngredientId}");

                var quantityInBase = item.Quantity * ingredient.UnitOfMeasure.ConversionFactor;

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
        }
    }
}
