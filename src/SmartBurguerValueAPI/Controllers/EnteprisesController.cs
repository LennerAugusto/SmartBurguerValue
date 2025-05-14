using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using SmartBurguerValueAPI.Context;
using SmartBurguerValueAPI.DTOs.Products;
using SmartBurguerValueAPI.DTOs;
using SmartBurguerValueAPI.Interfaces;
using SmartBurguerValueAPI.Models.Products;
using SmartBurguerValueAPI.Models;

namespace SmartBurguerValueAPI.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/enterprise")]
    public class EnteprisesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IUnityOfWork _unityOfWork;

        public EnteprisesController(AppDbContext context, IUnityOfWork unityOfWork)
        {
            _context = context;
            _unityOfWork = unityOfWork;
        }
        [HttpGet("get-all")]
        public async Task<ActionResult<IEnumerable<EnterpriseDTO>>> GetAllEnterprises()
        {
            var Enterprises = await _unityOfWork.EnterpriseRepository.GetAll();
            return Ok(Enterprises);
        }

        [HttpGet("get-by-id/")]
        public async Task<IActionResult> GetEnterpriseById(Guid EnterpriseId)
        {
            var Enterprise = _unityOfWork.EnterpriseRepository.GetById(EnterpriseId);
            return Ok(Enterprise);
        }

        [HttpPost("create")]
        public async Task<ActionResult<EnterpriseDTO>> CreateEnterprise([FromBody] EnterpriseEntity enterprise)
        {
            var Enterprise = _unityOfWork.EnterpriseRepository.Create(enterprise);
            _unityOfWork.Commit();
            return Ok(Enterprise);
        }


        [HttpPut("update/")]
        public ActionResult Put([FromBody] EnterpriseEntity enterprise)
        {
            var Enterprise = _unityOfWork.EnterpriseRepository.Update(enterprise);
            _unityOfWork.Commit();
            return Ok(Enterprise);
        }

        [HttpDelete("delete/{id:guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var Enterprise = await _unityOfWork.EnterpriseRepository.GetById(id);

            await _unityOfWork.EnterpriseRepository.Delete(Enterprise);
            _unityOfWork.Commit();
            return Ok(Enterprise);
        }
    }
}
