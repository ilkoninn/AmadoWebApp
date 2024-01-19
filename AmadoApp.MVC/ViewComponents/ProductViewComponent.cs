using AmadoApp.Business.Services.Interfaces;
using AmadoApp.Core.Entities;
using AmadoApp.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AmadoApp.MVC.ViewComponents
{
    public class ProductViewComponent : ViewComponent
    {
        private readonly IProductService _ser;

        public ProductViewComponent(IProductService ser)
        {
            _ser = ser;
        }

        public async Task<IViewComponentResult> InvokeAsync(int changer)
        {
            IQueryable<Product> query = await _ser.ReadAsync();
            ViewBag.Changer = false;

            if(changer == 0)
            {
                ViewBag.Changer = true;
            }

            return View(query);
        }
    }
}
