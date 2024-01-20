using AmadoApp.Core.Entities;
using AmadoApp.DAL.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
