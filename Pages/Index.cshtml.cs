using JobFinder.Interface;
using JobFinder.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JobFinder.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IJobPostingRepository _jobPostingRepository;

        public IndexModel(IJobPostingRepository jobPostingRepository)
        {
            _jobPostingRepository = jobPostingRepository;
        }

        public List<JobPosting> JobPostings { get; set; } = new List<JobPosting>();

        public IEnumerable<string> JobTitles { get; set; }

        public void OnGet(string jobTitle, string location)
        {
            JobTitles = _jobPostingRepository.GetDistinctJobTitles();
        }
    }
}
