using SmartBurguerValueAPI.DTOs.Products;
using SmartBurguerValueAPI.IRepository.IRepositoryBase;
using SmartBurguerValueAPI.Models.Products;

namespace SmartBurguerValueAPI.Interfaces.IProducts
{
    public interface IComboRepository : IRepositoryBase<ComboEntity>
    {
        Task<IEnumerable<ComboDTO>> GetAllCombosByEnterpriseId(Guid EnterpriseId);
        Task<Guid> CreateComboAsync(ComboDTO dto);
        Task UpdateComboAsync(ComboDTO dto);
    }
}
