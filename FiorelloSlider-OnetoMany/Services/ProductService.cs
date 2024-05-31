using FiorelloSlider_OnetoMany.Data;
using FiorelloSlider_OnetoMany.Services.Interfaces;
using FiorelloSlider_OnetoMany.Models;

namespace FiorelloSlider_OnetoMany.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;

        public ProductService(AppDbContext context)
        {
            _context = context;
        }
        public async Task CreateAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();  
        }

        public async Task DeleteAsync(Product product)
        {
            _context.Products.Remove(product);

            await _context.SaveChangesAsync();
        }
    }
}
