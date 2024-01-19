using AmadoApp.Business.ViewModels.ProductVMs;
using AmadoApp.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmadoApp.Business.Services.Interfaces
{
    public interface IProductService
    {
        Task<IQueryable<Product>> ReadAsync();
        Task<Product> ReadIdAsync(int Id);
        Task CreateAsync(CreateProductVM entity, string env);
        Task UpdateAsync(UpdateProductVM entity, string env);
        Task DeleteAsync(int Id);
        Task RecoverAsync(int Id);
        Task RemoveAsync(int Id);
    }
}
