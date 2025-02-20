using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HXQ.QuizApp.Data.Models
{
    public class Quiz
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 5)]
        public required string Title { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        [Range(1, 3600)]
        public int Duration { get; set; }

        [MaxLength(500)]
        public string? ThumbnailUrl { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

        //n-n vs question
        public ICollection<QuizQuestion> QuizQuestions { get; set; } = new List<QuizQuestion>();

        //user tham gia quiz
        public ICollection<UserQuiz> UserQuizzes { get; set; } = new List<UserQuiz>();

    }
}
