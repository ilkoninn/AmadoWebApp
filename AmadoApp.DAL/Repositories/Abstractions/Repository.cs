using AmadoApp.Core.Entities.BaseEntities;
using AmadoApp.DAL.Context;
using AmadoApp.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AmadoApp.DAL.Repositories.Abstractions
{
    public class Repository<T> : IRepository<T> where T : BaseAuditableEntity, new()
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<IQueryable<T>> ReadAsync(
        Expression<Func<T, bool>>? expression = null,
        Expression<Func<T, object>>? expressionOrder = null,
        bool isDescending = false,
        params string[] includes
)
        {
            IQueryable<T> query = _dbSet;

            if (expression is not null)
            {
                query = query.Where(expression);
            }
            if (expressionOrder is not null)
            {
                query = isDescending ? query.OrderByDescending(expressionOrder) : query.OrderBy(expressionOrder);
            }
            if (includes is not null)
            {
                for (int i = 0; i < includes.Length; i++)
                {
                    query = query.Include(includes[i]);
                }
            }

            return query;
        }

        public async Task<T> ReadIdAsync(int Id = 0, params string[] entityIncludes)
        {
            IQueryable<T> query = _dbSet;

            if (entityIncludes is not null)
            {
                for (int i = 0; i < entityIncludes.Length; i++)
                {
                    query = query.Include(entityIncludes[i]);
                }
            }

            return await query.AsNoTracking().Where(x => x.Id == Id).FirstOrDefaultAsync();
        }

        public async Task<T> CreateAsync(T entity)
        {
            await _dbSet.AddAsync(entity);

            return entity;
        }
        public async Task<T> UpdateAsync(T entity)
        {
            _dbSet.Update(entity);

            return entity;
        }

        public async Task<T> DeleteAsync(int Id)
        {
            var entity = await ReadIdAsync(Id);
            entity.IsDeleted = true;

            await UpdateAsync(entity);

            return entity;
        }

        public async Task<int> SaveChangesAsync()
        {
            var result = await _context.SaveChangesAsync();

            return result;
        }

        public async Task<T> RecoverAsync(int Id)
        {
            var entity = await ReadIdAsync(Id);
            entity.IsDeleted = false;

            await UpdateAsync(entity);

            return entity;
        }

        public async Task RemoveAsync(int Id)
        {
            var entity = await ReadIdAsync(Id);

            _dbSet.Remove(entity);
        }
    }
}
