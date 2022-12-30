using ApplicationPortal.Enums;
using System.ComponentModel.DataAnnotations;

namespace ApplicationPortal.Models
{
    public class FrequencyViewModel
    {
        public int? FrequencyId { get; set; }
        [Required(ErrorMessage = "Please insert actual frequency range")]
        [Display(Name = "Frequency: from")]
        public double StartRange { get; set; }

        [Required]
        [Display(Name = "to")]
        public double EndRange { get; set; }

        //писать имя тут или во вьюмодели frequencyunit
        [Required(ErrorMessage = "Please insert actual frequency unit")]
        [Display(Name = " ")]
        public FrequencyUnit FrequencyUnit { get; set; }
    }
}
