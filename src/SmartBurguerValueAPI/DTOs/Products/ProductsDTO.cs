using SmartBurguerValueAPI.Models.Products;

namespace SmartBurguerValueAPI.DTOs.Products
{
    public class  ProductsDTO : BaseDTO
    {
        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }
        public Guid UnityTypeId { get; set; }
        public float QuantityPerPackage { get; set; }
        public float ValuePerPackage { get; set; }
        public float UnityValue { get; set; }
    }
}
