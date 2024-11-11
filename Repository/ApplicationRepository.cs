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



    /*    public async Task<List<Application>> GetApplicationByJobSeekerIdAsync(int jobSeekerId)
        {
            return await _context.Application
                        .Where(a => a.JobSeekerId == jobSeekerId)
                        .Include(a => a.JobPosting)
                        .ToListAsync();
        }*/

        public async Task<List<Application>> GetApplicationsWithDetailsByJobSeekerIdAsync(int jobSeekerId)
        {
            return await _context.Application
                .Where(a => a.JobSeekerId == jobSeekerId)
                .Include(a => a.JobPosting)    
                .ThenInclude(j => j.Recruiter)     
                .ToListAsync();
        }

        public async Task<int> GetAcceptedApplicationsCountAsync(int jobPostingId)
        {
            return await _context.Application.CountAsync(a => a.JobPostingId == jobPostingId && a.ApplicationStatus == 2);
        }

        public async Task<JobSeeker> GetJobSeekerByIdAsync(int id)
        {
            return await _context.JobSeeker
                .Include(js => js.User)
                .FirstOrDefaultAsync(js => js.Id == id);
        }

        public async Task<IEnumerable<Application>> GetApplicationsByJobSeekerIdAsync(int jobSeekerId)
        {
            return await _context.Application
                .Include(a => a.JobPosting).
                Include(js => js.JobSeeker)
                .Where(a => a.JobSeekerId == jobSeekerId)
                .ToListAsync();
        }

        public List<Application> GetApplicationsByJobSeekerId(string jobSeekerId)
        {
            return _context.Application
                .Include(a => a.JobPosting)
                .Where(a => a.JobSeekerId.Equals(jobSeekerId))
                .ToList();
        }

        public async Task<Application> GetApplicationByIdAsync(int applicationId)
        {
            return await _context.Application.FindAsync(applicationId);
        }

        public async Task UpdateApplicationAsync(Application application)
        {
            _context.Application.Update(application);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Application>> GetApplicationsByJobPosting(int jobPostingId)
        {
            return await _context.Application
                .Where(a => a.JobPostingId == jobPostingId && a.ApplicationStatus == 2)
                .Include(a => a.JobSeeker)
                .ThenInclude(js => js.User)
                .ToListAsync();
        }


        public async Task<Application> GetApplicationWithDetails(int applicationId)
        {
            return await _context.Application
                .Include(a => a.JobSeeker)
                .ThenInclude(js => js.User)
                .FirstOrDefaultAsync(a => a.Id == applicationId);
        }

        public async Task UpdateApplication(Application application)
        {
            _context.Application.Update(application);
            await _context.SaveChangesAsync();
        }
    }
}
}
