using HXQ.QuizApp.Business.ViewModels;
using HXQ.QuizApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HXQ.QuizApp.Business.Services
{
    public interface IQuizService : IBaseService<Quiz>
    {
        Task<IEnumerable<Quiz>> GetQuizzesByDurationAsync(int duration);
        Task<PaginatedResult<Quiz>> GetQuizzesPageAsync(int pageIndex, int pageSize);
        Task<QuizPrepareInfoViewModel?> PrepareQuizForUserAsync(PrepareQuizViewModel prepareQuizViewModel);
        Task<QuizForTestViewModel> TakeQuizAsync(TakeQuizViewModel model);
        Task<bool> SubmitQuizAsync(QuizSubmissionViewModel model);
        Task<QuizResultViewModel> GetQuizResultAsync(GetQuizResultViewModel model);
    }
}
