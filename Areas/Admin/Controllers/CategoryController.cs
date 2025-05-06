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
            List<Category> categories = await _context.Categories.Where(c=>c.IsDelete==false).Include(c => c.Products).AsNoTracking().ToListAsync();
            return View(categories);
        }
        public async Task<IActionResult> Arxiv()
        {
            List<Category> categories = await _context.Categories.Where(c => c.IsDelete == true).Include(c => c.Products).AsNoTracking().ToListAsync();
            return View(categories);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            bool result = await _context.Categories.AnyAsync(c => c.Name == category.Name);
            if (result)
            {
                ModelState.AddModelError(nameof(Category.Name), $"{category.Name} bu ad artiq movcuddur");
                return View();
            }
            category.CreatedAt = DateTime.Now;
            _context.Categories.Add(category);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Update(int? id)
        {
            if (id is null || id<=0) return BadRequest();   
            Category? category = _context.Categories.FirstOrDefault(c => c.Id == id);
            if (category is null) return NotFound();

            return View(category);


        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id,Category category)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            bool result = await _context.Categories.AnyAsync(c => c.Name == category.Name && c.Id!=id);
            if (result)
            {
                ModelState.AddModelError(nameof(Category.Name), $"{category.Name} bu adli category artiq movcuddur");
                return View();
            }
            Category? existed =await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
        
            if (existed.Name ==category.Name) return RedirectToAction(nameof(Index));
            existed.Name = category.Name;
             await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null || id <= 0) return BadRequest();
            Category? category= await _context.Categories.FirstOrDefaultAsync(c=>c.Id==id);
            if (category is null) return NotFound();

            if (category.IsDelete)
            {
                category.IsDelete = false;
            }
            else
            {
                category.IsDelete = true;
            }
            //_context.Categories.Remove(category);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
