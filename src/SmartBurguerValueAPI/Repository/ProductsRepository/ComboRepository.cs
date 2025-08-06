using Microsoft.EntityFrameworkCore;
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
    public class ComboRepository : RepositoryBase<ComboEntity>, IComboRepository
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnityOfWork _UnityOfWork;

        public ComboRepository(IUnityOfWork unityOfWork, AppDbContext context) : base(context)
        {
            _UnityOfWork = unityOfWork;
        }
        public async Task<ComboDTO> GetComboByIdAsync(Guid comboId)
        {
            var comboEntity = await _context.Set<ComboEntity>()
                .Where(x => x.Id == comboId)
                .Include(x => x.Products)
                    .ThenInclude(cp => cp.Product)
                .FirstOrDefaultAsync();

            if (comboEntity == null)
                throw new Exception("Combo not found");

            var comboDTO = new ComboDTO
            {
                Id = comboEntity.Id,
                Name = comboEntity.Name,
                Description = comboEntity.Description,
                ImageUrl = comboEntity.ImageUrl,
                SellingPrice = comboEntity.SellingPrice,
                SugestedPrice = comboEntity.SugestedPrice,
                Margin = comboEntity.Margin,
                DesiredMargin = comboEntity.DesiredMargin,
                CPV = comboEntity.CPV,
                CMV = comboEntity.CMV,
                Markup = comboEntity.Markup,
                EnterpriseId = comboEntity.EnterpriseId,
                Products = comboEntity.Products.Select(cp => new ProductDTO
                {
                    Id = cp.Product.Id,
                    Name = cp.Product.Name,
                    Description = cp.Product.Description,
                    SellingPrice = cp.Product.SellingPrice,
                    DesiredMargin = cp.Product.DesiredMargin,
                    CPV = cp.Product.CPV,
                    CMV = cp.Product.CMV,
                    SuggestedPrice = cp.Product.SuggestedPrice,
                    Markup = cp.Product.Markup,
                    Margin = cp.Product.Margin,
                    ImageUrl = cp.Product.ImageUrl,
                    EnterpriseId = cp.Product.EnterpriseId

                }).ToList()
            };

            return comboDTO;
        }

        public async Task<List<ComboDTO>> GetAllCombosByEnterpriseIdAsync(Guid enterpriseId)
        {
            var combos = await _context.Set<ComboEntity>()
                .Where(x => x.EnterpriseId == enterpriseId)
                .Include(x => x.Products)
                    .ThenInclude(cp => cp.Product)
                        .ThenInclude(p => p.ProductIngredients)
                            .ThenInclude(pi => pi.Ingredient)
                                .ThenInclude(i => i.UnitOfMeasure)
                .Select(x => new ComboDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    ImageUrl = x.ImageUrl,
                    SellingPrice = x.SellingPrice,
                    SugestedPrice = x.SugestedPrice,
                    CPV = x.CPV,
                    CMV = x.CMV,
                    Markup = x.Markup,
                    EnterpriseId = x.EnterpriseId,
                    Products = x.Products.Select(cp => new ProductDTO
                    {
                        Id = cp.Product.Id,
                        Name = cp.Product.Name,
                        Description = cp.Product.Description,
                        SellingPrice = cp.Product.SellingPrice,
                        DesiredMargin = cp.Product.DesiredMargin,
                        CPV = cp.Product.CPV,
                        CMV = cp.Product.CMV,
                        SuggestedPrice = cp.Product.SuggestedPrice,
                        Markup = cp.Product.Markup,
                        Margin = cp.Product.Margin,
                        ImageUrl = cp.Product.ImageUrl,
                        EnterpriseId = cp.Product.EnterpriseId,
                        Ingredients = cp.Product.ProductIngredients.Select(pi => new ProductIngredientDTO
                        {
                            Id = pi.IngredientId,
                            Name = pi.Ingredient.Name,
                            Quantity = pi.QuantityUsedInBase,
                            BaseUnit = pi.Ingredient.UnitOfMeasure.BaseUnit
                        }).ToList()
                    }).ToList()
                }).ToListAsync();

            return combos;
        }

        public async Task<Guid> CreateComboAsync(ComboDTO dto)
        {
            decimal CPV = 0;

            foreach (var item in dto.Products)
            {
                var product = await _UnityOfWork.ProductRepository.GetByIdAsync(item.Id);
                if (product == null)
                    throw new Exception($"Product not found: {item.Id}");

                CPV += product.CPV ?? 0;
            }

            decimal? CMV = (dto.SellingPrice.HasValue && dto.SellingPrice.Value != 0)
                ? Math.Round((CPV / dto.SellingPrice.Value) * 100, 2)
                : null;

            decimal? Markup = (CPV > 0 && dto.SellingPrice.HasValue)
                ? Math.Round(((dto.SellingPrice.Value / CPV) - 1m), 2)
                : null;

            decimal? SuggestedPrice = (dto.DesiredMargin.HasValue && dto.DesiredMargin.Value < 100m)
                ? Math.Round(CPV / (1m - (dto.DesiredMargin.Value / 100m)), 2)
                : null;

            decimal? Margin = (dto.SellingPrice.HasValue && dto.SellingPrice.Value > 0)
                ? Math.Round(((dto.SellingPrice.Value - CPV) / dto.SellingPrice.Value) * 100, 2)
                : null;

            var combo = new ComboEntity
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                ImageUrl = dto.ImageUrl,
                Description = dto.Description,
                EnterpriseId = dto.EnterpriseId,
                SellingPrice = dto.SellingPrice,
                DesiredMargin = dto.DesiredMargin,
                CPV = CPV,
                CMV = CMV,
                SugestedPrice = SuggestedPrice,
                Markup = Markup,
                Margin = Margin,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow,
                IsActive = true
            };

            await _UnityOfWork.ComboRepository.Create(combo);

            foreach (var item in dto.Products)
            {
                var comboProduct = new ComboProductEntity
                {
                    Id = Guid.NewGuid(),
                    ComboId = combo.Id,
                    ProductId = item.Id,
                    DateCreated = DateTime.UtcNow,
                    DateUpdated = DateTime.UtcNow,
                    IsActive = true
                };

                await _UnityOfWork.ComboProductRepository.Create(comboProduct);
            }

            await _UnityOfWork.CommitAsync();
            return combo.Id;
        }

        public async Task UpdateComboAsync(ComboDTO dto)
        {
            var combo = await _UnityOfWork.ComboRepository.GetByIdAsync(dto.Id);
            if (combo == null)
                throw new Exception("Combo not found");

            decimal CPV = 0;

            await _UnityOfWork.ComboProductRepository.RemoveAllByComboIdAsync(combo.Id);

            foreach (var item in dto.Products)
            {
                var product = await _UnityOfWork.ProductRepository.GetByIdAsync(item.Id);
                if (product == null)
                    throw new Exception($"Product not found: {item.Id}");

                CPV += product.CPV ?? 0;

                var comboProduct = new ComboProductEntity
                {
                    Id = Guid.NewGuid(),
                    ProductId = product.Id,
                    ComboId = combo.Id,
                    DateCreated = DateTime.UtcNow,
                    DateUpdated = DateTime.UtcNow,
                    IsActive = true
                };

                await _UnityOfWork.ComboProductRepository.Create(comboProduct);
            }

            decimal? CMV = (dto.SellingPrice.HasValue && dto.SellingPrice.Value != 0)
                ? Math.Round((CPV / dto.SellingPrice.Value) * 100, 2)
                : null;

            decimal? Markup = (CPV > 0 && dto.SellingPrice.HasValue)
                ? Math.Round(((dto.SellingPrice.Value / CPV) - 1m), 2)
                : null;

            decimal? SuggestedPrice = (dto.DesiredMargin.HasValue && dto.DesiredMargin.Value < 100m)
                ? Math.Round(CPV / (1m - (dto.DesiredMargin.Value / 100m)), 2)
                : null;

            decimal? Margin = (dto.SellingPrice.HasValue && dto.SellingPrice.Value > 0)
                ? Math.Round(((dto.SellingPrice.Value - CPV) / dto.SellingPrice.Value) * 100, 2)
                : null;

            combo.Name = dto.Name;
            combo.Description = dto.Description;
            combo.SellingPrice = dto.SellingPrice;
            combo.DesiredMargin = dto.DesiredMargin;
            combo.CPV = CPV;
            combo.CMV = CMV;
            combo.SugestedPrice = SuggestedPrice;
            combo.Markup = Markup;
            combo.Margin = Margin;
            combo.EnterpriseId = dto.EnterpriseId;
            combo.DateUpdated = DateTime.UtcNow;

            if (!string.IsNullOrEmpty(dto.ImageUrl))
                combo.ImageUrl = dto.ImageUrl;

            _UnityOfWork.ComboRepository.Update(combo);
        }

    }
}
