using JobFinder.Data;
using JobFinder.Interface;
using JobFinder.Models;
using Microsoft.EntityFrameworkCore;

namespace JobFinder.Repository
{
    public class JobDetailRepository : IJobDetailRepository
    {
        private readonly ApplicationDbContext _context;

        public JobDetailRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public JobPosting GetJobPostingById(int id)
        {
            return _context.JobPosting
                .Include(j => j.Recruiter)
                .Include(j => j.JobNature)
                .FirstOrDefault(j => j.Id == id);
        }

    }
}
