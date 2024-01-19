using AmadoApp.Business.Exceptions.Account;
using AmadoApp.Business.Exceptions.Commons;
using AmadoApp.Business.Services.Interfaces;
using AmadoApp.Business.ViewModels.AccountVMs;
using Microsoft.AspNetCore.Mvc;

namespace AmadoApp.MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM vm)
        {
            try
            {
                await _accountService.Register(vm);

                return RedirectToAction(nameof(Login));
            }
            catch (UserRegistrationException ex)
            {
                ModelState.AddModelError(ex.ParamName, ex.Message);

                return View(vm);
            }
            catch (ObjectParamsNullException ex)
            {
                ModelState.AddModelError(ex.ParamName, ex.Message);

                return View(vm);
            }
            catch (UsedEmailException ex)
            {
                ModelState.AddModelError(ex.ParamName, ex.Message);

                return View(vm);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM vm, string? returnUrl)
        {
            try
            {
                await _accountService.Login(vm);

                if (returnUrl is not null) return Redirect(returnUrl);

                return RedirectToAction("Index", "Home");
            }
            catch (UserNotFoundException ex)
            {
                ModelState.AddModelError(ex.ParamName, ex.Message);

                return View(vm);
            }
            catch (ObjectParamsNullException ex)
            {
                ModelState.AddModelError(ex.ParamName, ex.Message);

                return View(vm);
            }
        }

        [HttpGet]
        public async Task<IActionResult> CreateRoles()
        {
            await _accountService.CreateRoles();

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            if (User.Identity.IsAuthenticated)
            {
                await _accountService.Logout();

                return RedirectToAction(nameof(Login));
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
