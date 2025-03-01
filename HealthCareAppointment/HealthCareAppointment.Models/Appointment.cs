using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace HealthCareAppointment.Models
{
    public class Appointment
    {

        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid PatientId { get; set; }
        [ForeignKey("PatientId")]
        public virtual Users? Patient { get; set; } 

        [Required]
        public Guid DoctorId { get; set; }
        [ForeignKey("DoctorId")]
        public virtual Users? Doctor { get; set; }

        [Required]        
        public DateTime Date { get; set; }
        
             
        public enum StatusEnum
        {
            Scheduled,
            Completed,
            Cancelled
        }
        [Required]   
        public StatusEnum Status { get; set; }

    }
}
