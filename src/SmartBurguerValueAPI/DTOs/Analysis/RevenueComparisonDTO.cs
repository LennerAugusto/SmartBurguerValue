namespace SmartBurguerValueAPI.DTOs.Analysis
{
    public class RevenueComparisonDTO
    {
        public List<RevenueSeriesDTO> Series { get; set; } = new();
    }

    public class RevenueSeriesDTO
    {
      
        public string Name { get; set; } = string.Empty;
        public List<decimal> Number { get; set; } = new();
    }
}
