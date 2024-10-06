using JobFinder.Models;

namespace JobFinder.Interface
{
    public interface IjobpostingindexRepository
    {
        List<JobPosting> GetLatestJobPostings(int count =4);
    }
}
