using JobFinder.Models;

namespace JobFinder.Interface
{
    public interface IApplicationRepository
    {
        Task<Application> AddApplication(Application application);
        Task<Application?> GetApplicationExistAsync(int jobSeekerId, int jobPostingId);
        Task<List<JobSeeker>> GetJobSeekersByJobPostingIdAsync(int jobPostingId);
    }
}
