using Microsoft.AspNetCore.Identity;

namespace ApplicationPortal.Data.Entities
{
    public class User : IdentityUser
    {
        //dbset ne nado add for user, as it exists in db
        public int? CompanyId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        public Company Company { get; set; }

        public List<Product> Products { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
