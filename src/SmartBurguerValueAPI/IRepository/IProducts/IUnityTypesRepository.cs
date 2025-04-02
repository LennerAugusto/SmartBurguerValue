
using SmartBurguerValueAPI.DTOs;

namespace SmartBurguerValueAPI.IRepository.IProducts
{
    public interface IUnityTypesRepository
    {
        IEnumerable<BaseDTO> GetAllUnityTypesProducts();
        BaseDTO GetUnityTypeProductsById(Guid Id);
        BaseDTO CreateUnityTypesProducts(BaseDTO unityTypes);
        BaseDTO UpdateUnityTypesProducts(BaseDTO unityType);
        BaseDTO DeleteUnityTypeProducts(Guid Id);
    }
}
