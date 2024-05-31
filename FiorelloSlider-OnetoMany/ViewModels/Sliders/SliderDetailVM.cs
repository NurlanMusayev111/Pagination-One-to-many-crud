using System.ComponentModel.DataAnnotations;

namespace FiorelloSlider_OnetoMany.ViewModels.Sliders
{
    public class SliderDetailVM
    {
        [Required]
        public string Image { get; set; }
        
    }
}
