using FiorelloSlider_OnetoMany.Data;
using FiorelloSlider_OnetoMany.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FiorelloSlider_OnetoMany.Controllers
{
    public class BlogController : Controller
    {

        private readonly AppDbContext _context;

        public BlogController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            int count = await _context.Blogs.CountAsync();
            ViewBag.Count = count;  
            List<Blog> blogs = await _context.Blogs.Where(m => !m.SoftDeleted).Take(3).ToListAsync();
            return View(blogs);
        }



        [HttpGet]
        public async Task<IActionResult> ShowMore(int skip)
        {
            List<Blog> blogs = await _context.Blogs.Skip(3).Take(3).ToListAsync();

            return PartialView("_BlogsPartial",blogs);

        }
    }
}
