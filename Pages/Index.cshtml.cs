using JobFinder.Interface;
using JobFinder.Models;
using JobFinder.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JobFinder.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IJobPostingRepository _jobPostingRepository;
        private readonly IJobTypeRepository _jobTypeRepository;
        public IndexModel(IJobPostingRepository jobPostingRepository, IJobTypeRepository jobTypeRepository)
        {
            _jobPostingRepository = jobPostingRepository;
            _jobTypeRepository = jobTypeRepository;
        }

        public List<JobPosting> JobPostings { get; set; } = new List<JobPosting>();

        public IEnumerable<string> JobTitles { get; set; }
        public List<JobPosting> LatestJobPostings { get; set; } = new List<JobPosting>();
        public List<JobType> JobTypes { get; set; }
        public void OnGet(string jobTitle, string location)
        {
            JobTitles = _jobPostingRepository.GetDistinctJobTitles();
            LatestJobPostings =  _jobPostingRepository.GetLatestJobPostings();
            JobTypes = _jobTypeRepository.GetAllJobTypes();
        }

    }
}
