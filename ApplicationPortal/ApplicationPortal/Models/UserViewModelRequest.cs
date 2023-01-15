using System.ComponentModel.DataAnnotations;

namespace ApplicationPortal.Models
{
    public class UserViewModelRequest
    {
        [Display(Name = "First name")]
        [MaxLength(25, ErrorMessage = "The name cannot exceed 25 characters. ")]
        [MinLength(2, ErrorMessage = "The name must be filled in at least with 2 symbols.")]
        public string FirstName { get; set; }

        [Display(Name = "Last name")]
        [MaxLength(25, ErrorMessage = "The name cannot exceed 25 characters. ")]
        [MinLength(2, ErrorMessage = "The name must be filled in at least with 2 symbols.")]
        public string LastName { get; set; }

        public string UserName { get; set; }

        public DateTime? BirthDate { get; set; }

        [Display(Name = "Email")]
        [EmailAddress]
        public string UserEmail { get; set; }

        [Phone]
        public string Phone { get; set; }
        public CompanyViewModel Company { get; set; }
    }
}
