using AmadoApp.Business.Exceptions.Commons;
using AmadoApp.Core.Entities;
using AmadoApp.DAL.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AmadoApp.MVC.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class SubscribeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly DbSet<Subscribe> _table;
        public SubscribeController(AppDbContext context)
        {
            _context = context;
            _table = _context.Set<Subscribe>();
        }

        [Authorize(Roles = "Moderator, Admin")]
        public async Task<IActionResult> Table()
        {
            if (User.IsInRole("Admin"))
            {
                IQueryable<Subscribe> subcribes = _table;

                return View(subcribes);
            }
            else
            {
                IQueryable<Subscribe> subcribes = _table.Where(x => !x.IsDeleted);

                return View(subcribes);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var oldSubscibre = await CheckSubscribe(id);

            oldSubscibre.IsDeleted = true;

            _context.Update(oldSubscibre);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Table));
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Recover(int id)
        {
            var oldSubscibre = await CheckSubscribe(id);

            oldSubscibre.IsDeleted = false;

            _context.Update(oldSubscibre);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Table));
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Remove(int id)
        {
            var oldSubscibre = await CheckSubscribe(id);

            _context.Subscribes.Remove(oldSubscibre);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Table));
        }

        public async Task<Subscribe> CheckSubscribe(int id)
        {
            if (id <= 0) throw new IdNegativeOrZeroException("Id must be over than and not equal to zero!", nameof(id));
            var oldSubscribe = await _context.Subscribes.FirstOrDefaultAsync(x => x.Id == id);
            if (oldSubscribe is null) throw new ObjectNullException("There is no like that object in Data!", nameof(oldSubscribe));

            return oldSubscribe;
        }
    }
}
