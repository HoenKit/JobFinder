using JobFinder.Models;

namespace JobFinder.Interface
{
    public interface IJobTypeRepository
    {
        public ICollection<JobType> GetAllJobType();
    }
}
