using JobFinder.Data;
using JobFinder.Interface;
using JobFinder.Models;
using Microsoft.EntityFrameworkCore;

namespace JobFinder.Repository
{
    public class JobTypeRepository : IJobTypeRepository
    {
        private readonly ApplicationDbContext _context;
        public JobTypeRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public ICollection<JobType> GetAllJobType() => _context.JobType.ToList();

        public List<JobType> GetAllJobTypes()
        {
            return _context.JobType.Include(j => j.JobPostings).ToList();
        }
    }
}
