using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gusev284_UP.Models
{
    [Table("MaterialTypes")]
    public class MaterialType
    {
        [Key]
        public int MaterialTypeId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public double LossPercent { get; set; } // процент потерь сырья

        // Навигационное свойство: один тип материала может быть основным для многих продуктов
        public ICollection<Product> Products { get; set; }
    }
}