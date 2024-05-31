using System.ComponentModel.DataAnnotations;

namespace FiorelloSlider_OnetoMany.ViewModels.Sliders
{
    public class SliderCreateVM
    {
        [Required]
        public IFormFile Image { get; set; }
    }
}
