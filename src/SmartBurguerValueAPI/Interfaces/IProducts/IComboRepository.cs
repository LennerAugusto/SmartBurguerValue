using SmartBurguerValueAPI.DTOs.Products;
using SmartBurguerValueAPI.IRepository.IRepositoryBase;
using SmartBurguerValueAPI.Models.Products;
using SmartBurguerValueAPI.Pagination;


namespace SmartBurguerValueAPI.Interfaces.IProducts
{
    public interface IComboRepository : IRepositoryBase<ComboEntity>
    {
        Task<PagedList<ComboDTO>> GetAllCombosByEnterpriseIdAsync(PaginationParamiters paramiters, Guid EnterpriseId);
        Task<Guid> CreateComboAsync(ComboDTO dto);
        Task UpdateComboAsync(ComboDTO dto);
    }
}
