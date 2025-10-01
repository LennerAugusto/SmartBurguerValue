namespace SmartBurguerValueAPI.DTOs.Analysis
{
    public class InvoicingSeriesDTO
    {
        public string? Label { get; set; }  
        public decimal? Invoicing { get; set; } 
        public decimal? NetValue { get; set; }
    }
    public class InvoicingResponseDTO
    {
        public List<InvoicingSeriesDTO>? Current { get; set; } = new();
        public List<InvoicingSeriesDTO>? Previous { get; set; } = new();
        public int? StatusCode { get; set; }
    }
}

