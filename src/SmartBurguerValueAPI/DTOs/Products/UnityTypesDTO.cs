namespace SmartBurguerValueAPI.DTOs.Products
{
    public class UnityTypesDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public string BaseUnit { get; set; }
        public decimal ConversionFactor { get; set; }
    }
}
