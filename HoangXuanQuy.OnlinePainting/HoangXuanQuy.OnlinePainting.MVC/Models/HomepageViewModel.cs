using HoangXuanQuy.OnlinePainting.Data.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace HoangXuanQuy.OnlinePainting.MVC.Models
{
    public class HomepageViewModel
    {
        public List<PaintingViewModel> Paintings { get; set; } = new List<PaintingViewModel>();
        public List<Painting> MostViewedPaintings { get; set; } = new List<Painting>();
        public List<Painting> MostLikedPaintings { get; set; } = new List<Painting>();
        public List<Painting> MostCommentedPaintings { get; set; } = new List<Painting>();
    }

    public class PaintingViewModel
    {
        public int PaintingId { get; set; }
        public string? Title { get; set; } 
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? Dimensions { get; set; }
        public string? Medium { get; set; }
        public int? ArtistId { get; set; }
        public ArtistViewModel? Artist { get; set; }
        public int? CategoryId { get; set; }
        public virtual Category? Category { get; set; }

        //quan hệ cha con, class này là bố 
        public List<Comment>? Comments { get; set; }
        //public virtual Order? Order { get; set; }
        public List<Order> Orders { get; set; } = new List<Order>();
    }

    public class ArtistViewModel
    {
        public int ArtistId { get; set; }
        public string? Name { get; set; }
        public string? Bio { get; set; }
        public string? Website { get; set; }
        public DateTime BirthDate { get; set; }
        public string? Nationality { get; set; }
        public List<PaintingViewModel> Paintings { get; set; } = new List<PaintingViewModel>();
    }
}
