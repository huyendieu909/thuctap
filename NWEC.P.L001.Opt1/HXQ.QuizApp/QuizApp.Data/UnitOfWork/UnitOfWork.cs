
using HXQ.QuizApp.Data.Context;
using HXQ.QuizApp.Data.Models;
using HXQ.QuizApp.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace HXQ.QuizApp.Data.UnitOfWork
{
    /* Note: giả sử có một sửa đổi trong dữ liệu mà có liên quan đến rất nhiều bảng, thay vì phải gọi rất nhiều instances thì có thể gộp chung các interface có nhiệm vụ liên quan vào một instance rồi thực hiện tất cả 1 lần 
      Unit of Work sinh ra từ đây */
    public class UnitOfWork : IUnitOfWork
    {
        private readonly QuizAppDbContext db;
        private IDbContextTransaction transaction;
        public UnitOfWork(QuizAppDbContext context)
        {
            db = context;
            // Khởi tạo các repository cụ thể dựa trên GenericRepository
            QuizRepository = new GenericRepository<Quiz>(db);
            QuestionRepository = new GenericRepository<Question>(db);
            UserQuizRepository = new GenericRepository<UserQuiz>(db);
            QuizQuestionRepository = new GenericRepository<QuizQuestion>(db);
            UserAnswerRepository = new GenericRepository<UserAnswer>(db);
            UserRepository = new GenericRepository<User>(db);
            RoleRepository = new GenericRepository<Role>(db);
            AnswerRepository = new GenericRepository<Answer>(db);
        }
        //Các interface repo được thêm như property
        public IGenericRepository<Quiz> QuizRepository { get; private set; }
        public IGenericRepository<Question> QuestionRepository { get; private set; }
        public IGenericRepository<UserQuiz> UserQuizRepository { get; private set; }
        public IGenericRepository<QuizQuestion> QuizQuestionRepository { get; private set; }
        public IGenericRepository<UserAnswer> UserAnswerRepository { get; private set; }
        public IGenericRepository<User> UserRepository { get; private set; }
        public IGenericRepository<Role> RoleRepository { get; private set; }
        public IGenericRepository<Answer> AnswerRepository { get; private set; }

        /* Cách dưới đây gọi là Lazy load */
        //private IGenericRepository<Quiz> _quizRepository;
        //public IGenericRepository<Quiz> QuizRepository
        //{
        //    get
        //    {
        //        if (_quizRepository == null)
        //            _quizRepository = new GenericRepository<Quiz>(_context);
        //        return _quizRepository;
        //    }
        //}

        public QuizAppDbContext Context => db;

        public IGenericRepository<TEntity> GenericRepository<TEntity>() where TEntity : class
        {
            return new GenericRepository<TEntity>(db);
        }

        public int SaveChanges()
        {
            return db.SaveChanges();
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await db.SaveChangesAsync(cancellationToken);
        }

        //Phương thức khởi tạo transaction
        public async Task BeginTransactionAsync()
        {
            if (transaction == null)
            {
                transaction = await db.Database.BeginTransactionAsync();
            }
        }

        //Phương thức để cam kết transaction
        public async Task CommitTransactionAsync()
        {
            try
            {
                await db.SaveChangesAsync();
                if (transaction != null) await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
            finally
            {
                if (transaction != null)
                {
                    await transaction.DisposeAsync();
                    transaction = null;
                }
            }
        }

        //Phương thức để rollback transaction
        public async Task RollbackTransactionAsync()
        {
            if (transaction != null)
            {
                await transaction.RollbackAsync();
                await transaction.DisposeAsync();
                transaction = null;
            }
        }

        //Giải phóng
        public void Dispose()
        {
            db.Dispose();
        }
    }
}
