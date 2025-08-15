using SmartBurguerValueAPI.Constants;

namespace SmartBurguerValueAPI.DTOs.Analysis
{
    public class AnalysisRequestDTO
    {
        public EPeriod Period { get; set; }
        public Guid EnterpriseId { get; set; }
    }
}
