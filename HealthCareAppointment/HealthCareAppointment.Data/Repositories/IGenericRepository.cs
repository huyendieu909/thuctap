using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HealthCareAppointment.Data.Repositories
{
    public interface IGenericRepository<T>
    {
        IEnumerable<T> GetAll();
        Task<IEnumerable<T>> GetAllAsync();
        T? GetById(Guid id);
        Task<T?> GetByIdAsync(Guid id);

        void Add(T entity);
        Task AddAsync(T entity);

        /* Note: Không viết Task UpdateAsync(T entity); vì bản chất db.Update(entity) là thao tác đồng bộ, ko cần async */
        bool Update(T entity);

        bool Delete(Guid id);
        Task<bool> DeleteAsync(Guid id);
        bool Delete(T entity);
        Task<bool> DeleteAsync(T entity);

        Task<T> FindAsync(Guid? id);

        /* Note: IQueryable<T> không cần async vì nó chưa thực thi truy vấn. Cái cần async là .ToListAsync() */
        //Truy vấn gốc
        IQueryable<T> GetQuery();
        //Truy vấn có điều kiện
        IQueryable<T> GetQuery(Expression<Func<T, bool>> predicate);
        //Truy vấn có filter điều kiện, order và include
        IQueryable<T> Get(
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            string includeProperties = "");
    }
}
