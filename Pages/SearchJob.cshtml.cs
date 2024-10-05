using JobFinder.Dtos;
using JobFinder.Interface;
using JobFinder.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JobFinder.Pages
{
    public class SearchJobModel : PageModel
    {
        private readonly IJobPostingRepository _jobPostingRepository;

        public SearchJobModel(IJobPostingRepository jobPostingRepository)
        {
            _jobPostingRepository = jobPostingRepository;
        }

        public PaginatedResult<JobPosting> JobPostings { get; set; }

        public IEnumerable<string> JobTitles { get; set; }

        public void OnGet(string jobTitle, string location, int pageNumber = 1)
        {
            JobPostings = _jobPostingRepository.GetAllJobPostings(pageNumber, 10, jobTitle, location);
            JobTitles = _jobPostingRepository.GetDistinctJobTitles();
        }
    }
}
