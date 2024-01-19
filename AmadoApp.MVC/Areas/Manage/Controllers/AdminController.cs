using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AmadoApp.MVC.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class AdminController : Controller
    {
        [Authorize(Roles = "Moderator, Admin")]
        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}
