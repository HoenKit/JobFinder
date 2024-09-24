using JobFinder.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace JobFinder.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public DbSet<JobNature> JobNature { get; set; }
        public DbSet<JobPosition> JobPosition { get; set; }
        public DbSet<JobPosting> JobPosting { get; set; }
        public DbSet<JobSeeker> JobSeeker { get; set; }
        public DbSet<JobType> JobType { get; set; }
        public DbSet<Recruiter> Recruiter { get; set; }

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

            var recruiter = new IdentityRole("Recruiter");
            recruiter.NormalizedName = "Recruiter";
            builder.Entity<IdentityRole>().HasData(admin, users, recruiter);

            builder.Entity<JobPosting>(entity =>
            {
                // Configuring the SQL column type for Salary
                entity.Property(jp => jp.Salary)
                      .HasColumnType("decimal(18,2)"); 
            });

            builder.Entity<Recruiter>(entity =>
            {
                // Configuring the relationship between Recruiter and AppUser
                entity.HasOne(r => r.User)
                      .WithOne()
                      .HasForeignKey<Recruiter>(r => r.UserId);
            });

            builder.Entity<JobSeeker>(entity =>
            {
                // Configuring the one-to-one relationship between JobSeeker and AppUser
                entity.HasOne(js => js.User)
                      .WithOne()
                      .HasForeignKey<JobSeeker>(js => js.UserId);
            });

            builder.Entity<Application>(entity =>
            {
                // Configuring relationships
                entity.HasOne(a => a.JobSeeker)
                      .WithMany(js => js.Applications)
                      .HasForeignKey(a => a.JobSeekerId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(a => a.JobPosting)
                      .WithMany(jp => jp.Applications)
                      .HasForeignKey(a => a.JobPostingId)
                      .OnDelete(DeleteBehavior.Restrict); 
            });
        }
    }
}
