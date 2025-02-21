using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangXuanQuy.OnlinePainting.Data.Models
{
    public class Painting
    {
        public Painting()
        {
            CreatedDate = DateTime.Today;
        }

        [Key]
        public int PaintingId { get; set; }

        [Required]
        [MaxLength(150)]
        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public decimal Price { get; set; }

        [DataType(DataType.Url)]
        public string? ImageUrl { get; set; }

        [DataType(DataType.Date)]
        public DateTime? CreatedDate { get; set; }


        public string? Dimensions { get; set; }

        public string? Medium { get; set; }

        [ForeignKey("Artist")]
        public int ArtistId { get; set; }
        public virtual Artist? Artist { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public virtual Category? Category { get; set; }

        //quan hệ cha con, class này là bố 
        public virtual ICollection<Comment>? Comments { get; set; }
        //public virtual Order? Order { get; set; }
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
