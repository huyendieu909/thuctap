using HoangXuanQuy.OnlinePainting.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangXuanQuy.OnlinePainting.Data.Repository
{
    public interface IGenericRepository<T>
    {
        Task<IEnumerable<T>> GetAllAsync();  
        Task<T?> GetByIdAsync(int id);
        Task AddAsync(T entity);
        bool UpdateAsync(T entity);
        Task<bool> DeleteAsync(int id);

        //raw query không async, truy vấn xong mới ToListAsync()
        IQueryable<T> GetQuery(); 
        IQueryable<T> GetQuery(Func<T, bool> predicate);
        


    }
}
