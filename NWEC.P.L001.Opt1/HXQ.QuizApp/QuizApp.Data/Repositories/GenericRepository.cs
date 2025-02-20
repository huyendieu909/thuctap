using HXQ.QuizApp.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HXQ.QuizApp.Data.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly QuizAppDbContext db;
        protected readonly DbSet<T> dbSet;
        public GenericRepository(QuizAppDbContext context){
            db = context;
            dbSet = context.Set<T>();
            /* Note: giả sử db.Quizzes.ToList() 
             db thì vẫn là context như bth
            dbSet sẽ là db.Quizzes
            -> code sẽ gọn hơn, ko cần phải db.Set<T>. ... Tóm lại cứ nhìn 2 phương thức dưới là thấy =))
             */
        }

        public IEnumerable<T> GetAll() => db.Set<T>().ToList();

        public async Task<IEnumerable<T>> GetAllAsync() => await dbSet.ToListAsync();

        public T? GetById(Guid id) => dbSet.Find(id);


        public async Task<T?> GetByIdAsync(Guid id) => await dbSet.FindAsync(id);

        public void Add(T entity)
        {
            dbSet.Add(entity);
            db.SaveChanges();
        }

        public async Task AddAsync(T entity)
        {
            await dbSet.AddAsync(entity);
            await db.SaveChangesAsync();
        }

        public bool Update(T entity)
        {
            dbSet.Update(entity);
            return db.SaveChanges() > 0;
        }

        public bool Delete(Guid id)
        {
            var entity = dbSet.Find(id);
            if (entity == null) return false;
            else
            {
                dbSet.Remove(entity);
                db.SaveChanges();
                return true;
            }
        }

        public bool Delete(T entity)
        {
            dbSet.Remove(entity);
            return db.SaveChanges() > 0;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await dbSet.FindAsync(id);
            if (entity == null) return false;
            else
            {
                dbSet.Remove(entity);
                await db.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> DeleteAsync(T entity)
        {
            dbSet.Remove(entity);
            return await db.SaveChangesAsync() > 0;

        }

        public IQueryable<T> Get(
            Expression<Func<T, bool>>? filter = null, 
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, 
            string includeProperties = "")
        {
            IQueryable<T> query = dbSet;
            if (filter != null) query.Where(filter);
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var property in includeProperties.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries)) query.Include(property);
            }
            if (orderBy != null) query = orderBy(query);
            return query;
        }

        /* Note: .AsQueryable() sẽ trì hoãn thực thi truy vấn
         * Ví dụ: db.Quizzes.ToList() sẽ trả về list chứa toàn bộ danh sách Quiz nạp vào bộ nhớ
         * => nếu có nhiều bản gb có thể gây tốn RAM
         * db.Quizzes.AsQueryable() sẽ trả về IQueryable, có thể áp dụng lọc trước khi cho ra kết quả cuối. Cuối cùng mới thêm ToList.
         */
        public IQueryable<T> GetQuery() => dbSet.AsQueryable();

        public IQueryable<T> GetQuery(Expression<Func<T, bool>> predicate) => dbSet.Where(predicate);
    }
}
