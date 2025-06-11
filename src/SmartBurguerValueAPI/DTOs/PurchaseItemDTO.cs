namespace SmartBurguerValueAPI.DTOs
{
    public class PurchaseItemDTO
    {
        public string NameItem { get; set; }
        public Guid PurchaseId { get; set; }
        public Guid UnityOfMeasureId { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Quantity { get; set; }
    }
}
