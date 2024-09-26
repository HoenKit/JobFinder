using JobFinder.Data;
using JobFinder.Interface;
using JobFinder.Models;

namespace JobFinder.Repository
{
    public class RecruiterRepository : IRecruiterRepository
    {
        private readonly ApplicationDbContext _context;
        public RecruiterRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public void AddRecruiter(Recruiter recruiter)
        {
            _context.Recruiter.Add(recruiter);
            _context.SaveChanges();
        }
    }
}
