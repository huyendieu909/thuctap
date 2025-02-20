using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HXQ.QuizApp.Business.ViewModels
{
    public class QuizResultViewModel
    {
        public Guid QuizId { get; set; }
        public Guid UserId { get; set; }
        public int CorrectAnswers { get; set; }
        public int TotalQuestions { get; set; }
        public double Score { get; set; }
    }
}
