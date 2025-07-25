using SmartBurguerValueAPI.DTOs.Products;
using SmartBurguerValueAPI.IRepository.IRepositoryBase;
using SmartBurguerValueAPI.Models;
using SmartBurguerValueAPI.Models.Products;
using SmartBurguerValueAPI.Pagination;

namespace SmartBurguerValueAPI.IRepository.IProducts
{
    public interface IProductRepository : IRepositoryBase<ProductsEntity>
    {
        Task<List<ProductDTO>> GetAllProductsByEnterpriseId(Guid EnterpriseId);
        Task<Guid> CreateProductAsync(ProductDTO dto);
        Task UpdateProductAsync(ProductDTO dto);
    }
}
