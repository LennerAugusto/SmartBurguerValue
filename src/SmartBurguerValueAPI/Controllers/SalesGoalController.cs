using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SmartBurguerValueAPI.Context;
using SmartBurguerValueAPI.DTOs;
using SmartBurguerValueAPI.DTOs.Products;
using SmartBurguerValueAPI.Interfaces;
using SmartBurguerValueAPI.Models;
using SmartBurguerValueAPI.Pagination;

namespace SmartBurguerValueAPI.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/sales-goal")]
    public class SalesGoalController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IUnityOfWork _unityOfWork;

        public SalesGoalController(AppDbContext context, IUnityOfWork unityOfWork)
        {
            _context = context;
            _unityOfWork = unityOfWork;
        }
        [HttpGet("get-all")]
        public async Task<ActionResult<IEnumerable<SalesGoalDTO>>> GetAllSalesGoal()
        {
            var SalesGoals = await _unityOfWork.SalesGoalRepository.GetAllAsync();
            return Ok(SalesGoals);
        }
        [HttpGet("get-all/by-enterprise-id")]
        public async Task<ActionResult<IEnumerable<SalesGoalDTO>>> GetAllSalesGoalByEnterprise(PaginationParamiters paramiters, Guid EnterpriseId)
        {
            var SalesGoal = await _unityOfWork.SalesGoalRepository.GetAllSalesGoalByEnterpriseId(paramiters, EnterpriseId);

            var metadata = new
            {
                SalesGoal.TotalCount,
                SalesGoal.PageSize,
                SalesGoal.CurrentPage,
                SalesGoal.TotalPages,
                SalesGoal.HasNext,
                SalesGoal.HasPrevius
            };

            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

            return Ok(SalesGoal);
        }

        [HttpGet("get-by-id/")]
        public async Task<IActionResult> GetSalesGoalById(Guid salesGoalId)
        {
            var SalesGoal = _unityOfWork.SalesGoalRepository.GetByIdAsync(salesGoalId);
            return Ok(SalesGoal);
        }

        [HttpPost("create")]
        public async Task<ActionResult<SalesGoalDTO>> CreateSalesGoal([FromBody] SalesGoalEntity salesGoal)
        {
            var SalesGoal = _unityOfWork.SalesGoalRepository.Create(salesGoal);
            await _unityOfWork.CommitAsync();
            return Ok(SalesGoal);
        }


        [HttpPut("update/")]
        public ActionResult Put([FromBody] SalesGoalEntity combo)
        {
            var SalesGoal = _unityOfWork.SalesGoalRepository.Create(combo);
            _unityOfWork.CommitAsync();
            return Ok(SalesGoal);
        }

        [HttpDelete("delete/{id:guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var SalesGoal = await _unityOfWork.SalesGoalRepository.GetByIdAsync(id);

            await _unityOfWork.SalesGoalRepository.Delete(SalesGoal);
            await _unityOfWork.CommitAsync();
            return Ok(SalesGoal);
        }
    }
}
