using JobFinder.Models;

namespace JobFinder.Interface
{
    public interface IJobNatureRepository
    {
        public ICollection<JobNature> GetJobNature();
    }
}
