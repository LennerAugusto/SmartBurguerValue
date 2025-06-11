using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SmartBurguerValueAPI.Context;
using SmartBurguerValueAPI.DTOs;
using SmartBurguerValueAPI.Interfaces;
using SmartBurguerValueAPI.Models;
using SmartBurguerValueAPI.Pagination;

namespace SmartBurguerValueAPI.Controllers
{
    [Route("api/employee")]
    public class EmployeeController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IUnityOfWork _unityOfWork;

        public EmployeeController(AppDbContext context, IUnityOfWork unityOfWork)
        {
            _context = context;
            _unityOfWork = unityOfWork;
        }
        [HttpPost("create")]
        public async Task<ActionResult<EmployeeDTO>> CreateEmployee([FromBody] EmployeeDTO employee)
        {
            var Employee = _unityOfWork.EmployeeRepository.CreateEmployee(employee);
            await _unityOfWork.CommitAsync();
            return Ok(Employee);
        }
        [HttpGet("get-by-id/")]
        public async Task<IActionResult> GetEmployeeById(Guid EmployeeId)
        {
            var Employee = _unityOfWork.EmployeeRepository.GetByIdAsync(EmployeeId);
            return Ok(Employee);
        }
        [HttpGet("get-all")]
        public async Task<ActionResult<IEnumerable<EmployeeDTO>>> GetAllEmployees()
        {
            var Employees = await _unityOfWork.EmployeeRepository.GetAllAsync();
            return Ok(Employees);
        }
        [HttpGet("get-all/by-enterprise-id")]
        public async Task<ActionResult<IEnumerable<EmployeeDTO>>> GetAllEmployeeByEnterpriseId(PaginationParamiters paramiters, Guid EnterpriseId)
        {
            var Employees = await _unityOfWork.EmployeeRepository.GetAllEmployeeByEnterpriseId(paramiters, EnterpriseId);

            var metadata = new
            {
                Employees.TotalCount,
                Employees.PageSize,
                Employees.CurrentPage,
                Employees.TotalPages,
                Employees.HasNext,
                Employees.HasPrevius
            };

            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

            return Ok(Employees);
        }
        [HttpDelete("delete/{id:guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var Employee = await _unityOfWork.EmployeeRepository.GetByIdAsync(id);

            await _unityOfWork.EmployeeRepository.Delete(Employee);
            await _unityOfWork.CommitAsync();
            return Ok(Employee);
        }
    }
}
