using SmartBurguerValueAPI.DTOs;
using SmartBurguerValueAPI.DTOs.Products;
using SmartBurguerValueAPI.IRepository.IRepositoryBase;
using SmartBurguerValueAPI.Models;
using SmartBurguerValueAPI.Models.Products;

namespace SmartBurguerValueAPI.IRepository.IProducts
{
    public interface IProductRepository : IRepositoryBase<ProductsEntity>
    {
        Task<IEnumerable<ProductDTO>> GetAllProductsByEnterpriseId(Guid EnterpriseId);
        Task<Guid> CreateProductAsync(ProductDTO dto);
        Task UpdateProductAsync(ProductDTO dto);
    }
}
