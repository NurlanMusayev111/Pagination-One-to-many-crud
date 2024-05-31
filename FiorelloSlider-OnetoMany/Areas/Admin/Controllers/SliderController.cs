using FiorelloSlider_OnetoMany.Data;
using FiorelloSlider_OnetoMany.Models;
using FiorelloSlider_OnetoMany.ViewModels.Sliders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace FiorelloSlider_OnetoMany.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SliderController : Controller
    {

        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SliderController(AppDbContext context,
                                IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {

            List<Slider> sliders = await _context.Sliders.ToListAsync();

            List<SliderVM> result =  sliders.Select(m => new SliderVM { Id = m.Id, Image = m.Image }).ToList();

            return View(result);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SliderCreateVM request)
        {
            if (!ModelState.IsValid) return View();

            if (!request.Image.ContentType.Contains("image/"))
            {
                ModelState.AddModelError("Image", "File must be only image");
                return View();
            }

            if(request.Image.Length/1024 > 200)
            {
                ModelState.AddModelError("Image", "Image size is big");
                return View();
            }

            string fileName = Guid.NewGuid().ToString() + "-" + request.Image.FileName;

            string path = Path.Combine(_env.WebRootPath, "img", fileName);


            using(FileStream stream = new(path, FileMode.Create))
            {
                await request.Image.CopyToAsync(stream);
            }

            await _context.Sliders.AddAsync(new Slider { Image = fileName });
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");


        }


        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null) return BadRequest();

            Slider slider = await _context.Sliders.FirstOrDefaultAsync(m => m.Id == id);

            if (slider is null) return NotFound();

            SliderDetailVM model = new()
            {
                Image = slider.Image
            };

            return View(model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return BadRequest();

            var slider = await _context.Sliders.FirstOrDefaultAsync(m=>m.Id == id);

            if(slider is null) return NotFound();

            string path = Path.Combine(_env.WebRootPath, "img", slider.Image);

                        
            if(System.IO.File.Exists(path))
               System.IO.File.Delete(path);


            _context.Sliders.Remove(slider);

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");                   
                
        }




        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return BadRequest();

            var slider = await _context.Sliders.FirstOrDefaultAsync(m => m.Id == id);

            if(slider is null) return NotFound();   


            return View(new SliderEditVM { Image = slider.Image});
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id,SliderEditVM request)
        {
            if (id is null) return BadRequest();

            var slider = await _context.Sliders.FirstOrDefaultAsync(m => m.Id == id);

            if (slider is null) return NotFound();

            if (request.NewImage is null) return RedirectToAction("Index");


            if (!request.NewImage.ContentType.Contains("image/"))
            {
                ModelState.AddModelError("NewImage", "File must be only image");
                request.Image = slider.Image;
                return View(request);
            }

            if (request.NewImage.Length / 1024 > 200)
            {
                ModelState.AddModelError("NewImage", "Image size is big");
                request.Image = slider.Image;
                return View(request);
            }


            string oldPath = Path.Combine(_env.WebRootPath, "img", slider.Image);

            if(System.IO.File.Exists(oldPath))
               System.IO.File.Delete(oldPath);

            string fileName = Guid.NewGuid().ToString() + "-" + request.NewImage.FileName;

            string newPath = Path.Combine(_env.WebRootPath,"img",fileName);

            using (FileStream stream = new(newPath, FileMode.Create))
            {
                await request.NewImage.CopyToAsync(stream);
            }

            slider.Image = fileName;

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        
    }
}
