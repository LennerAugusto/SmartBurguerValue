using Microsoft.AspNetCore.Mvc;
using SmartBurguerValueAPI.DTOs;
using SmartBurguerValueAPI.IRepository.IRepositoryBase;
using SmartBurguerValueAPI.Models;
using SmartBurguerValueAPI.Pagination;

namespace SmartBurguerValueAPI.Interfaces
{
    public interface IEmployeeRepository : IRepositoryBase<EmployeeEntity>
    {
        Task  CreateEmployee(EmployeeDTO employee);
        Task UpdateEmployee(EmployeeDTO employee);
        Task<List<EmployeeDTO>> GetAllEmployeeByEnterpriseId(Guid enterpriseId);

    }
}
