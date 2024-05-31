using FiorelloSlider_OnetoMany.Data;
using FiorelloSlider_OnetoMany.Models;
using FiorelloSlider_OnetoMany.ViewModels;
using FiorelloSlider_OnetoMany.ViewModels.Baskets;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace FiorelloSlider_OnetoMany.Controllers
{
    public class HomeController : Controller
    {

        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _accessor;

        public HomeController(AppDbContext context,
                              IHttpContextAccessor accessor)
        {
              _context = context;
              _accessor = accessor;
        }
        public async Task<IActionResult> Index()
        {
            List<Category> categories = await _context.Categories.ToListAsync();
            List<Product> products = await _context.Products.Include(m=>m.ProductImages).ToListAsync();
            List<Blog> blogs = await _context.Blogs.Where(m=>!m.SoftDeleted).ToListAsync();

           

            HomeVM model = new()
            {
                Categories = categories,
                Products = products,
                Blogs = blogs,
            };


            return View(model);
        }


        public async Task<IActionResult> AddProductToBasket(int? id)
        {
            if (id is null) return BadRequest();

            List<BasketVM> basketProducts = null;

            if (_accessor.HttpContext.Request.Cookies["basket"] is not null)
            {
                basketProducts = JsonConvert.DeserializeObject<List<BasketVM>>(_accessor.HttpContext.Request.Cookies["basket"]);
            }
            else
            {
                basketProducts = new List<BasketVM>();
            }

            var dbProduct = await _context.Products.FirstOrDefaultAsync(m=>m.Id == (int)id);



            var existProduct = basketProducts.FirstOrDefault(m=>m.Id == (int)id);

            if(existProduct is not null)
            {
                existProduct.Count++;
            }
            else
            {
                basketProducts.Add(new BasketVM
                {
                    Id = (int)id,
                    Count = 1,
                    Price = dbProduct.Price
                });
            }
            
                
            _accessor.HttpContext.Response.Cookies.Append("basket", JsonConvert.SerializeObject(basketProducts));

            return RedirectToAction("Index");
        }
    }
}
