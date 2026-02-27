using Gusev284_UP.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gusev284_UP.Models
{
    [Table("ProductWorkshops")]
    public class ProductWorkshop
    {
        [Key]
        public int ProductWorkshopId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int WorkshopId { get; set; }

        [Required]
        public double TimeHours { get; set; } // время изготовления в этом цехе (часы)

        // Навигационные свойства
        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        [ForeignKey("WorkshopId")]
        public Workshop Workshop { get; set; }
    }
}