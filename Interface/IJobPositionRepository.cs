using JobFinder.Models;

namespace JobFinder.Interface
{
    public interface IJobPositionRepository
    {
        public ICollection<JobPosition> GetJobPositions();
    }
}
