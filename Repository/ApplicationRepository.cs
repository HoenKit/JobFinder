using JobFinder.Data;
using JobFinder.Interface;
using JobFinder.Models;
using Microsoft.EntityFrameworkCore;

namespace JobFinder.Repository
{
    public class ApplicationRepository : IApplicationRepository
    {
        private readonly ApplicationDbContext _context;

        public ApplicationRepository(ApplicationDbContext context)
        {
            _context = context;
        }



        public async Task<Application> AddApplication(Application application)
        {  _context.Application.Add(application);
            await _context.SaveChangesAsync();
            return application;
        }

        public async Task<Application?> GetApplicationExistAsync(int jobSeekerId, int jobPostingId)
        {
            return await _context.Application
                .FirstOrDefaultAsync(a => a.JobSeekerId == jobSeekerId && a.JobPostingId == jobPostingId);
        }



        public async Task<List<JobSeeker>> GetJobSeekersByJobPostingIdAsync(int jobPostingId)
        {
            return await _context.Application
                        .Where(a => a.JobPostingId == jobPostingId)
                        .Select(a => a.JobSeeker) 
                        .ToListAsync();
        }
    }
}
