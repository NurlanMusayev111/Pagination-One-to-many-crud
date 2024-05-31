using System.ComponentModel.DataAnnotations;

namespace FiorelloSlider_OnetoMany.ViewModels.Categories
{
    public class CategoryCreateVM
    {
        [Required]
        public string Name { get; set; }
    }
}
