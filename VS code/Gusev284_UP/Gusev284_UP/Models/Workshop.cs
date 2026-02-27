using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gusev284_UP.Models
{
    [Table("Workshops")]
    public class Workshop
    {
        [Key]
        public int WorkshopId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string Type { get; set; } // тип цеха (например, "Обработка", "Сборка")

        [Required]
        public int EmployeeCount { get; set; } // количество человек для производства

        // Навигационное свойство: связь с продуктами через промежуточную таблицу
        public ICollection<ProductWorkshop> ProductWorkshops { get; set; }
    }
}