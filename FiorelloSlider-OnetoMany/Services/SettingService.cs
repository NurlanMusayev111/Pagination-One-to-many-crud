using FiorelloSlider_OnetoMany.Data;
using FiorelloSlider_OnetoMany.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FiorelloSlider_OnetoMany.Services
{
    public class SettingService : ISettingService
    {
        private readonly AppDbContext _context;

        public SettingService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Dictionary<string, string>> GetAllAsync()
        {
            return await _context.Settings.ToDictionaryAsync(m=>m.Key, m => m.Value);
        }
    }
}
