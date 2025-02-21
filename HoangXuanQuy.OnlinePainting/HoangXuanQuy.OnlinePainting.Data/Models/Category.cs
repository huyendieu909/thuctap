using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangXuanQuy.OnlinePainting.Data.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public virtual ICollection<Painting>? Paintings { get; set; }
    }
}
