using System.ComponentModel.DataAnnotations;

namespace ApplicationPortal.Models
{
    public class CommentViewModelResponse
    {
        [MaxLength(150, ErrorMessage = "The comment cannot be more than 150 characters. ")]
        [Display(Name = "Company name")]
        public string Text { get; set; }

        public DateTime CreatedDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserEmail { get; set; }
        public int ProductId { get; set; }
    }
}
