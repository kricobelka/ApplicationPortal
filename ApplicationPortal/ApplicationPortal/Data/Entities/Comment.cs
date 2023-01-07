namespace ApplicationPortal.Data.Entities
{
    public class Comment
    {
        public int CommentId { get; set; }
        public string Text { get; set; }
        public DateTime CreatedDate { get; set; }

        public User User { get; set; }
        public string UserId { get; set; }

        public Product Product { get; set; }
        public int ProductId { get; set; }
    }
}