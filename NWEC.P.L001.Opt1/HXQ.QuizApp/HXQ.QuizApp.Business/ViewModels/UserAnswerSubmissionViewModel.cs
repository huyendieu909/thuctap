using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HXQ.QuizApp.Business.ViewModels
{
    public class UserAnswerSubmissionViewModel
    {
        public Guid QuestionId { get; set; }
        public Guid AnswerId { get; set; }
    }
}
