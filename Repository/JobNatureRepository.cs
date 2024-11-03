using JobFinder.Interface;
using JobFinder.Data;
using JobFinder.Models;

namespace JobFinder.Repository
{
    public class JobNatureRepository : IJobNatureRepository
    {
        private readonly ApplicationDbContext _context;

        public JobNatureRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public ICollection<JobNature> GetJobNature() => _context.JobNature.ToList();
    }
}
