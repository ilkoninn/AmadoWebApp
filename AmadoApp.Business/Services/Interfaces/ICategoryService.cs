using AmadoApp.Business.ViewModels.CategoryVMs;
using AmadoApp.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmadoApp.Business.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IQueryable<Category>> ReadAsync();
        Task<Category> ReadIdAsync(int Id);
        Task CreateAsync(CreateCategoryVM entity);
        Task UpdateAsync(UpdateCategoryVM entity);
        Task DeleteAsync(int Id);
        Task RecoverAsync(int Id);
        Task RemoveAsync(int Id);
    }
}
