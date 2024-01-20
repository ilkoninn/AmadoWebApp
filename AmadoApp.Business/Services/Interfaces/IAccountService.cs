using AmadoApp.Business.ViewModels.AccountVMs;
using AmadoApp.Core.Entities.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmadoApp.Business.Services.Interfaces
{
    public interface IAccountService
    {
        Task<List<string>> Register(RegisterVM vm);
        Task Login(LoginVM vm);
        Task Logout();
        Task CreateRoles();
        Task<List<string>> SendConfirmEmailAddress(AppUser user);
        Task<bool> ConfirmEmailAddress(ConfirmEmailVM vm, string userId, string token, string pincode);
        Task Subscription(SubscribeVM vm);
    }
}
