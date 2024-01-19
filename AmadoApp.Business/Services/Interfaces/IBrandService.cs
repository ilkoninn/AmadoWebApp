using AmadoApp.Business.ViewModels.BrandVMs;
using AmadoApp.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmadoApp.Business.Services.Interfaces
{
    public interface IBrandService
    {
        Task<IQueryable<Brand>> ReadAsync();
        Task<Brand> ReadIdAsync(int Id);
        Task CreateAsync(CreateBrandVM entity);
        Task UpdateAsync(UpdateBrandVM entity);
        Task DeleteAsync(int Id);
        Task RecoverAsync(int Id);
        Task RemoveAsync(int Id);
    }
}
