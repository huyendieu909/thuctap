using HoangXuanQuy.OnlinePainting.Data.Context;
using HoangXuanQuy.OnlinePainting.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace HoangXuanQuy.OnlinePainting.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly OnlinePaintingContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(OnlinePaintingContext context, ILogger<HomeController> logger)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var paintings = await _context.Paintings.Include(x => x.Artist).ToListAsync();
            var homePageViewModel = new HomepageViewModel
            {
                Paintings = paintings.Select(x => new PaintingViewModel
                {
                    PaintingId = x.PaintingId,
                    Title = x.Title,
                    Description = x.Description,
                    Price = x.Price,
                    ImageUrl = x.ImageUrl,
                    CreatedDate = x.CreatedDate,
                    Dimensions = x.Dimensions,
                    Medium = x.Medium,
                    ArtistId = x.ArtistId,
                    Artist = new ArtistViewModel
                    {
                        ArtistId = x.Artist.ArtistId,
                        Name = x.Artist.Name,
                        Bio = x.Artist.Bio,
                        Website = x.Artist.Website,
                        BirthDate = x.Artist.BirthDate,
                    },
                    CategoryId = x.CategoryId,
                    Category = x.Category,
                    Comments = x.Comments.ToList(),
                    Orders = x.Orders.ToList()
                }).ToList()
            };
            return View(homePageViewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
