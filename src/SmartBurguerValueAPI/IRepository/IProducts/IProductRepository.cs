using SmartBurguerValueAPI.DTOs.Products;
using SmartBurguerValueAPI.Models.Products;

namespace SmartBurguerValueAPI.IRepository.IProducts
{
    public interface IProductRepository 
    {
        IEnumerable<ProductsEntity> GetAllProducts();
        ProductsEntity GetProductById(Guid Id);
        ProductsDTO CreateProduct(ProductsDTO Product);
        ProductsDTO UpdateProduct(ProductsDTO Product);
        ProductsEntity DeleteProduct(Guid Id);
    }
}
