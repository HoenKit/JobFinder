using JobFinder.Data;
using JobFinder.Interface;
using JobFinder.Models;
using Microsoft.EntityFrameworkCore;

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

        public async Task<Recruiter?> GetRecruiterByUserIdAsync(string userId)
        {
            return await _context.Recruiter.FirstOrDefaultAsync(r => r.UserId == userId);
        }
    }
}
