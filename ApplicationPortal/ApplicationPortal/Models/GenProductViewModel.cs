using System.ComponentModel.DataAnnotations;

namespace ApplicationPortal.Models
{
    public class GenProductViewModel
    {
        public int? ProductId { get; set; }

        [Required(ErrorMessage = "Please type name")]
        [MaxLength(50, ErrorMessage = "The product name cannot exceed 50 characters. ")]
        [MinLength(2, ErrorMessage = "The product name cannot be less than 3 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please type model")]
        [MaxLength(30, ErrorMessage = "The model cannot exceed 50 characters. ")]
        [MinLength(2, ErrorMessage = "The model cannot be less than 3 characters.")]
        public string Model { get; set; }

        [Required(ErrorMessage = "Please type brand or N/A")]
        [MaxLength(30, ErrorMessage = "The brand cannot exceed 30 characters. ")]
        public string Brand { get; set; }

        [Required(ErrorMessage = "Please type manufacturer")]
        [MaxLength(50, ErrorMessage = "The manufacturer cannot exceed 50 characters. ")]
        [MinLength(2, ErrorMessage = "The manufacturer cannot be less than 3 characters.")]
        public string Manufacturer { get; set; }
    }
}
