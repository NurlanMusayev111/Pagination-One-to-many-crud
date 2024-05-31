using FiorelloSlider_OnetoMany.Data;
using FiorelloSlider_OnetoMany.Models;
using FiorelloSlider_OnetoMany.ViewModels.Categories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace FiorelloSlider_OnetoMany.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<Category> categories = await _context.Categories.OrderByDescending(m=>m.Id).ToListAsync();

            List<CategoryVM> model = categories.Select(m => new CategoryVM { Id = m.Id, Name = m.Name }).ToList();
            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryCreateVM categoryVM)
        {

            if (!ModelState.IsValid)
            {
                return View();
            }

            Category category = new()
            {
                Name = categoryVM.Name
            };

            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");

        }

        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null) return BadRequest();

            Category? category = await _context.Categories.Where(m => m.Id == id)
                                                         .Include(m => m.Products)
                                                         .FirstOrDefaultAsync();

            if (category is null) return NotFound();

            CategoryDetailVM model = new()
            {
                Name = category.Name,
                ProductCount = category.Products.Count()
            };

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return BadRequest();

            Category? category = await _context.Categories.Where(m => m.Id == id)
                                                         .Include(m => m.Products)
                                                         .FirstOrDefaultAsync();

            if (category is null) return NotFound();

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return BadRequest();

            Category? category = await _context.Categories.Where(m=>m.Id == id)
                                                         .FirstOrDefaultAsync();

            if(category is null) return NotFound();

            return View(new CategoryEditVM { Id = category.Id,Name = category.Name });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, CategoryEditVM category)
        {

            if(!ModelState.IsValid)
            {
                return View();
            }


            if (id is null) return BadRequest();

            Category? existCategory = await _context.Categories.Where(m => m.Id == id)
                                                               .FirstOrDefaultAsync();

            if (existCategory is null) return NotFound();

            existCategory.Name = category.Name;

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }


    }
}
