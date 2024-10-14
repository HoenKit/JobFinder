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

        public void UpdateJobSeeker(JobSeeker jobSeeker)
        {
            if (jobSeeker == null)
            {
                throw new ArgumentNullException(nameof(jobSeeker), "JobSeeker object is null");
            }

            var existingJobSeeker = _context.JobSeeker.FirstOrDefault(js => js.Id == jobSeeker.Id);
            if (existingJobSeeker != null)
            {
                // Update each field
                existingJobSeeker.FirstName = jobSeeker.FirstName;
                existingJobSeeker.LastName = jobSeeker.LastName;
                existingJobSeeker.Birthday = jobSeeker.Birthday;
                existingJobSeeker.Address = jobSeeker.Address;
                existingJobSeeker.EducationLevel = jobSeeker.EducationLevel;
                existingJobSeeker.Specialized = jobSeeker.Specialized;
                existingJobSeeker.Experience = jobSeeker.Experience;
                /*   existingJobSeeker.CV = jobSeeker.CV;
                   existingJobSeeker.JobPositionId = jobSeeker.JobPositionId;
                */
                existingJobSeeker.UserId = jobSeeker.UserId;

                // Save changes to the database
                _context.SaveChanges();
            }
            else
            {
                throw new InvalidOperationException("JobSeeker not found in the database");
            }
        }

        public async Task<JobSeeker> GetJobSeekerByUserIdAsync(string userId)
        {
            return await _context.JobSeeker
                                 .FirstOrDefaultAsync(js => js.UserId == userId);
        }
    }
}
