using FiorelloSlider_OnetoMany.Data;
using FiorelloSlider_OnetoMany.Services.Interfaces;
using FiorelloSlider_OnetoMany.Models;
using FiorelloSlider_OnetoMany.ViewModels.Products;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FiorelloSlider_OnetoMany.Helpers.Extentions;

namespace FiorelloSlider_OnetoMany.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ICategoryService _categoryService;
        private readonly IWebHostEnvironment _env;
        private readonly IProductService _productService;

        public ProductController(AppDbContext context,
                                ICategoryService categoryService,
                                IWebHostEnvironment env,
                                IProductService productService)
        {
            _context = context;
            _categoryService = categoryService;
            _env = env;
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            List<Product> products = await _context.Products.Include(m => m.ProductImages)
                                                            .Include(m=>m.Category)        
                                                            .ToListAsync();

            List<ProductVM> model = products.Select(m=> new ProductVM {Id = m.Id, Name = m.Name,Description = m.Description,Price = m.Price, Category = m.Category.Name, Image = m.ProductImages.FirstOrDefault().Name }).ToList();   

            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null) return BadRequest();

            Product product = await _context.Products.Include(m=>m.ProductImages)
                                                     .Include(m=>m.Category) 
                                                     .Where(m=>!m.SoftDeleted)
                                                     .FirstOrDefaultAsync(m=>m.Id == id);

            if (product == null) return NotFound();

            List<ProductImageVM> productImages = new();

            foreach (var item in product.ProductImages)
            {
                productImages.Add(new ProductImageVM
                {
                    Image = item.Name,
                    IsMain = item.IsMain
                });
            }

            ProductDetailVM model = new()
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Category = product.Category.Name,
                Images = productImages
            };

            return View(model);
        }



        [HttpGet]

        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.Categories = await _categoryService.GetAllBySelectedAsync();

            if (id is null) return BadRequest();

            Product product = await _context.Products.Include(m => m.ProductImages)
                                                     .Include(m => m.Category)
                                                     .Where(m => !m.SoftDeleted)
                                                     .FirstOrDefaultAsync(m => m.Id == id);

            if(product is null) return NotFound();


            List<ProductImageVM> productImage = new();

            foreach (var item in product.ProductImages)
            {
                productImage.Add(new ProductImageVM
                {
                    Image = item.Name,
                    IsMain = item.IsMain
                });
            }

            return View(new ProductEditVM { Name = product.Name, Description = product.Description, Price = product.Price, Images = productImage, Category = product.Category.Name });

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductEditVM productEditVM,int? id)
        {

            if (id is null) return BadRequest();

            Product existProduct = await _context.Products.Include(m => m.ProductImages)
                                                          .Include(m => m.Category)
                                                          .Where(m => !m.SoftDeleted)
                                                          .FirstOrDefaultAsync(m => m.Id == id);

            if (existProduct is null) return NotFound();


            existProduct.Name = productEditVM.Name;
            existProduct.Description = productEditVM.Description;
            existProduct.Price = productEditVM.Price;
            existProduct.CategoryId = productEditVM.CategoryId;

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");

        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _categoryService.GetAllBySelectedAsync();
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateVM request)
        {
            ViewBag.Categories = await _categoryService.GetAllBySelectedAsync();



            foreach (var item in request.Images)
            {
                if (!item.CheckFileSize(500))
                {
                    ModelState.AddModelError("Images", "Image size is big");
                }

                if (!item.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Images", "File type must be only image");
                }


            }

            List<ProductImage> images = new();

            foreach (var item in request.Images)
            {
                string fileName = Guid.NewGuid().ToString() + "-" + item.FileName;

                string path = Path.Combine(_env.WebRootPath, "img", fileName);

                await item.SaveFileToLocalAsync(path);

                images.Add(new ProductImage
                {
                    Name = fileName
                });
                
            }

            images.FirstOrDefault().IsMain = true;  

            Product product = new()
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                CategoryId = request.CategoryId,
                ProductImages = images
            };

            await _productService.CreateAsync(product);


            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return BadRequest();

            Product product = await _context.Products.Include(m => m.ProductImages)
                                                     .Include(m => m.Category)
                                                     .Where(m => !m.SoftDeleted)
                                                     .FirstOrDefaultAsync(m => m.Id == id);

            if (product == null) return NotFound();

            foreach (var item in product.ProductImages)
            {
                string path = Path.Combine(_env.WebRootPath, "img", item.Name);

                path.DeleteFileFromLocal();
            }

            await _productService.DeleteAsync(product);

            return RedirectToAction("Index");
        }
    }
}   
