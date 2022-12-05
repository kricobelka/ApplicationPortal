using Microsoft.AspNetCore.Identity;

namespace ApplicationPortal.Data.Entities
{
    public class User : IdentityUser
    {
        //dbset ne nado add for user, as it exists in db
        public int ?CompanyId { get; set; }
        public Company Company { get; set; }
    }
}
