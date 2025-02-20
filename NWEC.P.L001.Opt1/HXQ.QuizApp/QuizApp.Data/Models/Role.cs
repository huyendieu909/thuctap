using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HXQ.QuizApp.Data.Models
{
    public class Role : IdentityRole<Guid>
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public required string Description { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

        //n-n với user
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
