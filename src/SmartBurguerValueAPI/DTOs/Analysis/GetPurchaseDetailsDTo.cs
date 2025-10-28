namespace SmartBurguerValueAPI.DTOs.Analysis
{
    public class GetPurchaseDetailsDTO
    {
        public int QuantityPurchases { get; set; }
        public decimal TotalSpent { get; set; }
        public decimal ProjectedExpensesNextMonth { get; set; }
        public List<string> TopSuppliers { get; set; }
    }
}
