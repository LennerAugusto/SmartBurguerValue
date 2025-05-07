using SmartBurguerValueAPI.DTOs.Products;
using SmartBurguerValueAPI.IRepository.IRepositoryBase;
using SmartBurguerValueAPI.Models;
using SmartBurguerValueAPI.Models.Products;

namespace SmartBurguerValueAPI.IRepository.IProducts
{
    public interface IProductRepository : IRepositoryBase<ProductsEntity>
    {
        IEnumerable<ProductsEntity> GetAllProducts();
        ProductsEntity GetProductById(Guid Id);
        ProductsDTO CreateProduct(ProductsDTO Product);
        ProductsDTO UpdateProduct(ProductsDTO Product);
        ProductsEntity DeleteProduct(Guid Id);
    }
}
