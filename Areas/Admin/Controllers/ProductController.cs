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

    public class ProductController : Controller
    {
        public readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductController(AppDbContext context,IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
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
        public async Task<IActionResult> Create()
        {
            CreateProductVM createProductVM = new CreateProductVM
            {
               Categories= await _context.Categories.ToListAsync()
            };
            return View(createProductVM);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateProductVM createProductVM)
        {

            createProductVM.Categories = await _context.Categories.ToListAsync();
             
            if (!ModelState.IsValid)
            {
                return View(createProductVM);
            }
            bool result = createProductVM.Categories.Any(c=>c.Id==createProductVM.CategoryId);
            if (!result)
            {
                ModelState.AddModelError(nameof(CreateProductVM.CategoryId), $"{createProductVM.Name} bu category movcud deyil");
                return View(createProductVM);
            }
            if (!createProductVM.MainPhoto.ValidateType("image/"))
            {
                ModelState.AddModelError(nameof(CreateProductVM.MainPhoto), "File type duzgun deyil");
                return View(createProductVM);
            }
            if (!createProductVM.MainPhoto.ValidateSize(FileSize.KB,500))
            {
                ModelState.AddModelError(nameof(CreateProductVM.MainPhoto), "File size duzgun deyil");
                return View(createProductVM);
            }
            bool nameResult = await _context.Products.AnyAsync(p=>p.Name==createProductVM.Name);
            if (nameResult)
            {
                ModelState.AddModelError(nameof(CreateProductVM.Name), $"{createProductVM.Name} bu adli mehsul artiq movcuddur");
                return View(createProductVM);
            }
            ProductImage main = new ProductImage
            {
                Image = await createProductVM.MainPhoto.CreateFileAsync(_env.WebRootPath,"assets","images","website-images"),
                IsPrimary = true,
                CreatedAt= DateTime.Now
            };
            Product product = new Product
            {
                Name = createProductVM.Name,
                Price = createProductVM.Price.Value, //veya (decimal)
                SKU = createProductVM.SKU,
                Description = createProductVM.Description,
                CategoryId=createProductVM.CategoryId.Value,
                ProductImages=new List<ProductImage>() ,
                CreatedAt = DateTime.Now
            };
            product.ProductImages.Add(main);
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id is null || id <= 0) return BadRequest();
            Product? product = await _context.Products.Include(p=>p.ProductImages).FirstOrDefaultAsync(c => c.Id == id);
            if (product is null) return NotFound();
            UpdateProductVM updateProductVM = new UpdateProductVM
            {
                Name = product.Name,
                SKU = product.SKU,
                Description = product.Description,
                Price = product.Price,
                CategoryId = product.CategoryId,
                PrimaryImage = product.ProductImages.FirstOrDefault(pi=>pi.IsPrimary==true).Image,
                Categories= await _context.Categories.ToListAsync()
            };
            return View(updateProductVM);


        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id, UpdateProductVM updateProductVM)
        {
            updateProductVM.Categories = await _context.Categories.ToListAsync();
            if (!ModelState.IsValid)
            {
                return View(updateProductVM);
            }
            if (updateProductVM.MainPhoto is not null)
            {
                if (!updateProductVM.MainPhoto.ValidateType("image/"))
                {
                    ModelState.AddModelError(nameof(UpdateProductVM.MainPhoto), "File type duzgun deyil");
                    return View(updateProductVM);
                }
                if (!updateProductVM.MainPhoto.ValidateSize(FileSize.KB, 500))
                {
                    ModelState.AddModelError(nameof(UpdateProductVM.MainPhoto), "File size duzgun deyil");
                    return View(updateProductVM);
                }
            }
            bool result = updateProductVM.Categories.Any(c => c.Id == updateProductVM.CategoryId);
            if (!result)
            {
                ModelState.AddModelError(nameof(UpdateProductVM.CategoryId), $"{updateProductVM.Name} bu category movcud deyil");
                return View(updateProductVM);
            }
            bool nameResult = await _context.Products.AnyAsync(p => p.Name == updateProductVM.Name && p.Id!=id);
            if (nameResult)
            {
                ModelState.AddModelError(nameof(UpdateProductVM.Name), $"{updateProductVM.Name} bu adli mehsul artiq movcuddur");
                return View(updateProductVM);
            }

            Product? existed= await _context.Products.Include(p=>p.ProductImages).FirstOrDefaultAsync(p => p.Id==id);

            if (updateProductVM.MainPhoto is not null)
            {
                ProductImage main = new ProductImage
                {
                    Image = await updateProductVM.MainPhoto.CreateFileAsync(_env.WebRootPath, "assets", "images", "website-images"),
                    IsPrimary = true,
                    CreatedAt = DateTime.Now
                };

                ProductImage? existedMain = existed.ProductImages.FirstOrDefault(pi => pi.IsPrimary == true);
                existedMain.Image.DeleteFile(_env.WebRootPath, "assets", "images", "website-images");
                existed.ProductImages.Remove(existedMain);
                existed.ProductImages.Add(main);
            }
            existed.Name= updateProductVM.Name;
            existed.SKU= updateProductVM.SKU;
            existed.Description= updateProductVM.Description;
            existed.Price=updateProductVM.Price.Value;
            existed.CategoryId = updateProductVM.CategoryId.Value;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null || id <= 0) return BadRequest();
            Product? product = await _context.Products.Include(p => p.ProductImages).FirstOrDefaultAsync(c => c.Id == id);
            if (product is null) return NotFound();
            foreach (ProductImage item in product.ProductImages)
            {
                item.Image.DeleteFile(_env.WebRootPath, "assets", "images", "website-images");
            }
            _context.Products.Remove(product);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));

        }
    }
}
