using AmadoApp.Business.Exceptions.Commons;
using AmadoApp.Business.Services.Interfaces;
using AmadoApp.Business.ViewModels.CategoryVMs;
using AmadoApp.Business.ViewModels.CategoryVMs;
using AmadoApp.Core.Entities;
using AmadoApp.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmadoApp.Business.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _rep;

        public CategoryService(ICategoryRepository rep)
        {
            _rep = rep;
        }

        public async Task<IQueryable<Category>> ReadAsync()
        {
            IQueryable<Category> categories = await _rep.ReadAsync();

            return categories;
        }

        public async Task<Category> ReadIdAsync(int Id)
        {
            return await CheckCategory(Id);
        }

        public async Task CreateAsync(CreateCategoryVM entity)
        {
            if (entity.Name is null) throw new ObjectParamsNullException("Object parameters is required!", nameof(entity.Name));

            var existsSameName = await (await _rep.ReadAsync(expression: x => x.Name == entity.Name))
                .FirstOrDefaultAsync() is null;

            if (!existsSameName) throw new ObjectSameParamsException("There is same name category in data!", nameof(entity.Name));

            Category newCategory = new()
            {
                Name = entity.Name,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
            };

            await _rep.CreateAsync(newCategory);
            await _rep.SaveChangesAsync();
        }

        public async Task DeleteAsync(int Id)
        {
            await CheckCategory(Id);

            await _rep.DeleteAsync(Id);
            await _rep.SaveChangesAsync();
        }

        public async Task UpdateAsync(UpdateCategoryVM entity)
        {
            var oldCategory = await CheckCategory(entity.Id);

            var existsSameName = await (await _rep.ReadAsync(expression: x => x.Name == entity.Name && x.Id != entity.Id))
                .FirstOrDefaultAsync() is null;

            if (!existsSameName) throw new ObjectSameParamsException("There is same name category in data!", nameof(entity.Name));

            oldCategory.Name = entity.Name;
            oldCategory.UpdatedDate = DateTime.Now;

            await _rep.UpdateAsync(oldCategory);
            await _rep.SaveChangesAsync();
        }

        public async Task<Category> CheckCategory(int id)
        {
            if (id <= 0) throw new IdNegativeOrZeroException("Id must be over than and not equal to zero!", nameof(id));
            var oldCategory = await _rep.ReadIdAsync(id);
            if (oldCategory is null) throw new ObjectNullException("There is no like that object in Data!", nameof(oldCategory));

            return oldCategory;
        }

        public async Task RecoverAsync(int Id)
        {
            await CheckCategory(Id);

            await _rep.RecoverAsync(Id);
            await _rep.SaveChangesAsync();
        }

        public async Task RemoveAsync(int Id)
        {
            await CheckCategory(Id);

            await _rep.RemoveAsync(Id);
            await _rep.SaveChangesAsync();
        }
    }
}
