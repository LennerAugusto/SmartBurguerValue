using SmartBurguerValueAPI.DTOs.Products;
using SmartBurguerValueAPI.IRepository.IRepositoryBase;
using SmartBurguerValueAPI.Models.Products;
using SmartBurguerValueAPI.Pagination;

namespace SmartBurguerValueAPI.Interfaces.IProducts
{
    public interface IIngredientRepository : IRepositoryBase<IngredientsEntity>
    {
        Task<List<IngredientDTO>> GetAllIngredientByEnterpriseId( Guid EnterpriseId);
        Task<IngredientDTO> CreateIngredient(IngredientsEntity ingredient);
    }
}
