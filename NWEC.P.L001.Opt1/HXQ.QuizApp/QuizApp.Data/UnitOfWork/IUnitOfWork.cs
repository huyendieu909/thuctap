using HXQ.QuizApp.Data.Context;
using HXQ.QuizApp.Data.Models;
using HXQ.QuizApp.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HXQ.QuizApp.Data.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        QuizAppDbContext Context { get; }
        /* Note: thời điểm đang làm Task 3, nhìn bên dưới toàn IGenericRepository làm cho việc tạo các interface và class riêng (mà lại ko có phương thức gì thêm) cho các repository của quiz, question và answer trở nên hơi thừa? =)) Có lẽ interface này sớm bị sửa trong các task sau. */
        IGenericRepository<Quiz> QuizRepository { get; }
        IGenericRepository<Question> QuestionRepository { get; }
        IGenericRepository<UserQuiz> UserQuizRepository { get; }
        IGenericRepository<QuizQuestion> QuizQuestionRepository { get; }
        IGenericRepository<UserAnswer> UserAnswerRepository { get; }
        IGenericRepository<User> UserRepository { get; }
        IGenericRepository<Role> RoleRepository { get; }
        IGenericRepository<Answer> AnswerRepository { get; }
        IGenericRepository<TEntity> GenericRepository<TEntity>() where TEntity : class;

        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
