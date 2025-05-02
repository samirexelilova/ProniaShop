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
        public async Task<IActionResult> Index()
        {
            
            HomeVM homeVM = new HomeVM()
            {
                Slides = await _context.Slides.OrderBy(s=>s.Order).ToListAsync(),

                Products= await _context.Products.Take(4).Include(p => p.ProductImages.Where(pi => pi.IsPrimary != null)).ToListAsync()
            };


            return View(homeVM);
        }
    }
}
