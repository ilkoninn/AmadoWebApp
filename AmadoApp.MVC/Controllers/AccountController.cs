using AmadoApp.Business.Exceptions.Account;
using AmadoApp.Business.Exceptions.Commons;
using AmadoApp.Business.Services.Interfaces;
using AmadoApp.Business.ViewModels.AccountVMs;
using AmadoApp.Core.Entities.Account;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;

namespace AmadoApp.MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly LinkGenerator _linkGenerator;
        private readonly IHttpContextAccessor _http;

        public AccountController(IAccountService accountService, LinkGenerator linkGenerator, IHttpContextAccessor http)
        {
            _accountService = accountService;
            _linkGenerator = linkGenerator;
            _http = http;
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
                var result = await _accountService.Register(vm);

                var token = result[0];
                var pincode = result[1];
                var userId = result[2];

                string url = _linkGenerator.GetUriByAction(_http.HttpContext, action: "ConfirmEmail", controller: "Account",
                values: new 
                {
                    token,
                    userId,
                    pincode
                });

                return Redirect(url);
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

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmEmail(ConfirmEmailVM vm, string userId, string token, string pincode)
        {
            ViewBag.Success = true;

            if(await _accountService.ConfirmEmailAddress(vm: vm, userId: userId, token: token, pincode: pincode))
            {
                return RedirectToAction(nameof(Login));
            }
            else
            {
                ViewBag.Success = false;
            }
            
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Subscription(SubscribeVM vm, string? returnUrl)
        {
            try
            {
                await _accountService.Subscription(vm);

                if (returnUrl is not null) return Redirect(returnUrl);

                return RedirectToAction("Index", "Home");
            }
            catch (UsedEmailException ex)
            {
                ModelState.AddModelError(ex.ParamName, ex.Message);

                if (returnUrl is not null) return Redirect(returnUrl);

                return RedirectToAction("Index", "Home", vm);
            }
        }
    }
}
