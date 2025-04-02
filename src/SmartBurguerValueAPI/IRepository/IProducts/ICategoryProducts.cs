using SmartBurguerValueAPI.DTOs;
using SmartBurguerValueAPI.DTOs.Products;
using SmartBurguerValueAPI.Models.Products;
using SmartBurguerValueAPI.ReadModels;

namespace SmartBurguerValueAPI.IRepository.IProducts
{
    public interface ICategoryProducts
    {
       IEnumerable<CategoryProductsReadModel> GetAllCategoryProducts();
       BaseDTO GetCategoryProductsById(Guid id);
       CategoryProductsDTO CreateCategoryProducts(CategoryProductsDTO category);
       CategoryProductsDTO UpdateCategoryProducts(CategoryProductsDTO category);
       CategoryProductsDTO DeleteCategoryProducts(Guid id);
        
    }
}
