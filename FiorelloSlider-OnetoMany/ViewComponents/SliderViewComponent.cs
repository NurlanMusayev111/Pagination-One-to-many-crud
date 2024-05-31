using FiorelloSlider_OnetoMany.Data;
using FiorelloSlider_OnetoMany.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FiorelloSlider_OnetoMany.ViewComponents
{
    public class SliderViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;

        public SliderViewComponent(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {

            var sliderData = new SliderVMVC
            {
                Sliders = await _context.Sliders.ToListAsync(),
                SliderInfo = await _context.SliderInfos.FirstOrDefaultAsync()
            };


            return await Task.FromResult(View(sliderData)); 


        }

    }


    public class SliderVMVC
    {
        public List<Slider> Sliders { get; set; }
        public SliderInfo SliderInfo { get; set; }
    }
}
