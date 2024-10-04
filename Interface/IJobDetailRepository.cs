using JobFinder.Models;

namespace JobFinder.Interface
{
    public interface IJobDetailRepository
    {
        public JobPosting GetJobPostingById(int id);

    }
}
