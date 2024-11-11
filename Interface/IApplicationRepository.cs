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
        Task<int> GetAcceptedApplicationsCountAsync(int jobPostingId);
        Task<JobSeeker> GetJobSeekerByIdAsync(int id);
        Task<IEnumerable<Application>> GetApplicationsByJobSeekerIdAsync(int jobSeekerId);
        List<Application> GetApplicationsByJobSeekerId(string jobSeekerId);
        Task<Application> GetApplicationByIdAsync(int applicationId);
        Task UpdateApplicationAsync(Application application);
        Task<List<Application>> GetApplicationsByJobPosting(int jobPostingId);
        Task<Application> GetApplicationWithDetails(int applicationId);
        Task UpdateApplication(Application application);
    }
}
