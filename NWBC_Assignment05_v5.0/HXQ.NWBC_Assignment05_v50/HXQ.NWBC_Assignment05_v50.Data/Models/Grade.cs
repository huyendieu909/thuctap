using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HXQ.NWBC_Assignment05_v50.Data.Models
{
    public class Grade
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Student> Students { get; set; } = new List<Student>();
    }
}
