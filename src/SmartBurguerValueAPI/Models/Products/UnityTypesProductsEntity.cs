using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SmartBurguerValueAPI.Models.Products
{
    [Table("UnitytypesProducts")]
    public class UnityTypesProductsEntity : BaseEntity
    {
        public string Name { get; set; }
        public string Symbol { get; set; }
        public string BaseUnit { get; set; }
        public decimal ConversionFactor { get; set; }
        [JsonIgnore]
        public ICollection<IngredientsEntity> Ingredients { get; set; }
    }
}
