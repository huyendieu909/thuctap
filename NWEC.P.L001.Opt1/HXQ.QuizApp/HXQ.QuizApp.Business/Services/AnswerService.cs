using HXQ.QuizApp.Data.Models;
using HXQ.QuizApp.Data.UnitOfWork;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HXQ.QuizApp.Business.Services
{
    public class AnswerService : BaseService<Answer>, IAnswerService
    {
        public AnswerService(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public async Task<bool> CheckCorrectAnswerAsync(Guid questionId, Guid answerId)
        {
            var answer = await unitOfWork.AnswerRepository.GetByIdAsync(answerId);
            return answer != null && answer.QuestionId == questionId && answer.IsCorrect;
        }
    }
}
