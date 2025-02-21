using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangXuanQuy.OnlinePainting.Data.Models
{
    public class Artist
    {
        [Key]
        public int ArtistId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [Required]
        public string Bio { get; set; } = null!;

        [MaxLength(255), DataType(DataType.Url)]
        public string? Website { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Date of Birth")]
        public DateTime BirthDate { get; set; }

        [Required, MaxLength(100)]
        public required string Nationality { get; set; } = null!;

        public virtual ICollection<Painting>? Paintings { get; set; }
    }
}
