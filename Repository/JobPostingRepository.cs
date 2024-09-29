using JobFinder.Data;
using JobFinder.Dtos;
using JobFinder.Interface;
using JobFinder.Models;
using Microsoft.EntityFrameworkCore;

namespace JobFinder.Repository
{
    public class JobPostingRepository : IJobPostingRepository
    {
        private readonly ApplicationDbContext _context;
        public JobPostingRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public PaginatedResult<JobPosting> GetAllJobPostings(int pageNumber, int pageSize)
        {
            var query = _context.JobPosting.Where(p => p.JobPostingStatus == 0)
               .Include(p => p.JobNature)
               .Include(o => o.JobPosition)
               .Include(c => c.Recruiter)
               .Include(t => t.JobType);
              
            var totalRecords = query.Count();
            var paginatedData = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            foreach (var job in paginatedData)
            {
                var locationParts = job.JobLocation.Split(',');
                job.JobLocation = string.Join(", ", locationParts.Skip(Math.Max(0, locationParts.Length - 2)));
                job.Salary = Math.Round(job.Salary, job.Salary % 1 == 0 ? 0 : 2);
            }
            return new PaginatedResult<JobPosting>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                Data = paginatedData
            };
        }
    }
}
