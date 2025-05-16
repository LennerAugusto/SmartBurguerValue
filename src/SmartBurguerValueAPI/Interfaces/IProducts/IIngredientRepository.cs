using SmartBurguerValueAPI.DTOs.Products;
using SmartBurguerValueAPI.IRepository.IRepositoryBase;
using SmartBurguerValueAPI.Models.Products;

namespace SmartBurguerValueAPI.Interfaces.IProducts
{
    public interface IIngredientRepository : IRepositoryBase<IngredientsEntity>
    {
        Task<IEnumerable<IngredientDTO>> GetAllIngredientByEnterpriseId(Guid EnterpriseId);
    }
}
