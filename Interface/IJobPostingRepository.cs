using JobFinder.Dtos;
using JobFinder.Models;

namespace JobFinder.Interface
{
    public interface IJobPostingRepository
    {
        PaginatedResult<JobPosting> GetAllJobPostings(int pageNumber, int pageSize, string[] jobTypeFilter, string[] experienceFilter, int? postedWithin, decimal? minSalary, decimal? maxSalary, int jobTypeId, string address);

        PaginatedResult<JobPosting> GetAllJobPostings(int pageNumber, int pageSize, string jobTitle, string location);

        IEnumerable<string> GetDistinctJobTitles();

        PaginatedResult<JobPosting> GetAllJobPostings(int pageNumber, int pageSize, int? selectedStatus);

        Task<bool> UpdateJobPostingAsync(JobPosting jobPosting);

        Task<JobPosting> GetJobPostingByIdAsync(int id);

        Task<bool> ToggleBanStatusAsync(JobPosting job);
        List<JobPosting> GetLatestJobPostings(int count = 5);
        Task<List<JobPosting>> GetJobPostingsByRecruiterAsync(int recruiterId);
        PaginatedResult<JobPosting> GetJobPostingsByJobType(int jobTypeId, int pageNumber, int pageSize);
    }
}
