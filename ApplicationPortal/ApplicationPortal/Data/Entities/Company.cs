namespace ApplicationPortal.Data.Entities
{
    public class Company
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string BusinessId { get; set; }

        public ICollection<User> Users { get; set; }
    }
}
