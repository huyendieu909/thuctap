using HXQ.QuizApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HXQ.QuizApp.Business.Services
{
    public interface IQuestionService : IBaseService<Question> 
    {
        Task<IEnumerable<Question>> GetQuestionsByQuizIdAsync(Guid quizId);
    }
}
