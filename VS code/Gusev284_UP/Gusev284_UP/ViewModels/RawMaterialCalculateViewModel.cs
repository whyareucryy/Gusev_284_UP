using System.ComponentModel.DataAnnotations;

namespace Gusev284_UP.ViewModels
{
    public class RawMaterialCalculateViewModel
    {
        [Required(ErrorMessage = "Выберите тип продукции")]
        [Display(Name = "Тип продукции")]
        public int ProductTypeId { get; set; }

        [Required(ErrorMessage = "Выберите тип материала")]
        [Display(Name = "Тип материала")]
        public int MaterialTypeId { get; set; }

        [Required(ErrorMessage = "Укажите количество продукции")]
        [Range(1, int.MaxValue, ErrorMessage = "Количество должно быть положительным целым числом")]
        [Display(Name = "Количество продукции")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Параметр 1 обязателен")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Параметр должен быть положительным числом")]
        [Display(Name = "Параметр 1")]
        public double Param1 { get; set; }

        [Required(ErrorMessage = "Параметр 2 обязателен")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Параметр должен быть положительным числом")]
        [Display(Name = "Параметр 2")]
        public double Param2 { get; set; }

        [Display(Name = "Результат (количество сырья)")]
        public int? Result { get; set; }
    }
}