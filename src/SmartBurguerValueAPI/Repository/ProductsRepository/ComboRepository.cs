using Microsoft.EntityFrameworkCore;
using SmartBurguerValueAPI.Context;
using SmartBurguerValueAPI.DTOs.Products;
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
        private readonly IComboProductRepository _comboProductRepository;
        private readonly IComboRepository _comboRepository;

        public ComboRepository(AppDbContext context) : base(context)
        {
        }
        public async Task<PagedList<ComboDTO>> GetAllCombosByEnterpriseId(PaginationParamiters paramiters, Guid enterpriseId)
        {
            var query = _context.Set<ComboEntity>()
                .Where(x => x.EnterpriseId == enterpriseId)
                .Include(x => x.ComboProducts)
                    .ThenInclude(cp => cp.Product)
                .AsNoTracking()
                .Select(x => new ComboDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    EnterpriseId = x.EnterpriseId,
                    Products = x.ComboProducts.Select(cp => new ProductDTO
                    {
                        Id = cp.Product.Id,
                        Name = cp.Product.Name,
                    }).ToList()
                });

            return PagedList<ComboDTO>.ToPagedList(query, paramiters.PageNumber, paramiters.PageSize);
        }

        public async Task<Guid> CreateComboAsync(ComboDTO dto)
        {
            var combo = new ComboEntity
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Description = dto.Description,
                EnterpriseId = dto.EnterpriseId,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow,
                IsActive = true
            };

            foreach (var item in dto.Products)
            {
                var product = await _productRepository.GetById(item.Id);
                if (product == null)
                    throw new Exception($"Product not found: {item.Id}");

                var comboProduct = new ComboProductEntity
                {
                    Id = Guid.NewGuid(),
                    ProductId = product.Id,
                    DateCreated = DateTime.UtcNow,
                    DateUpdated = DateTime.UtcNow,
                    IsActive = true
                };

                await _comboProductRepository.Create(comboProduct);
            }

            return combo.Id;
        }
        public async Task UpdateComboAsync(ComboDTO dto)
        {
            var combo = await _comboRepository.GetById(dto.Id);
            if (combo == null)
                throw new Exception("Combo not found");

            combo.Name = dto.Name;
            combo.Description = dto.Description;
            combo.DateUpdated = DateTime.UtcNow;

            await _comboRepository.Update(combo);

            await _comboProductRepository.RemoveAllByComboIdAsync(combo.Id);

            foreach (var item in dto.Products)
            {
                var products = await _productRepository.GetById(item.Id);
                if (products == null)
                    throw new Exception($"Products not found: {item.Id}");

                var comboProduct = new ComboProductEntity
                {
                    Id = Guid.NewGuid(),
                    ProductId = products.Id,
                    DateCreated = DateTime.UtcNow,
                    DateUpdated = DateTime.UtcNow,
                    IsActive = true
                };

                await _comboProductRepository.AddAsync(comboProduct);
            }
        }
    }
}
