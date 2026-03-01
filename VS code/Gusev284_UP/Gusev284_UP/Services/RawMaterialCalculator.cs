using Gusev284_UP.Data;
using Microsoft.EntityFrameworkCore;

namespace Gusev284_UP.Services
{
    public class RawMaterialCalculator : IRawMaterialCalculator
    {
        private readonly ApplicationDbContext _context;

        public RawMaterialCalculator(ApplicationDbContext context)
        {
            _context = context;
        }

        public int CalculateRawMaterial(int productTypeId, int materialTypeId, int quantity, double param1, double param2)
        {
            try
            {
                // Получаем коэффициент типа продукции
                var productType = _context.ProductTypes.FirstOrDefault(pt => pt.ProductTypeId == productTypeId);
                if (productType == null) return -1;

                // Получаем процент потерь материала
                var materialType = _context.MaterialTypes.FirstOrDefault(mt => mt.MaterialTypeId == materialTypeId);
                if (materialType == null) return -1;

                // Проверка входных данных
                if (quantity <= 0 || param1 <= 0 || param2 <= 0)
                    return -1;

                // Количество сырья на одну единицу продукции = произведение параметров * коэффициент типа продукции
                double rawPerUnit = param1 * param2 * productType.Coefficient;

                // Общее количество сырья без учёта потерь
                double totalRaw = rawPerUnit * quantity;

                // Увеличиваем с учётом процента потерь
                double totalWithLoss = totalRaw * (1 + materialType.LossPercent);

                // Возвращаем целое число (округляем вверх, чтобы точно хватило)
                return (int)Math.Ceiling(totalWithLoss);
            }
            catch
            {
                return -1;
            }
        }
    }
}