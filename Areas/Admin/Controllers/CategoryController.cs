using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProniaShop.DAL;
using ProniaShop.Models;

namespace ProniaShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            List<Category> categories=await _context.Categories.Include(c=>c.Products).ToListAsync();    
            return View(categories);
        }
    }
}
