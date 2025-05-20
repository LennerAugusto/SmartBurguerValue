namespace SmartBurguerValueAPI.DTOs
{
    public class FixedCoastDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Value { get; set; }
        public Guid EnterpriseId { get; set; }
    }
}
