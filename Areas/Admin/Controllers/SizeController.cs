using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProniaShop.DAL;
using ProniaShop.Models;

namespace ProniaShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SizeController : Controller
    {
        public readonly AppDbContext _context;
        public SizeController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            List<Size> sizes = await _context.Sizes.ToListAsync();
            return View(sizes);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Size size)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            bool result = await _context.Sizes.AnyAsync(s => s.Name == size.Name);
            if (result)
            {
                ModelState.AddModelError(nameof(Size.Name), $"{size.Name} -bu size artiq movcuddur");
                return View();
            }
            size.CreatedAt = DateTime.Now;
            _context.Sizes.Add(size);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));

        }

        public IActionResult Update(int? id)
        {
            if (id is null || id <= 0)
            {
                return BadRequest();
            }
            Size? size = _context.Sizes.FirstOrDefault(s => s.Id == id);

            if (size is null) return NotFound();
            return View(size);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id, Size size)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            bool result = await _context.Sizes.AnyAsync(s => s.Name == size.Name);
            if (result)
            {
                ModelState.AddModelError(nameof(Size.Name), $" {size.Name}- bu size artiq movcuddur");
                return View();
            }

            Size? existed = await _context.Sizes.FirstOrDefaultAsync(s => s.Id == id);
            if (existed.Name == size.Name) return RedirectToAction(nameof(Index));
            existed.Name = size.Name;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }
    }
}
