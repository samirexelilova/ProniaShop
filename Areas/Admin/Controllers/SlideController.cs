using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProniaShop.DAL;
using ProniaShop.Models;
using ProniaShop.Utilities.Enums;
using ProniaShop.Utilities.Extensions;
using ProniaShop.ViewModels;

namespace ProniaShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class SlideController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SlideController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<GetSlideVM> slideVMs = await _context.Slides.OrderBy(s=>s.Order).Select(s=>
            new GetSlideVM
            {
                Id=s.Id,
                Title=s.Title,
                Image=s.Image,
                Order=s.Order,
                CreatedAt=s.CreatedAt,
            }

            ).ToListAsync();
            TempData["Message"] = "Ugurla yarandi mehsul";
            return View(slideVMs);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateSlideVM slideVM)
        {
            //if (!ModelState.IsValid)
            //{
            //    return View();
            //}

            bool title = await _context.Slides.AnyAsync(s => s.Title == slideVM.Title);
            if (title)
            {
                ModelState.AddModelError(nameof(CreateSlideVM.Title), $"{slideVM.Title} Bu title artiq movcuddur");
                return View();
            }
            bool order = await _context.Slides.AnyAsync(s => s.Order == slideVM.Order);
            if (order)
            {
                ModelState.AddModelError(nameof(CreateSlideVM.Order), $"{slideVM.Order} Bu order artiq movcuddur");
                return View();
            }
            if (slideVM.Photo == null)
            {
                ModelState.AddModelError(nameof(CreateSlideVM.Photo), "Zəhmət olmasa şəkil seçin.");
                return View();
            }

            if (!slideVM.Photo.ValidateType("image/"))
            {
                ModelState.AddModelError(nameof(CreateSlideVM.Photo), " Bu File Type duzgun deyil");
                return View();
            }
            if (!slideVM.Photo.ValidateSize(FileSize.MB, 1))
            {
                ModelState.AddModelError(nameof(CreateSlideVM.Photo), " File size 2 mb dan boyuk ola bilmez ");
                return View();
            }


            string fileName = await slideVM.Photo.CreateFileAsync(_env.WebRootPath, "assets", "images", "website-images");

            Slide slide = new Slide()
            {
                Title = slideVM.Title,
                SubTitle = slideVM.SubTitle,
                Description = slideVM.Description,
                Order = slideVM.Order,
                Image=fileName,
                CreatedAt = DateTime.Now
            };

            await _context.Slides.AddAsync(slide);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null || id <= 0) return BadRequest();
            Slide? slide = await _context.Slides.FirstOrDefaultAsync(s => s.Id == id);
            if (id is null) return NotFound();

            slide.Image.DeleteFile(_env.WebRootPath,"assets", "iamges", "website-images");
            _context.Remove(slide);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id is null || id <= 0) return BadRequest();
            Slide? slide = await _context.Slides.FirstOrDefaultAsync(s => s.Id == id);
            if (slide is null) return NotFound();
            UpdateSlideVM updateSlideVM = new UpdateSlideVM
            {
                Title=slide.Title,
                Description=slide.Description,
                SubTitle=slide.SubTitle,
                Order=slide.Order,
                Image=slide.Image,
            };
            return View(updateSlideVM);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int? id,UpdateSlideVM slideVM)
        {
            if (!ModelState.IsValid)
            {
                return View(slideVM);
            }
            Slide? existed = await _context.Slides.FirstOrDefaultAsync(s => s.Id == id);
            if (existed is null) return NotFound();
            if (slideVM.Photo is not null)
            {
                if (!slideVM.Photo.ValidateType("image/"))
                {
                    ModelState.AddModelError(nameof(UpdateSlideVM.Photo), "Sekil type duzgun deyil");
                    return View(slideVM);
                }
                if (!slideVM.Photo.ValidateSize(FileSize.MB,1))
                {
                    ModelState.AddModelError(nameof(UpdateSlideVM.Photo), "Sekil 1 mb dan boyuk ola bilmez");
                    return View(slideVM);
                }
               string fileName= await slideVM.Photo.CreateFileAsync(_env.WebRootPath, "assets", "images", "website-images");
                existed.Image.DeleteFile(_env.WebRootPath, "assets", "images", "website-images");
                existed.Image = fileName;
            }
            existed.Title= slideVM.Title;
            existed.SubTitle= slideVM.SubTitle;
            existed.Description= slideVM.Description;
            existed.Order= slideVM.Order;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


    }
}
