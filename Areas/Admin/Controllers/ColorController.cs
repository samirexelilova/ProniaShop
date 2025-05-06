using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProniaShop.DAL;
using ProniaShop.Models;

namespace ProniaShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ColorController : Controller
    {
        public readonly AppDbContext _context;
        public ColorController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            List<Color> colors = await _context.Colors.ToListAsync();
            return View(colors);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Color color)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            bool result = await _context.Colors.AnyAsync(c => c.Name == color.Name);
            if (result)
            {
                ModelState.AddModelError(nameof(Color.Name), $"{color.Name} -bu color artiq movcuddur");
                return View();
            }
            color.CreatedAt = DateTime.Now;
            _context.Colors.Add(color);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));

        }

        public IActionResult Update(int? id)
        {
            if (id is null || id <= 0)
            {
                return BadRequest();
            }
            Color? color = _context.Colors.FirstOrDefault(c => c.Id == id);
            if (color is null) return NotFound();
            return View(color);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int? id, Color color)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            bool result = await _context.Colors.AnyAsync(c => c.Name == color.Name);
            if (result)
            {
                ModelState.AddModelError(nameof(Color.Name), $"{color.Name} -bu color artiq movcuddur");
                return View();
            }

            Color? existed = await _context.Colors.FirstOrDefaultAsync(c => c.Id == id);
            if (existed.Name == color.Name) return RedirectToAction(nameof(Index));
            existed.Name = color.Name;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }
    }
}
