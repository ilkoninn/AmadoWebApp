using Microsoft.AspNetCore.Mvc;

namespace AmadoApp.MVC.Controllers
{
    public class CheckOutController : Controller
    {
        public async Task<IActionResult> CheckoutView()
        {
            return View();
        }
    }
}
