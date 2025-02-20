using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HXQ.QuizApp.Business.ViewModels
{
    public class QuestionForTestViewModel
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public string QuestionType { get; set; }
        public List<AnswerForTestViewModel> Answers { get; set; }
    }
}
