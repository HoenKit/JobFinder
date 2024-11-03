using JobFinder.Dtos;
using JobFinder.Models;
using System.Threading.Tasks;

namespace JobFinder.Interface
{
    public interface IApplicationRepository
    {
        Task<Application> AddApplication(Application application);
        Task<Application?> GetApplicationExistAsync(int jobSeekerId, int jobPostingId);
        Task<List<JobSeeker>> GetJobSeekersByJobPostingIdAsync(int jobPostingId);

        // Get Application by JobSeekerId
        /*Task<List<Application>> GetApplicationByJobSeekerIdAsync(int jobSeekerId);*/
        Task<List<Application>> GetApplicationsWithDetailsByJobSeekerIdAsync(int jobSeekerId);
    }
}
