using JobFinder.Data;
using JobFinder.Interface;
using JobFinder.Models;
using Microsoft.EntityFrameworkCore;

namespace JobFinder.Repository
{
    public class JobSeekerRepository : IJobSeekerRepository
    {
        private readonly ApplicationDbContext _context;
        public JobSeekerRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public void AddJobSeeker(JobSeeker jobSeeker)
        {
            _context.JobSeeker.Add(jobSeeker);
            _context.SaveChanges();
        }

        public async Task UpdateJobSeeker(JobSeeker jobSeeker)
        {
            if (jobSeeker == null)
            {
                throw new ArgumentNullException(nameof(jobSeeker), "JobSeeker object is null");
            }

            var existingJobSeeker = await GetJobSeekerByUserIdAsync(jobSeeker.UserId);

            if (existingJobSeeker != null)
            {
                existingJobSeeker.FirstName = jobSeeker.FirstName;
                existingJobSeeker.LastName = jobSeeker.LastName;
                existingJobSeeker.Birthday = jobSeeker.Birthday;
                existingJobSeeker.Address = jobSeeker.Address;
                existingJobSeeker.EducationLevel = jobSeeker.EducationLevel;
                existingJobSeeker.Specialized = jobSeeker.Specialized;
                existingJobSeeker.Experience = jobSeeker.Experience;
                existingJobSeeker.CV = jobSeeker.CV;
                existingJobSeeker.UserId = jobSeeker.UserId;

                _context.JobSeeker.Update(existingJobSeeker);

                await _context.SaveChangesAsync();
            }
            else
            {
                throw new InvalidOperationException("JobSeeker not found in the database");
            }
        }

        public async Task<JobSeeker?> GetJobSeekerByUserIdAsync(string userId)
        {
            return await _context.JobSeeker.FirstOrDefaultAsync(js => js.UserId == userId);
        }
    }
}


