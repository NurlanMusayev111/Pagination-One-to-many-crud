using Microsoft.Build.Framework;

namespace FiorelloSlider_OnetoMany.Models
{
    public class Category : BaseEntity
    {
        [Required]
        public string Name { get; set; }
        public ICollection<Product>? Products { get; set; }

    }
}
