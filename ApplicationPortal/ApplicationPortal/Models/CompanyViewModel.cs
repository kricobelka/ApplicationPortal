using System.ComponentModel.DataAnnotations;

namespace ApplicationPortal.Models
{
    public class CompanyViewModel
    {
        [Display(Name = "Reference company number")]
        public int CompanyId { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "The company name cannot exceed 50 characters. ")]
        [MinLength(3, ErrorMessage = "The company name cannot be less than 3 characters.")]
        [Display(Name = "Company name")]
        public string CompanyName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The company address cannot exceed 150 characters.")]
        [MinLength(5,ErrorMessage = "The company address cannot be less than 5 characters.")]
        public string Address { get; set; }

        [Required]
        [MaxLength(25, ErrorMessage = "The registration number cannot exceed 50 characters. ")]
        [MinLength(5, ErrorMessage = "The registration number cannot be less than 5 characters.")]
        [Display(Name = "Business license number/ registration number")]
        public string BusinessId { get; set; }
    }
}
