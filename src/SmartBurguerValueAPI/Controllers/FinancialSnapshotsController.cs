using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartBurguerValueAPI.Context;
using SmartBurguerValueAPI.Interfaces;
using SmartBurguerValueAPI.Models;

namespace SmartBurguerValueAPI.Controllers
{
    [Authorize(Policy = "Enterprise")]
    [Microsoft.AspNetCore.Mvc.Route("api/financial-snapshots")]
    public class FinancialSnapshotsController: ControllerBase
    {
        private readonly IUnityOfWork _unityOfWork;
        private readonly AppDbContext _context;

        public FinancialSnapshotsController(IUnityOfWork unityOfWork, AppDbContext context)
        {
            _unityOfWork = unityOfWork;
            _context = context;
        }
        [HttpGet("get-financial-snapshot/by-date")]
        public async Task<FinancialSnapshotEntity> GetAllFinancialSnapshots([FromQuery] DateTime date, [FromQuery] Guid enterpriseId)
        {
            var Analyse = await _unityOfWork.FinancialSnapshotsRepository.GetAnalyse(date, enterpriseId);
            return Analyse;
        }
    }
}
