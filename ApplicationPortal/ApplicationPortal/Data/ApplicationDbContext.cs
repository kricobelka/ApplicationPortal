using ApplicationPortal.Configurations;
using ApplicationPortal.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ApplicationPortal.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            //adding data/ to the db databASES(in UserService)
            //this.Users.Add(new User());
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Frequency> Frequencies { get; set; }
        public DbSet<AntennaGain> AntennaGains { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().Property(q => q.CompanyId)
                .IsRequired(false);
            
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new CompanyConfiguration());
            modelBuilder.ApplyConfiguration(new FrequencyConfiguration());
            modelBuilder.ApplyConfiguration(new AntennaGainConfiguration());
            modelBuilder.ApplyConfiguration(new CommentConfiguration());
            //if necessary, add new properties in class User,
            //then OnModelcreating
            //-----------------------

            //add DbSets after {}
            //public DbSet<>
        }
    }
}