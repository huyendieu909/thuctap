using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HXQ.QuizApp.Business.ViewModels
{
    public class QuizSubmissionViewModel
    {
        public Guid QuizId { get; set; }
        public Guid UserId { get; set; }
        public List<UserAnswerSubmissionViewModel> UserAnswers { get; set; }
    }
}
