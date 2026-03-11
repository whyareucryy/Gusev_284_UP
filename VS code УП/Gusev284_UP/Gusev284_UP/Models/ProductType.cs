using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gusev284_UP.Models
{
    [Table("ProductTypes")]
    public class ProductType
    {
        [Key]
        public int ProductTypeId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public double Coefficient { get; set; } // коэффициент типа продукции (вещественный)

        // Навигационное свойство: один тип может быть у многих продуктов
        public ICollection<Product> Products { get; set; }
    }
}
