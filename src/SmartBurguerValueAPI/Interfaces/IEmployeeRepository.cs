using Microsoft.AspNetCore.Mvc;
using SmartBurguerValueAPI.DTOs;
using SmartBurguerValueAPI.IRepository.IRepositoryBase;
using SmartBurguerValueAPI.Models;
using SmartBurguerValueAPI.Pagination;

namespace SmartBurguerValueAPI.Interfaces
{
    public interface IEmployeeRepository : IRepositoryBase<EmployeeEntity>
    {
        Task CreateEmployee(EmployeeDTO employee);
        Task EmployeeUpdate(EmployeeDTO employee);
        Task<PagedList<EmployeeDTO>> GetAllEmployeeByEnterpriseId(PaginationParamiters paramiters, Guid enterpriseId);

    }
}
