namespace SmartBurguerValueAPI.Models
{
    public class PurchaseEntity : BaseEntity
    {

        public string SupplierName { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime PurchaseDate { get; set; }
        public Guid EnterpriseId { get; set; }
        public EnterpriseEntity Enterprise { get; set; }
        public ICollection<PurchaseItemEntity> Items { get; set; }
    }
}
