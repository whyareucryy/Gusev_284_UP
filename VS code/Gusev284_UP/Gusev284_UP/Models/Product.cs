using Gusev284_UP.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gusev284_UP.Models
{
    [Table("Products")]
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required]
        public int ProductTypeId { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string Article { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal MinPriceForPartner { get; set; } // минимальная стоимость для партнёра

        [Required]
        public int MainMaterialId { get; set; } // основной материал

        // Опциональные поля (можно добавить, если нужно)
        public string? Description { get; set; }
        public byte[]? Image { get; set; }

        // Навигационные свойства
        [ForeignKey("ProductTypeId")]
        public ProductType ProductType { get; set; }

        [ForeignKey("MainMaterialId")]
        public MaterialType MainMaterial { get; set; }

        // Связь с цехами через промежуточную таблицу
        public ICollection<ProductWorkshop> ProductWorkshops { get; set; }
    }
}