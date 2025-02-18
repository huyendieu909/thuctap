using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HXQ.NWBC_Assignment05_v50.Data.Models
{
    public class Student
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int GradeId { get; set; }
        [JsonIgnore]
        public Grade? Grade { get; set; }
        public ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}
