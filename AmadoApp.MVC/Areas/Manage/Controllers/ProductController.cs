using AmadoApp.Business.Exceptions.Commons;
using AmadoApp.Business.Services.Interfaces;
using AmadoApp.Business.ViewModels.ProductVMs;
using AmadoApp.Core.Entities;
using AmadoApp.DAL.Context;
using AmadoApp.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AmadoApp.MVC.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class ProductController : Controller
    {
        private readonly IProductService _ProductService;
        private readonly IWebHostEnvironment _env;

        private readonly IBrandRepository _repBrand;
        private readonly ICategoryRepository _repCat;
        private readonly AppDbContext _context;

        public ProductController(IProductService ProductService, IWebHostEnvironment env, IBrandRepository repBrand, ICategoryRepository repCat, AppDbContext context)
        {
            _ProductService = ProductService;
            _env = env;
            _repBrand = repBrand;
            _repCat = repCat;
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "Moderator, Admin")]
        public async Task<IActionResult> Table()
        {
            if (User.IsInRole("Admin"))
            {
                IQueryable<Product> products = await _ProductService.ReadAsync();

                return View(products);
            }
            else
            {
                IQueryable<Product> products = (await _ProductService.ReadAsync()).Where(x => !x.IsDeleted);

                return View(products);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Moderator, Admin")]
        public async Task<IActionResult> Detail(int id)
        {
            try
            {
                Product oldProduct = await _ProductService.ReadIdAsync(id);

                return View(oldProduct);
            }
            catch (IdNegativeOrZeroException ex)
            {
                ModelState.AddModelError(ex.ParamName, ex.Message);

                return RedirectToAction(nameof(Table));
            }
            catch (ObjectNullException ex)
            {
                ModelState.AddModelError(ex.ParamName, ex.Message);

                return RedirectToAction(nameof(Table));
            }
        }

        [HttpGet]
        [Authorize(Roles = "Moderator, Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _ProductService.DeleteAsync(id);

                return RedirectToAction(nameof(Table));
            }
            catch (IdNegativeOrZeroException ex)
            {
                ModelState.AddModelError(ex.ParamName, ex.Message);

                return RedirectToAction(nameof(Table));
            }
            catch (ObjectNullException ex)
            {
                ModelState.AddModelError(ex.ParamName, ex.Message);

                return RedirectToAction(nameof(Table));
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Recover(int id)
        {
            try
            {
                await _ProductService.RecoverAsync(id);

                return RedirectToAction(nameof(Table));
            }
            catch (IdNegativeOrZeroException ex)
            {
                ModelState.AddModelError(ex.ParamName, ex.Message);

                return RedirectToAction(nameof(Table));
            }
            catch (ObjectNullException ex)
            {
                ModelState.AddModelError(ex.ParamName, ex.Message);

                return RedirectToAction(nameof(Table));
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Remove(int id)
        {
            try
            {
                await _ProductService.RemoveAsync(id);

                return RedirectToAction(nameof(Table));
            }
            catch (IdNegativeOrZeroException ex)
            {
                ModelState.AddModelError(ex.ParamName, ex.Message);

                return RedirectToAction(nameof(Table));
            }
            catch (ObjectNullException ex)
            {
                ModelState.AddModelError(ex.ParamName, ex.Message);

                return RedirectToAction(nameof(Table));
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            CreateProductVM vm = new()
            {
                Brands = await _repBrand.ReadAsync(),
                Categories = await _repCat.ReadAsync(),
                Colors = await _context.Colors.ToListAsync()
            };

            return View(vm);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateProductVM vm)
        {
            try
            {
                await _ProductService.CreateAsync(vm, _env.WebRootPath);

                return RedirectToAction(nameof(Table));
            }
            catch (ObjectParamsNullException ex)
            {
                ModelState.AddModelError(ex.ParamName, ex.Message);

                return View(vm);
            }
            catch (ObjectSameParamsException ex)
            {
                ModelState.AddModelError(ex.ParamName, ex.Message);

                return View(vm);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id)
        {
            try
            {
                Product oldProduct = await _ProductService.ReadIdAsync(id);

                UpdateProductVM vm = new()
                {
                    Title = oldProduct.Title,
                    Description = oldProduct.Description,
                    Price = oldProduct.Price,
                    IsInStock = oldProduct.IsInStock,
                    BrandId = oldProduct.BrandId,
                    ProductImageVMs = new List<ProductImageVM>(),
                    Brands = await _repBrand.ReadAsync(),
                    Categories = await _repCat.ReadAsync(),
                    CategoryIds = oldProduct.ProductCategories.Select(x => x.CategoryId).ToList(),
                    Colors = await _context.Colors.ToListAsync(),
                    ColorIds = oldProduct.ProductColors.Select(x => x.ColorId).ToList(),
                };

                foreach (var item in oldProduct.ProductImages)
                {
                    ProductImageVM productImageVM = new()
                    {
                        Id = item.Id,
                        ImageUrl = item.ImageUrl,
                    };

                    vm.ProductImageVMs.Add(productImageVM);
                }


                return View(vm);
            }
            catch (IdNegativeOrZeroException ex)
            {
                ModelState.AddModelError(ex.ParamName, ex.Message);

                return RedirectToAction(nameof(Table));
            }
            catch (ObjectNullException ex)
            {
                ModelState.AddModelError(ex.ParamName, ex.Message);

                return RedirectToAction(nameof(Table));
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(UpdateProductVM vm)
        {
            try
            {
                await _ProductService.UpdateAsync(vm, _env.WebRootPath);

                return RedirectToAction(nameof(Table));
            }
            catch (IdNegativeOrZeroException ex)
            {
                ModelState.AddModelError(ex.ParamName, ex.Message);

                return RedirectToAction(nameof(Table));
            }
            catch (ObjectNullException ex)
            {
                ModelState.AddModelError(ex.ParamName, ex.Message);

                return RedirectToAction(nameof(Table));
            }
            catch (ObjectParamsNullException ex)
            {
                ModelState.AddModelError(ex.ParamName, ex.Message);

                return View(vm);
            }
            catch (ObjectSameParamsException ex)
            {
                ModelState.AddModelError(ex.ParamName, ex.Message);

                return View(vm);
            }
        }
    }
}
