using JobFinder.Data;
using JobFinder.Interface;
using JobFinder.Models;

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
    }
}
