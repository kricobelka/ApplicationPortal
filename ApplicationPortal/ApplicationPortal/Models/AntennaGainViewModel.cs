using ApplicationPortal.Enums;
using System.ComponentModel.DataAnnotations;

namespace ApplicationPortal.Models
{
    public class AntennaGainViewModel
    {
        [Display(Name = "Antenna Gain:")]
        [Required]       
        public double Amount { get; set; }

        [Display(Name = " ")]
        [Required]
        public AntennaGainUnit AntennaGainUnit { get; set; }
    }
}
