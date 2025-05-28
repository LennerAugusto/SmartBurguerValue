using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SmartBurguerValueAPI.Context;
using SmartBurguerValueAPI.DTOs;
using SmartBurguerValueAPI.DTOs.Products;
using SmartBurguerValueAPI.Interfaces;
using SmartBurguerValueAPI.Migrations;
using SmartBurguerValueAPI.Models;
using SmartBurguerValueAPI.Pagination;

namespace SmartBurguerValueAPI.Controllers
{
    [Route("api/fixed-coast")]
    public class FixedCostController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IUnityOfWork _unityOfWork;

        public FixedCostController(AppDbContext context, IUnityOfWork unityOfWork)
        {
            _context = context;
            _unityOfWork = unityOfWork;
        }
        [HttpGet("get-all")]
        public async Task<ActionResult<IEnumerable<FixedCoastDTO>>> GetAllFixedCoasts()
        {
            var FixedCoasts = await _unityOfWork.FixedCoastRepository.GetAllAsync();
            return Ok(FixedCoasts);
        }
        [HttpGet("get-all/by-enterprise-id")]
        public async Task<ActionResult<IEnumerable<FixedCoastDTO>>> GetAllFixedCoastByEnterpriseId(PaginationParamiters paramiters, Guid EnterpriseId)
        {
            var FixedCoasts = await _unityOfWork.FixedCoastRepository.GetAllFixedCostByEnterpriseId(paramiters, EnterpriseId);

            var metadata = new
            {
                FixedCoasts.TotalCount,
                FixedCoasts.PageSize,
                FixedCoasts.CurrentPage,
                FixedCoasts.TotalPages,
                FixedCoasts.HasNext,
                FixedCoasts.HasPrevius
            };

            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

            return Ok(FixedCoasts);
        }
        [HttpGet("get-by-id/")]
        public async Task<IActionResult> GetFixedCoastById(Guid FixedCoastId)
        {
            var FixedCoast = _unityOfWork.FixedCoastRepository.GetByIdAsync(FixedCoastId);
            return Ok(FixedCoast);
        }
        
        [HttpPost("create")]
        public async Task<ActionResult<FixedCoastDTO>> CreateFixedCoast([FromBody] FixedCostEntity fixedCoast)
        {
            var FixedCoast = _unityOfWork.FixedCoastRepository.Create(fixedCoast);
            await _unityOfWork.CommitAsync();
            return Ok(FixedCoast);
        }


        [HttpPut("update/")]
        public ActionResult Put([FromBody] FixedCostEntity fixedCoast)
        {
            var FixedCoast = _unityOfWork.FixedCoastRepository.Update(fixedCoast);
            _unityOfWork.CommitAsync();
            return Ok(FixedCoast);
        }

        [HttpDelete("delete/{id:guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var FixedCoast = await _unityOfWork.FixedCoastRepository.GetByIdAsync(id);

            await _unityOfWork.FixedCoastRepository.Delete(FixedCoast);
            await _unityOfWork.CommitAsync();
            return Ok(FixedCoast);
        }
    }
}
