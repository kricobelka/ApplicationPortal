using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ApplicationPortal.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            //adding data/ to the db databASES(in UserService)
            //this.Users.Add(new User());
        }
        //if necessary, add new properties in class User,
        //then OnModelcreating
        //add DbSets after {}
        //public DbSet<>
    }
}