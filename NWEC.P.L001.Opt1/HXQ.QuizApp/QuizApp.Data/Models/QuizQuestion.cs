using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HXQ.QuizApp.Data.Models
{
    public class QuizQuestion
    {
        public Guid Id { get; set; }
        public Guid QuizId { get; set; }
        public Quiz Quiz { get; set; } 
        public Guid QuestionId { get; set; }
        public Question Question { get; set; }

        public int Order { get; set; }
    }
}
