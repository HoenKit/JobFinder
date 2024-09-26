using JobFinder.Data;
using JobFinder.Interface;
using JobFinder.Models;

namespace JobFinder.Repository
{
    public class JobSeekerRepository : IJobSeekerRepository
    {
        private readonly ApplicationDbContext _context;
        public JobSeekerRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public void AddJobSeeker(JobSeeker jobSeeker)
        {
            _context.JobSeeker.Add(jobSeeker);
            _context.SaveChanges();
        }
    }
}
