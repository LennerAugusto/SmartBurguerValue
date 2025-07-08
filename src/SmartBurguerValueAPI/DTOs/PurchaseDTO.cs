namespace SmartBurguerValueAPI.DTOs
{
    public class PurchaseDTO : BaseDTO
    {
        public Guid Id { get; set; }
        public string SupplierName { get; set; }
        public DateTime PurchaseDate { get; set; }
        public decimal TotalAmount { get; set; }
        public Guid EnterpriseId { get; set; }
        public List<PurchaseItemDTO> PurchaseItems { get; set; }
    }
}
