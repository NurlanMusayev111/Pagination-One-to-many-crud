using FiorelloSlider_OnetoMany.Services.Interfaces;
using FiorelloSlider_OnetoMany.ViewModels;
using FiorelloSlider_OnetoMany.ViewModels.Baskets;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace FiorelloSlider_OnetoMany.ViewComponents
{
    public class HeaderViewComponent : ViewComponent
    {

        private readonly ISettingService _settingService;
        private readonly IHttpContextAccessor _accessor;

        public HeaderViewComponent(ISettingService settingService, 
                                   IHttpContextAccessor contextAccessor)
        {
            _settingService = settingService;
            _accessor = contextAccessor;

        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            Dictionary<string,string> settingDatas  = await _settingService.GetAllAsync();

            List<BasketVM> basketProducts = new();

            if (_accessor.HttpContext.Request.Cookies["basket"] is not null)
            {
                basketProducts = JsonConvert.DeserializeObject<List<BasketVM>>(_accessor.HttpContext.Request.Cookies["basket"]);
            }

            HeaderVM response = new()
            {
                Settings = settingDatas,
                BasketCount = basketProducts.Sum(m=>m.Count),
                BasketTotalPrice = basketProducts.Sum(m=>m.Count * m.Price)
            };


            return await Task.FromResult(View(response));


        }

    }
}
