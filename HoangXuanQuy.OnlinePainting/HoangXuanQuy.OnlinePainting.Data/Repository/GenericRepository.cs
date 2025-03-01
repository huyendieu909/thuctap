using HoangXuanQuy.OnlinePainting.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangXuanQuy.OnlinePainting.Data.Repository
{
    internal class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly OnlinePaintingContext _context;
        protected readonly DbSet<T> _dbSet;
        public GenericRepository(OnlinePaintingContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null) return false;
            _dbSet.Remove(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);

        }

        public IQueryable<T> GetQuery()
        {
            return _dbSet.AsQueryable();
        }

        public IQueryable<T> GetQuery(Func<T, bool> predicate)
        {
            return _dbSet.Where(predicate).AsQueryable();
        }

        public bool UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            return _context.SaveChanges() > 0;
        }
    }
}
