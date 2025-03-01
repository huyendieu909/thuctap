using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCareAppointment.Models
{
    public class Users
    {
        [Key]
        public Guid Id { get; set; }

        [Required, StringLength(255, MinimumLength = 3)]
        public String Name { get; set; } = null!;

        [Required, EmailAddress, StringLength(255, MinimumLength = 5)]
        public String Email { get; set; } = null!;

        public DateTime? DateOfBirth { get; set; }

        [Required, PasswordPropertyText, MinLength(8)]
        public string Password { get; set; } = null!;
        
        public enum RoleEnum
        {
            Patient,
            Doctor
        }
        public RoleEnum Role { get; set; }

        [Required, StringLength(255, MinimumLength = 3)]
        public string Specialization { get; set; } = null!;

        public virtual ICollection<Appointment>? AppointmentsAsPatient { get; set; }
        public virtual ICollection<Appointment>? AppointmentsAsDoctor { get; set; }
    }
}
