using AmadoApp.Business.Exceptions.Commons;
using AmadoApp.Business.Services.Interfaces;
using AmadoApp.Business.ViewModels.BrandVMs;
using AmadoApp.Core.Entities;
using AmadoApp.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmadoApp.Business.Services.Implementations
{
    public class BrandService : IBrandService
    {
        private readonly IBrandRepository _rep;

        public BrandService(IBrandRepository rep)
        {
            _rep = rep;
        }

        public async Task<IQueryable<Brand>> ReadAsync()
        {
            IQueryable<Brand> brands = await _rep.ReadAsync();

            return brands;
        }

        public async Task<Brand> ReadIdAsync(int Id)
        {
            return await CheckBrand(Id);
        }

        public async Task CreateAsync(CreateBrandVM entity)
        {
            if(entity.Name is null) throw new ObjectParamsNullException("Object parameters is required!", nameof(entity.Name));

            var existsSameName = await (await _rep.ReadAsync(expression: x => x.Name == entity.Name))
                .FirstOrDefaultAsync() is null;

            if (!existsSameName) throw new ObjectSameParamsException("There is same name brand in data!", nameof(entity.Name));


            Brand newBrand = new()
            {
                Name = entity.Name,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
            };

            await _rep.CreateAsync(newBrand);
            await _rep.SaveChangesAsync();
        }

        public async Task DeleteAsync(int Id)
        {
            await CheckBrand(Id);

            await _rep.DeleteAsync(Id);
            await _rep.SaveChangesAsync();
        }

        public async Task UpdateAsync(UpdateBrandVM entity)
        {
            var oldBrand = await CheckBrand(entity.Id);

            var existsSameName = await (await _rep.ReadAsync(expression: x => x.Name == entity.Name && x.Id != entity.Id))
                .FirstOrDefaultAsync() is null;

            if (!existsSameName) throw new ObjectSameParamsException("There is same name brand in data!", nameof(entity.Name));


            oldBrand.Name = entity.Name;
            oldBrand.UpdatedDate = DateTime.Now;

            await _rep.UpdateAsync(oldBrand);
            await _rep.SaveChangesAsync();
        }

        public async Task<Brand> CheckBrand(int id)
        {
            if (id <= 0) throw new IdNegativeOrZeroException("Id must be over than and not equal to zero!", nameof(id));
            var oldBrand = await _rep.ReadIdAsync(id);
            if (oldBrand is null) throw new ObjectNullException("There is no like that object in Data!", nameof(oldBrand));

            return oldBrand;
        }

        public async Task RecoverAsync(int Id)
        {
            await CheckBrand(Id);

            await _rep.RecoverAsync(Id);
            await _rep.SaveChangesAsync();
        }

        public async Task RemoveAsync(int Id)
        {
            await CheckBrand(Id);

            await _rep.RemoveAsync(Id);
            await _rep.SaveChangesAsync();
        }
    }
}
