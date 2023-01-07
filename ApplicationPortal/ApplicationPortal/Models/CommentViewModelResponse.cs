namespace ApplicationPortal.Models
{
    public class CommentViewModelResponse
    {
        public string Text { get; set; }
        public DateTime CreatedDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserEmail { get; set; }
        public int ProductId { get; set; }
    }
}
