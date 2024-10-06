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
        private readonly IjobpostingindexRepository _jobpostingindexRepository;
        private readonly IJobTypeRepository _jobTypeRepository;
        public IndexModel(IJobPostingRepository jobPostingRepository, IjobpostingindexRepository jobpostingindexRepository, IJobTypeRepository jobTypeRepository)
        {
            _jobPostingRepository = jobPostingRepository;
            _jobpostingindexRepository = jobpostingindexRepository;
            _jobTypeRepository = jobTypeRepository;
        }

        public List<JobPosting> JobPostings { get; set; } = new List<JobPosting>();

        public IEnumerable<string> JobTitles { get; set; }
        public List<JobPosting> LatestJobPostings { get; set; } = new List<JobPosting>();
        public void OnGet(string jobTitle, string location)
        {
            JobTitles = _jobPostingRepository.GetDistinctJobTitles();
            LatestJobPostings =  _jobpostingindexRepository.GetLatestJobPostings();
        }

    }
}
