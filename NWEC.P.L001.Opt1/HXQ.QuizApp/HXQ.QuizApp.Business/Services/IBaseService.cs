using HXQ.QuizApp.Business.ViewModels;
using System.Linq.Expressions;

namespace HXQ.QuizApp.Business.Services
{
    public interface IBaseService<T>
    {
        Task<int> AddAync(T entity);
        Task<bool> UpdateAsync(T entity);
        bool Delete(Guid id);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> DeleteAsync(T entity);
        Task<T?> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<PaginatedResult<T>> GetAsync(
            Expression<Func<T, bool>>? filter = null, 
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            string includeProperties = "", 
            int pageIndex = 1, 
            int pageSize = 10);
    }
}
