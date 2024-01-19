using AmadoApp.Business.Enums;
using AmadoApp.Business.Exceptions.Account;
using AmadoApp.Business.Exceptions.Commons;
using AmadoApp.Business.Services.Interfaces;
using AmadoApp.Business.ViewModels.AccountVMs;
using AmadoApp.Core.Entities.Account;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmadoApp.Business.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task CreateRoles()
        {
            foreach (var item in Enum.GetValues(typeof(UserRoles)))
            {
                var existsRole = await _roleManager.RoleExistsAsync(item.ToString());
                if (!existsRole)
                {
                    await _roleManager.CreateAsync(new IdentityRole(item.ToString()));
                }
            }
        }

        public async Task Login(LoginVM vm)
        {
            if (vm.UsernameOrEmail is null || vm.Password is null)
            {
                throw new ObjectParamsNullException("Object parameters is required!", nameof(vm.UsernameOrEmail));
            }

            var user = await _userManager.FindByEmailAsync(vm.UsernameOrEmail);
            if (user is null)
            {
                user = await _userManager.FindByNameAsync(vm.UsernameOrEmail);
                if (user is null) throw new UserNotFoundException("Username/Email or Password is not valid!", nameof(vm.UsernameOrEmail));
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, vm.Password, true);
            if (!result.Succeeded)
            {
                throw new UserNotFoundException("Username/Email or Password is not valid!", nameof(vm.Password));
            }

            await _signInManager.SignInAsync(user, true);
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task Register(RegisterVM vm)
        {
            if (vm.Username is null || vm.Name is null || vm.Surname is null ||
                vm.Email is null || vm.Password is null || vm.ConfirmPassword is null)
            {
                throw new ObjectParamsNullException("Object parameters is required!", nameof(vm.Username));
            }

            var existsEmail = await _userManager.FindByEmailAsync(vm.Email);

            if (existsEmail is null)
            {
                AppUser newUser = new()
                {
                    Name = vm.Name,
                    Email = vm.Email,
                    UserName = vm.Username,
                    Surname = vm.Surname
                };

                var result = await _userManager.CreateAsync(newUser, vm.Password);

                if (!result.Succeeded)
                {
                    foreach (var item in result.Errors)
                    {
                        throw new UserRegistrationException($"{item.Description}", nameof(item.Code));
                    }
                }

                await _userManager.AddToRoleAsync(newUser, UserRoles.Admin.ToString());
            }
            else
            {
                throw new UsedEmailException("This email address used before, try another!", nameof(vm.Email));
            }
        }
    }
}
