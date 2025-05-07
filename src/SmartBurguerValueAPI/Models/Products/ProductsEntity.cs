using System.ComponentModel.DataAnnotations.Schema;

namespace SmartBurguerValueAPI.Models.Products
{
    [Table("Products")]
    public class ProductsEntity : BaseEntity
    {
        public string Name { get; set; }
        public Guid CategoryId { get; set; }
        public Guid UnityTypeId {get; set;}
        public CategoryProductsEntity Category { get; set; }
        public UnityTypesProductsEntity UnityTypes { get; set; }
        public float QuantityPerPackage { get; set; }
        public float ValuePerPackage { get; set; }
        public float UnityValue { get; set; }
        public EnterpriseEntity Enterprise { get; set; }
    }
}
