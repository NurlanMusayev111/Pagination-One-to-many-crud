using FiorelloSlider_OnetoMany.Models;

namespace FiorelloSlider_OnetoMany.ViewModels
{
    public class HomeVM
    {
        public List<Category> Categories { get; set; }
        public List<Product> Products { get; set; }
        public List<Blog> Blogs { get; set; }
    }
}
