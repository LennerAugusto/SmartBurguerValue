using SmartBurguerValueAPI.IRepository.IRepositoryBase;
using SmartBurguerValueAPI.Models.Products;

namespace SmartBurguerValueAPI.Interfaces.IProducts
{
    public interface IProductIngredientsRepository : IRepositoryBase<ProductsIngredientsEntity>
    {
        Task RemoveAllByProductIdAsync(Guid productId);
        Task AddAsync(ProductsIngredientsEntity productIngredient);
    }
}
