using System.ComponentModel.DataAnnotations;

namespace Gusev284_UP.ViewModels
{
    public class ProductFormViewModel
    {
        public int ProductId { get; set; } // 0 для нового продукта

        [Required(ErrorMessage = "Артикул обязателен")]
        [StringLength(50, ErrorMessage = "Артикул не может быть длиннее 50 символов")]
        public string Article { get; set; }

        [Required(ErrorMessage = "Тип продукции обязателен")]
        [Display(Name = "Тип продукции")]
        public int ProductTypeId { get; set; }

        [Required(ErrorMessage = "Наименование обязательно")]
        [StringLength(200, ErrorMessage = "Наименование не может быть длиннее 200 символов")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Минимальная стоимость обязательна")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Стоимость должна быть положительной")]
        [Display(Name = "Мин. стоимость для партнера")]
        public decimal MinPriceForPartner { get; set; }

        [Required(ErrorMessage = "Основной материал обязателен")]
        [Display(Name = "Основной материал")]
        public int MainMaterialId { get; set; }
    }
}