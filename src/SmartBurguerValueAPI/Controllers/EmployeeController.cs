using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SmartBurguerValueAPI.Context;
using SmartBurguerValueAPI.DTOs;
using SmartBurguerValueAPI.Interfaces;
using SmartBurguerValueAPI.Models;
using SmartBurguerValueAPI.Models.Products;
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
            var createdEmployee = _unityOfWork.EmployeeRepository.CreateEmployee(employee);
            await _unityOfWork.CommitAsync();
            return Ok(createdEmployee);
        }

        [HttpGet("get-by-id/")]
        public async Task<IActionResult> GetEmployeeById(Guid EmployeeId)
        {
            var Employee = await _unityOfWork.EmployeeRepository.GetByIdAsync(EmployeeId);
            return Ok(Employee);
        }
        [HttpGet("get-all")]
        public async Task<ActionResult<IEnumerable<EmployeeDTO>>> GetAllEmployees()
        {
            var Employees = await _unityOfWork.EmployeeRepository.GetAllAsync();
            return Ok(Employees);
        }
        [HttpGet("get-all/by-enterprise-id")]
        public async Task<ActionResult<IEnumerable<EmployeeDTO>>> GetAllEmployeeByEnterpriseId(Guid EnterpriseId)
        {
            var Employees = await _unityOfWork.EmployeeRepository.GetAllEmployeeByEnterpriseId(EnterpriseId);
            return Ok(Employees);
        }
        [HttpPut("update/")]
        public async Task<IActionResult> Put([FromBody] EmployeeDTO employee)
        {
            await _unityOfWork.EmployeeRepository.UpdateEmployee(employee);
            await _unityOfWork.CommitAsync();

            var updateemployee = await _unityOfWork.EmployeeRepository.GetByIdAsync(employee.Id);
            return Ok(updateemployee);
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
