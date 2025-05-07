
using SmartBurguerValueAPI.DTOs;
using SmartBurguerValueAPI.IRepository.IRepositoryBase;
using SmartBurguerValueAPI.Models;
using SmartBurguerValueAPI.Models.Products;

namespace SmartBurguerValueAPI.IRepository.IProducts
{
    public interface IUnityTypesRepository : IRepositoryBase<UnityTypesProductsEntity>
    {
        IEnumerable<BaseDTO> GetAllUnityTypesProducts();
        BaseDTO GetUnityTypeProductsById(Guid Id);
        BaseDTO CreateUnityTypesProducts(BaseDTO unityTypes);
        BaseDTO UpdateUnityTypesProducts(BaseDTO unityType);
        BaseDTO DeleteUnityTypeProducts(Guid Id);
    }
}
