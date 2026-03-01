using Gusev284_UP.Data;
using Gusev284_UP.Models;
using Gusev284_UP.Services;      // для IRawMaterialCalculator
using Gusev284_UP.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Gusev284_UP.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IRawMaterialCalculator _calculator;

        public ProductsController(ApplicationDbContext context, IRawMaterialCalculator calculator)
        {
            _context = context;
            _calculator = calculator;
        }

        // ==================== МОДУЛЬ 2: Главная страница со списком ====================
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products
                .Include(p => p.ProductType)
                .Include(p => p.ProductWorkshops)
                .Select(p => new ProductIndexViewModel
                {
                    ProductId = p.ProductId,
                    Article = p.Article,
                    ProductTypeName = p.ProductType.Name,
                    Name = p.Name,
                    MinPriceForPartner = p.MinPriceForPartner,
                    TotalTimeHours = (int)Math.Ceiling(p.ProductWorkshops.Sum(pw => pw.TimeHours))
                })
                .ToListAsync();

            // Загружаем справочники для выпадающих списков в модальном окне
            ViewBag.ProductTypes = new SelectList(await _context.ProductTypes.ToListAsync(), "ProductTypeId", "Name");
            ViewBag.MaterialTypes = new SelectList(await _context.MaterialTypes.ToListAsync(), "MaterialTypeId", "Name");

            return View(products);
        }

        // ==================== МОДУЛЬ 3: Получение данных для редактирования (для модалки) ====================
        [HttpGet]
        public async Task<IActionResult> GetProductForEdit(int id)
        {
            var product = await _context.Products
                .Include(p => p.ProductType)
                .Include(p => p.MainMaterial)
                .FirstOrDefaultAsync(p => p.ProductId == id);

            if (product == null)
            {
                return NotFound();
            }

            var viewModel = new ProductFormViewModel
            {
                ProductId = product.ProductId,
                Article = product.Article,
                ProductTypeId = product.ProductTypeId,
                Name = product.Name,
                MinPriceForPartner = product.MinPriceForPartner,
                MainMaterialId = product.MainMaterialId
            };

            return Json(viewModel);
        }

        // ==================== МОДУЛЬ 3: Сохранение (добавление/редактирование) ====================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveProduct([FromForm] ProductFormViewModel model)
        {
            // Проверка валидации модели
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                TempData["ErrorMessage"] = "Ошибка валидации: " + string.Join("; ", errors);
                return RedirectToAction(nameof(Index));
            }

            // Проверка существования типов
            var productTypeExists = await _context.ProductTypes.AnyAsync(pt => pt.ProductTypeId == model.ProductTypeId);
            var materialTypeExists = await _context.MaterialTypes.AnyAsync(mt => mt.MaterialTypeId == model.MainMaterialId);

            if (!productTypeExists || !materialTypeExists)
            {
                TempData["ErrorMessage"] = "Выбранный тип продукции или материал не существует.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                if (model.ProductId == 0) // Добавление
                {
                    var product = new Product
                    {
                        Article = model.Article,
                        ProductTypeId = model.ProductTypeId,
                        Name = model.Name,
                        MinPriceForPartner = model.MinPriceForPartner,
                        MainMaterialId = model.MainMaterialId
                    };

                    _context.Products.Add(product);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Продукт успешно добавлен.";
                }
                else // Редактирование
                {
                    var product = await _context.Products.FindAsync(model.ProductId);
                    if (product == null)
                    {
                        TempData["ErrorMessage"] = "Продукт не найден.";
                        return RedirectToAction(nameof(Index));
                    }

                    product.Article = model.Article;
                    product.ProductTypeId = model.ProductTypeId;
                    product.Name = model.Name;
                    product.MinPriceForPartner = model.MinPriceForPartner;
                    product.MainMaterialId = model.MainMaterialId;

                    _context.Products.Update(product);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Продукт успешно обновлён.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Ошибка при сохранении: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }

        // ==================== МОДУЛЬ 4: Страница цехов для продукта ====================
        public async Task<IActionResult> Workshops(int productId)
        {
            var product = await _context.Products
                .Include(p => p.ProductWorkshops)
                    .ThenInclude(pw => pw.Workshop)
                .FirstOrDefaultAsync(p => p.ProductId == productId);

            if (product == null)
            {
                TempData["ErrorMessage"] = "Продукт не найден.";
                return RedirectToAction(nameof(Index));
            }

            var viewModel = new ProductWorkshopsViewModel
            {
                ProductName = product.Name,
                Workshops = product.ProductWorkshops.Select(pw => new WorkshopDetailViewModel
                {
                    WorkshopName = pw.Workshop.Name,
                    EmployeeCount = pw.Workshop.EmployeeCount,
                    TimeHours = pw.TimeHours
                }).ToList()
            };

            return View(viewModel);
        }

        // ==================== МОДУЛЬ 4: Страница калькулятора сырья (GET - отобразить форму) ====================
        [HttpGet]
        public async Task<IActionResult> Calculate()
        {
            // Загружаем списки для выпадающих списков
            ViewBag.ProductTypes = new SelectList(await _context.ProductTypes.ToListAsync(), "ProductTypeId", "Name");
            ViewBag.MaterialTypes = new SelectList(await _context.MaterialTypes.ToListAsync(), "MaterialTypeId", "Name");

            return View(new RawMaterialCalculateViewModel());
        }

        // ==================== МОДУЛЬ 4: Расчёт сырья (POST - обработка формы) ====================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Calculate(RawMaterialCalculateViewModel model)
        {
            // Загружаем списки снова на случай ошибки
            ViewBag.ProductTypes = new SelectList(await _context.ProductTypes.ToListAsync(), "ProductTypeId", "Name", model.ProductTypeId);
            ViewBag.MaterialTypes = new SelectList(await _context.MaterialTypes.ToListAsync(), "MaterialTypeId", "Name", model.MaterialTypeId);

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Вызов сервиса расчёта
            int result = _calculator.CalculateRawMaterial(
                model.ProductTypeId,
                model.MaterialTypeId,
                model.Quantity,
                model.Param1,
                model.Param2
            );

            if (result == -1)
            {
                ModelState.AddModelError("", "Не удалось выполнить расчёт. Проверьте правильность введённых данных.");
                return View(model);
            }

            model.Result = result;
            return View(model);
        }
    }
}