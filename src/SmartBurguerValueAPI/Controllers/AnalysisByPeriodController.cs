using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartBurguerValueAPI.Constants;
using SmartBurguerValueAPI.Context;
using SmartBurguerValueAPI.DTOs.Analysis;
using SmartBurguerValueAPI.DTOs.Products;
using SmartBurguerValueAPI.Interfaces;

namespace SmartBurguerValueAPI.Controllers
{
    //[Authorize(Policy = "Enterprise")]
    [Microsoft.AspNetCore.Mvc.Route("api/analysis")]
    public class AnalysisByPeriodController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IUnityOfWork _unityOfWork;

        public AnalysisByPeriodController(AppDbContext context, IUnityOfWork unityOfWork)
        {
            _context = context;
            _unityOfWork = unityOfWork;
        }

        [HttpPost("analysis-home/get-all")]
        public async Task<ActionResult<IEnumerable<InitialAnalysiDTO>>> GetAnalyseHome([FromBody]AnalysisRequestDTO analysis)
        {
            var InintialDetail = await _unityOfWork.AnalysisByPeriodRepository.GetInitialAnalysisByPeriod(analysis.Period, analysis.EnterpriseId);
            return Ok(InintialDetail);
        }
        [HttpPost("best-selling-products/by-enterprise-id")]
        public async Task<ActionResult<IEnumerable<BestSellingProductsDTO>>> GetCardsBestSellingProducts([FromBody] AnalysisRequestDTO analysis)
        {
            var InfoCards = await _unityOfWork.AnalysisByPeriodRepository.GetCardsBestSellingProductsByEnterpriseId(analysis.Period, analysis.EnterpriseId);
            return Ok(InfoCards);
        }
        [HttpPost("invoicing/by-enterprise-id")]
        public async Task<ActionResult<IEnumerable<InvoicingSeriesDTO>>> GetInvoicing([FromBody] AnalysisRequestDTO analysis)
        {
            var Invoicing = await _unityOfWork.AnalysisByPeriodRepository.GetInvoicingByPeriod(analysis.Period, analysis.EnterpriseId);
            return Ok(Invoicing);
        }
        [HttpPost("total-orders/by-enterprise-id")]
        public async Task<ActionResult<IEnumerable<TotalOrdersDTO>>> GetTotalOrders([FromBody] AnalysisRequestDTO analysis)
        {
            var TotalOrders = await _unityOfWork.AnalysisByPeriodRepository.GetTotalOrdersByPeriod(analysis.Period, analysis.EnterpriseId);
            return Ok(TotalOrders);
        }
        [HttpPost("get-margin-and-profit-products/by-enterprise-id")]
        public async Task<ActionResult<GetAnalysisCardsProductsDTO>> GetMarginAndProfitProduct([FromBody] AnalysisRequestDTO analysis)
        {
            var MarginAndProfitProduct = await _unityOfWork.AnalysisByPeriodRepository.GetMarginAndProfitProductByPeriod(analysis.Period, analysis.EnterpriseId);
            return Ok(MarginAndProfitProduct);
        }
        [HttpPost("get-margin-and-profit-combos/by-enterprise-id")]
        public async Task<ActionResult<GetAnalysisCardsProductsDTO>> GetMarginAndProfitCombo([FromBody] AnalysisRequestDTO analysis)
        {
            var MarginAndProfitCombo = await _unityOfWork.AnalysisByPeriodRepository.GetMarginAndProfitComboByPeriod(analysis.Period, analysis.EnterpriseId);
            return Ok(MarginAndProfitCombo);
        }
        [HttpPost("get-best-selling-combos/by-enterprise-id")]
        public async Task<ActionResult<List<BestSellingProductsByPeriodDTO>>> GetBestSellingCombos([FromBody] AnalysisRequestDTO analysis)
        {
            var BestSellingCombo = await _unityOfWork.AnalysisByPeriodRepository.GetBestSellingCombosByPeriod(analysis.Period, analysis.EnterpriseId);
            return Ok(BestSellingCombo);
        }
        [HttpPost("get-best-selling-products/by-enterprise-id")]
        public async Task<ActionResult<List<BestSellingProductsByPeriodDTO>>> GetBestSellingProducts([FromBody] AnalysisRequestDTO analysis)
        {
            var BestSellingProduct = await _unityOfWork.AnalysisByPeriodRepository.GetBestSellingProductsByPeriod(analysis.Period, analysis.EnterpriseId);
            return Ok(BestSellingProduct);
        }
        [HttpPost("products-best-margin/by-enterprise-id")]
        public async Task<ActionResult<List<ProductsBestMarginDTO>>> GetProductsBestMargin(Guid EnterpriseId)
        {
            var BestMarginProduct = await _unityOfWork.AnalysisByPeriodRepository.GetProductsBestMargin(EnterpriseId);
            return Ok(BestMarginProduct);
        }
        [HttpPost("combos-best-margin/by-enterprise-id")]
        public async Task<ActionResult<List<ProductsBestMarginDTO>>> GetCombosBestMargin(Guid EnterpriseId)
        {
            var BestMarginCombo = await _unityOfWork.AnalysisByPeriodRepository.GetCombosBestMargin(EnterpriseId);
            return Ok(BestMarginCombo);
        }
        [HttpPost("sales-distribution/by-enterprise-id")]
        public async Task<ActionResult<List<ProductsBestMarginDTO>>> GetSalesDistribution([FromBody] AnalysisRequestDTO analysis)
        {
            var SalesDistribution = await _unityOfWork.AnalysisByPeriodRepository.GetSalesDistributionByPeriod(analysis.Period, analysis.EnterpriseId);
            return Ok(SalesDistribution);
        }
        [HttpPost("markup-cmv-/by-enterprise-id")]
        public async Task<ActionResult<GetCmvMarkupInvoicingDTO>> GetCmvMarkupInvoicing([FromBody] AnalysisRequestDTO analysis)
        {
            var Get = await _unityOfWork.AnalysisByPeriodRepository.GetCmvMarkupInvoicingByPeriod(analysis.Period, analysis.EnterpriseId);
            return Ok(Get);
        }
        [HttpPost("purchase-details/by-enterprise-id")]
        public async Task<ActionResult<GetPurchaseDetailsDTO>> GetPurchaseDetails([FromBody] AnalysisRequestDTO analysis)
        {
            var PurchaseDetails  = await _unityOfWork.AnalysisByPeriodRepository.GetPurchaseDetailsByPeriod(analysis.Period, analysis.EnterpriseId);
            return Ok(PurchaseDetails);
        }
        [HttpPost("purchase-expanse/by-enterprise-id")]
        public async Task<ActionResult<GetPurchaseDetailsDTO>> GetPurchaseExpansse([FromBody] AnalysisRequestDTO analysis)
        {
            var PurchaseExpansse = await _unityOfWork.AnalysisByPeriodRepository.GetPurchaseExpanseByPeriod(analysis.Period, analysis.EnterpriseId);
            return Ok(PurchaseExpansse);
        }
    }
}