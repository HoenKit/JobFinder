using JobFinder.Models;

namespace JobFinder.Interface
{
    public interface IJobSeekerRepository
    {
        public void AddJobSeeker( JobSeeker jobSeeker );
        public Task UpdateJobSeeker(JobSeeker jobSeeker);
        Task<JobSeeker?> GetJobSeekerByUserIdAsync(string userId);
        JobSeeker GetJobSeekerById(string userId);
    }
}
