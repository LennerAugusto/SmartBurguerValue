namespace SmartBurguerValueAPI.DTOs.Products
{
    public class ComboDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public Guid EnterpriseId { get; set; }
        public List<ProductDTO> Products { get; set; }
    }
}
