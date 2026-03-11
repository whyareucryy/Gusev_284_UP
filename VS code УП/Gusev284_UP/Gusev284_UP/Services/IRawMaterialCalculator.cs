namespace Gusev284_UP.Services
{
    public interface IRawMaterialCalculator
    {
        /// <summary>
        /// Рассчитывает количество сырья для производства продукции
        /// </summary>
        /// <param name="productTypeId">ID типа продукции</param>
        /// <param name="materialTypeId">ID типа материала</param>
        /// <param name="quantity">Количество продукции</param>
        /// <param name="param1">Параметр 1 (вещественный)</param>
        /// <param name="param2">Параметр 2 (вещественный)</param>
        /// <returns>Целое количество сырья или -1 при ошибке</returns>
        int CalculateRawMaterial(int productTypeId, int materialTypeId, int quantity, double param1, double param2);
    }
}