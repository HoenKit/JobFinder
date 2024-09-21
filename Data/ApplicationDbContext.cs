using JobFinder.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JobFinder.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            var admin = new IdentityRole("Administrator");
            admin.NormalizedName = "Administrator";

            var users = new IdentityRole("Users");
            users.NormalizedName = "Users";

            var company = new IdentityRole("Company");
            company.NormalizedName = "Company";
            builder.Entity<IdentityRole>().HasData(admin, users, company);
        }
    }
}
