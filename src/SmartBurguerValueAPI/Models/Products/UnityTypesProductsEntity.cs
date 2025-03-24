using System.ComponentModel.DataAnnotations.Schema;

namespace SmartBurguerValueAPI.Models.Products
{
    [Table("UnitytypesProducts")]
    public class UnityTypesProductsEntity : BaseEntity
    {
        public string Name { get; set; }
        public ICollection<ProductsEntity> Products { get; set; } = new List<ProductsEntity>();
    }
}
