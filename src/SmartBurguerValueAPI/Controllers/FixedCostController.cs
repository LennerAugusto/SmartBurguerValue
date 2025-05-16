using Microsoft.AspNetCore.Mvc;
using SmartBurguerValueAPI.Context;
using SmartBurguerValueAPI.DTOs;
using SmartBurguerValueAPI.Interfaces;
using SmartBurguerValueAPI.Migrations;
using SmartBurguerValueAPI.Models;

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
            var FixedCoasts = await _unityOfWork.FixedCoastRepository.GetAll();
            return Ok(FixedCoasts);
        }
        [HttpGet("get-all/by-enterprise-id")]
        public async Task<ActionResult<IEnumerable<FixedCoastDTO>>> GetAllFixedCoastByEnterpriseId(Guid enterpriseId)
        {
            var FixedCoasts = await _unityOfWork.FixedCoastRepository.GetAllFixedCostByEnterpriseId(enterpriseId);
            return Ok(FixedCoasts);
        }
        [HttpGet("get-by-id/")]
        public async Task<IActionResult> GetFixedCoastById(Guid FixedCoastId)
        {
            var FixedCoast = _unityOfWork.FixedCoastRepository.GetById(FixedCoastId);
            return Ok(FixedCoast);
        }
        
        [HttpPost("create")]
        public async Task<ActionResult<FixedCoastDTO>> CreateFixedCoast([FromBody] FixedCostEntity fixedCoast)
        {
            var FixedCoast = _unityOfWork.FixedCoastRepository.Create(fixedCoast);
            _unityOfWork.Commit();
            return Ok(FixedCoast);
        }


        [HttpPut("update/")]
        public ActionResult Put([FromBody] FixedCostEntity fixedCoast)
        {
            var FixedCoast = _unityOfWork.FixedCoastRepository.Update(fixedCoast);
            _unityOfWork.Commit();
            return Ok(FixedCoast);
        }

        [HttpDelete("delete/{id:guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var FixedCoast = await _unityOfWork.FixedCoastRepository.GetById(id);

            await _unityOfWork.FixedCoastRepository.Delete(FixedCoast);
            _unityOfWork.Commit();
            return Ok(FixedCoast);
        }
    }
}
