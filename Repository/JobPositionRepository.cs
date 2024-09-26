using JobFinder.Data;
using JobFinder.Interface;
using JobFinder.Models;

namespace JobFinder.Repository
{
    public class JobPositionRepository : IJobPositionRepository
    {
        private readonly ApplicationDbContext _context;
        public JobPositionRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public ICollection<JobPosition> GetJobPositions() => _context.JobPosition.ToList();
    }
}
