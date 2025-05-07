using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProniaShop.DAL;
using ProniaShop.ViewModels;

namespace ProniaShop.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class ProductController : Controller
    {
        public readonly AppDbContext _context;
        public ProductController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            List<GetProductVM> productVMs = await _context.Products.Select(p =>
            new GetProductVM
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Description = p.Description,
                CategoryName=p.Category.Name,
                MainImage=p.ProductImages.FirstOrDefault(p=>p.IsPrimary==true).Image

            }

            ).ToListAsync();
            return View(productVMs);
        }
    }
}
