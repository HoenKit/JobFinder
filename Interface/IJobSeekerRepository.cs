using JobFinder.Models;

namespace JobFinder.Interface
{
    public interface IJobSeekerRepository
    {
        public void AddJobSeeker( JobSeeker jobSeeker );
        public void UpdateJobSeeker(JobSeeker jobSeeker );
        Task<JobSeeker?> GetJobSeekerByUserIdAsync(string userId);
    }
}
