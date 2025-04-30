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
            
            HomeVM homeVM = new HomeVM()
            {
                Slides = _context.Slides.OrderBy(s=>s.Order).ToList(),
                Products=_context.Products.Take(4).Include(p => p.ProductImages.Where(pi => pi.IsPrimary != null)).ToList()
            };


            return View(homeVM);
        }
    }
}
