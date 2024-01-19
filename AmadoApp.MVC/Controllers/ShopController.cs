using AmadoApp.Business.Services.Interfaces;
using AmadoApp.Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace AmadoApp.MVC.Controllers
{
    public class ShopController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IBrandService _brandService;

        public ShopController(IProductService productService, ICategoryService categoryService, IBrandService brandService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _brandService = brandService;
        }

        public async Task<IActionResult> ShopList()
        {
            ViewData["Categories"] = await _categoryService.ReadAsync();
            ViewData["Brands"] = await _brandService.ReadAsync();
            ViewData["Products"] = await _productService.ReadAsync();

            return View();
        }
        public async Task<IActionResult> ProductSingle(int id)
        {
            Product oldProduct =  await _productService.ReadIdAsync(id);

            return View(oldProduct);
        }
    }
}
