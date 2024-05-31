using FiorelloSlider_OnetoMany.Data;
using FiorelloSlider_OnetoMany.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FiorelloSlider_OnetoMany.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> Index(int? id)
        {
            if(id is null) return BadRequest(); 

            Product product = await _context.Products.Include(m => m.ProductImages)
                                                     .Include(m => m.Category)
                                                     .Where(m => !m.SoftDeleted)
                                                     .FirstOrDefaultAsync(m=>m.Id == id);

            if(product == null) return NotFound();

            return View(product);
        }

    }
}
