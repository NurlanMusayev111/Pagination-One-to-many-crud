using FiorelloSlider_OnetoMany.Models;

namespace FiorelloSlider_OnetoMany.Services.Interfaces
{
    public interface IProductService
    {
        Task CreateAsync(Product product);
        Task DeleteAsync(Product product);
    }
}
