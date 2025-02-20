using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HXQ.QuizApp.Data.Models
{
    public class Question
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(5000, MinimumLength = 5)]
        public required string Content { get; set; }

        [Required]
        [Range(0, 5)]
        public required QuestionTypeEnum QuestionType { get; set; }

        public enum QuestionTypeEnum
        {
            MultipleChoice,
            SingleChoice,
            TrueFalse,
            FillInTheBlanks,
            ShortAnswer,
            LongAnswer
        }

        [Required]
        public bool IsActive { get; set; } = true;

        //n-n vs quiz
        public ICollection<QuizQuestion> QuizQuestions { get; set; } = new List<QuizQuestion>();

        //1-n vs answer
        public ICollection<Answer> Answers { get; set; } = new List<Answer>();



    }
}
