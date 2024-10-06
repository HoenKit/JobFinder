using JobFinder.Data;
using JobFinder.Interface;
using JobFinder.Models;
using Microsoft.EntityFrameworkCore;

namespace JobFinder.Repository
{
    public class JobpostingindexRepository : IjobpostingindexRepository
    {
        private readonly ApplicationDbContext _context;
        public JobpostingindexRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public List<JobPosting> LatestJobPostings { get; set; } = new List<JobPosting>();
        public List<JobPosting> GetLatestJobPostings(int count = 4) => _context.JobPosting
              .Include(j => j.Recruiter)
              .Include(j => j.JobNature)
              .OrderByDescending(j => j.PostDate)
              .Take(count)
              .ToList();
    }
}
