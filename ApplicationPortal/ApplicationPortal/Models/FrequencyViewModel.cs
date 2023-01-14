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

        [Display(Name = "to:")]
        [Required(ErrorMessage = "Please insert actual frequency range")]
        public double EndRange { get; set; }

        //писать имя тут или во вьюмодели frequencyunit
        [Display(Name = " ")]
        public FrequencyUnit FrequencyUnit { get; set; }
    }
}
