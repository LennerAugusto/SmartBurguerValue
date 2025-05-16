namespace SmartBurguerValueAPI.DTOs
{
    public class FixedCoastDTO
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Value { get; set; }
        public Guid EnterpriseId { get; set; }
    }
}
