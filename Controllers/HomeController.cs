using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProniaShop.DAL;
using ProniaShop.Models;
using ProniaShop.ViewModels;

namespace ProniaShop.Controllers
{
    public class HomeController : Controller
    {
        public readonly AppDbContext _context;
        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<Slide> slides = new List<Slide>
            {
                new Slide{
                Title= "Bashliq 1",
                SubTitle="Komekci basliq 1",
                Description="Gozel guller",
                Image="1-1-524x617.png",
                Order=3,
                CreatedAt= DateTime.Now
                },
                new Slide{
                Title= "Bashliq 2",
                SubTitle="Komekci basliq 2",
                Description="Gozel agaclar",
                Image="agac.jpg",
                Order=1,
                CreatedAt= DateTime.Now
                },
                new Slide{
                Title= "Bashliq 3",
                SubTitle="Komekci basliq 3",
                Description="Gozel dibcek",
                Image="1-2-524x617.png",
                Order=2,
                CreatedAt= DateTime.Now
                }
            };


            //_context.slides.AddRange(slides);
            //_context.SaveChanges();
            HomeVM homeVM = new HomeVM()
            {
                Slides = _context.Slides.OrderBy(s=>s.Order).ToList(),
                Products=_context.Products.Include(p=>p.ProductImages.Where(pi=>pi.IsPrimary!=null)).Take(8).ToList()
            };


            return View(homeVM);
        }
    }
}
