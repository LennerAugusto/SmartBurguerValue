using DTOs.Analysis;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartBurguerValueAPI.Context;
using SmartBurguerValueAPI.DTOs.Analysis;
using SmartBurguerValueAPI.Interfaces;

namespace SmartBurguerValueAPI.Controllers
{
    [Authorize(Policy = "Enterprise")]
    [Microsoft.AspNetCore.Mvc.Route("api/analysis-year")]
    public class AnalysisByPeriodYearController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IUnityOfWork _unityOfWork;

        public AnalysisByPeriodYearController(AppDbContext context, IUnityOfWork unityOfWork)
        {
            _context = context;
            _unityOfWork = unityOfWork;
        }
        [HttpPost("analysis-invoicing/by-enterprise-id")]
        public async Task<ActionResult<RevenueComparisonDTO>> GetAnalyseInvoicing([FromBody] RequestAnalisysYearsDTO analysis)
        {
            var Combos = await _unityOfWork.AnalysisByPeriodYearsRepository.GetRevenueComparisonAsync(analysis.PeriodYear, analysis.EnterpriseId);
            return Ok(Combos);
        }
        [HttpPost("analysis-orders/by-enterprise-id")]
        public async Task<ActionResult<RevenueComparisonDTO>> GetAnalyseOrders([FromBody] RequestAnalisysYearsDTO analysis)
        {
            var Combos = await _unityOfWork.AnalysisByPeriodYearsRepository.GetOrdersComparisonAsync(analysis.PeriodYear, analysis.EnterpriseId);
            return Ok(Combos);
        }
    }
}
