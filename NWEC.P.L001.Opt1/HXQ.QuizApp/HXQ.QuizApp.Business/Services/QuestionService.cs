using HXQ.QuizApp.Data.Models;
using HXQ.QuizApp.Data.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HXQ.QuizApp.Business.Services
{
    public class QuestionService : BaseService<Question>, IQuestionService
    {
        public QuestionService(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public async Task<IEnumerable<Question>> GetQuestionsByQuizIdAsync(Guid quizId)
        {
            var questions = await unitOfWork.QuizQuestionRepository
                .GetQuery(qq => qq.QuizId == quizId)
                .Select(qq => qq.Question)
                .ToListAsync();

            return questions;
        }
    }
}
