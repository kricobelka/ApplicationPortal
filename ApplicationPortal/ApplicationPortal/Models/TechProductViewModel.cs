using System.ComponentModel.DataAnnotations;
namespace ApplicationPortal.Models
{
    public class TechProductViewModel
    {
        public int ProductId { get; set; }

        //писать атрибуты в модели freuqeucnyviewmodel?
        public List<FrequencyViewModel> Frequencies { get; set; } = new List<FrequencyViewModel>();

        [Display(Name = "Output power")]
        [Required(ErrorMessage = "Please insert actual output power (dbm)")]
        [MaxLength(20, ErrorMessage = "The output power cannot exceed 20 characters. ")]
        [MinLength(2, ErrorMessage = "The output power cannot be less than 2 characters.")]
        public string OutputPower { get; set; }

        //нужно ли писать required and display 
        
        public AntennaGainViewModel AntennaGain { get; set; } = new AntennaGainViewModel();
    }
}
