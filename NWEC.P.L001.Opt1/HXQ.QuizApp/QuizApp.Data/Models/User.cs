using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HXQ.QuizApp.Data.Models
{
    public class User : IdentityUser<Guid>
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public required string FirstName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public required string LastName { get; set; }

        [Required]
        [NotMapped]
        public string DisplayName => $"{FirstName} {LastName}";

        [Required]
        public DateTime DateOfBirth { get; set; }

        [MaxLength(500)]
        public string? Avatar { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

        //n-n với role
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

        //user tham gia quiz
        public ICollection<UserQuiz> UserQuizzes { get; set; } = new List<UserQuiz>();

    }
}
