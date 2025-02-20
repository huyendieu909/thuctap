using HXQ.QuizApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HXQ.QuizApp.Business.Services
{
    public interface IAnswerService : IBaseService<Answer>
    {
        Task<bool> CheckCorrectAnswerAsync(Guid questionId, Guid answerId);
    }
}
