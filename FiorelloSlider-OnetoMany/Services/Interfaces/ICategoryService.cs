using Microsoft.AspNetCore.Mvc.Rendering;

namespace FiorelloSlider_OnetoMany.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<SelectList> GetAllBySelectedAsync();

    }
}
