using Microsoft.AspNetCore.Mvc;

namespace AmadoApp.MVC.Controllers
{
    public class CartController : Controller
    {
        public async Task<IActionResult> CartView()
        {
            return View();
        }
    }
}
