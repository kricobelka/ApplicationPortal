using System.ComponentModel.DataAnnotations;

namespace ApplicationPortal.Models
{
    public class CompanyViewModel
    {
        
        [Display(Name = "Reference no.")]
        public int CompanyId { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "The company name cannot exceed 50 characters. ")]
        [MinLength(3, ErrorMessage = "The company name cannot be less than 3 characters.")]
        [Display(Name = "Name")]
        public string CompanyName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The company address cannot exceed 150 characters.")]
        [MinLength(5,ErrorMessage = "The company address cannot be less than 5 characters.")]
        public string Address { get; set; }

        [Required]
        [MaxLength(25, ErrorMessage = "The registration number cannot exceed 50 characters. ")]
        [MinLength(5, ErrorMessage = "The registration number cannot be less than 5 characters.")]
        [Display(Name = "Business license no.")]
        public string BusinessId { get; set; }
    }
}
