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
    //[Authorize(Policy = "Enterprise")]
    [Microsoft.AspNetCore.Mvc.Route("api/purchase")]
    public class PurchaseController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IUnityOfWork _unityOfWork;

        public PurchaseController(AppDbContext context, IUnityOfWork unityOfWork)
        {
            _context = context;
            _unityOfWork = unityOfWork;
        }
        [HttpGet("get-all/by-enterprise-id")]
        public async Task<ActionResult<IEnumerable<PurchaseDTO>>> GetAllPurchaseByEnterprise(Guid EnterpriseId)
        {
            var Purchases = await _unityOfWork.PurchaseRepository.GetAllPurchasesByEnterpriseId(EnterpriseId);
            return Ok(Purchases);
        }

        [HttpGet("get-by-id/")]
        public async Task<IActionResult> GetPurchaseById(Guid PurchaseId)
        {
            var Purchase = await _unityOfWork.PurchaseRepository.GetByIdAsync(PurchaseId);
            return Ok(Purchase);
        }

        [HttpPost("create")]
        public async Task<ActionResult<PurchaseDTO>> CreatePurchase([FromBody] PurchaseDTO purchase)
        {
            var Purchase = await _unityOfWork.PurchaseRepository.CreatePurchase(purchase);
            await _unityOfWork.CommitAsync();
            return Ok(Purchase);
        }


        [HttpPut("update/")]
        public async Task<ActionResult> Put([FromBody] PurchaseEntity purchase)
        {
            _unityOfWork.PurchaseRepository.Update(purchase);
            await _unityOfWork.CommitAsync();
            return Ok(purchase);
        }

        [HttpDelete("delete/{id:guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var Purchase = await _unityOfWork.PurchaseRepository.GetByIdAsync(id);

            await _unityOfWork.PurchaseRepository.Delete(Purchase);
            await _unityOfWork.CommitAsync();
            return Ok(Purchase);
        }
    }
}
