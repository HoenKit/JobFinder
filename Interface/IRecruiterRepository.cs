using JobFinder.Models;

namespace JobFinder.Interface
{
    public interface IRecruiterRepository
    {
        public void AddRecruiter(Recruiter recruiter);

        Task<Recruiter?> GetRecruiterByUserIdAsync(string userId);

        Task UpdateCompanyAsync(Recruiter Recruiter);
    }
}
