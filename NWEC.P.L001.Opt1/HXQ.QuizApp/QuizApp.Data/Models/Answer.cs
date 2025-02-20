using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HXQ.QuizApp.Data.Models
{
    public class Answer
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 5)]
        public required string Content { get; set; }

        [Required]
        public bool IsCorrect { get; set; } = false;

        [Required]
        public bool IsActive { get; set; } = true;

        //1-1 vs question
        public Guid QuestionId { get; set; }
        public Question Question { get; set; }
    }
}
