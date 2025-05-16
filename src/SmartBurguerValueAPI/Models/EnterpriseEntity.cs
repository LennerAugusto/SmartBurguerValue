using SmartBurguerValueAPI.Models.Products;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace SmartBurguerValueAPI.Models
{
    [Table("Enterprise")]
    public class EnterpriseEntity : BaseEntity
    {
        public string Name { get; set; }
        public string CpfCnpj {get;set;}
        public string? PhoneNumber { get; set; }
        public string? State { get; set; }
        public string? City { get; set; }
        public string? Street { get; set; }
        public string? Number { get; set; }
        public string? CEP { get; set; }
        public string? Plan { get; set; }
        //public ICollection<UsersEntity> Users { get; set; }
        [JsonIgnore]
        public ICollection<IngredientsEntity> Ingredients { get; set; }
        [JsonIgnore]
        public ICollection<ProductsEntity> Products { get; set; }
        [JsonIgnore]
        public ICollection<ComboEntity> Combos { get; set; }
    }
}
