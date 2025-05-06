using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProniaShop.DAL;
using ProniaShop.Models;

namespace ProniaShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SlideController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SlideController(AppDbContext context,IWebHostEnvironment env)
        {
            _context = context;
            _env= env;
        }
        public async Task<IActionResult> Index()
        {
            List<Slide> slides = await _context.Slides.OrderBy(s=>s.Order).ToListAsync();
            return View(slides);
        }
        public IActionResult Create()
        {
            return View();  
        }
        [HttpPost]
        public async Task<IActionResult> Create(Slide slide)
        {
            //if (!ModelState.IsValid)
            //{
            //    return View();
            //}

            bool title = await _context.Slides.AnyAsync(s => s.Title == slide.Title);
            if (title)
            {
                ModelState.AddModelError(nameof(Slide.Title), $"{slide.Title} Bu title artiq movcuddur");
                return View();
            }
            bool order = await _context.Slides.AnyAsync(s => s.Order == slide.Order);
            if (order)
            {
                ModelState.AddModelError(nameof(Slide.Order), $"{slide.Order} Bu order artiq movcuddur");
                return View();
            }

            if (!slide.Photo.ContentType.Contains("image/"))
            {
                ModelState.AddModelError(nameof(Slide.Photo), " Bu File Type duzgun deyil");
                return View();
            }
            if (slide.Photo.Length>1*1024*1024)
            {
                ModelState.AddModelError(nameof(Slide.Photo), " File size 2 mb dan boyuk ola bilmez ");
                return View();
            }

            string fileName = string.Concat(Guid.NewGuid().ToString(), Path.GetExtension(slide.Photo.FileName));
            string path = Path.Combine(_env.WebRootPath,"assets", "images", "website-images", fileName);
            FileStream fileStream = new(path,FileMode.Create);
            await slide.Photo.CopyToAsync(fileStream);

            slide.Image = fileName;
            slide.CreatedAt = DateTime.Now;
            await _context.Slides.AddAsync(slide);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }
    }
}
