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

        public PaginatedResult<JobPosting> JobPostings { get; set; } = new PaginatedResult<JobPosting>();

        public IEnumerable<string> JobTitles { get; set; }

        public void OnGet(string jobTitle, string location, int pageNumber = 1)
        {
            if (!string.IsNullOrEmpty(jobTitle) || !string.IsNullOrEmpty(location))
            {
                JobPostings = _jobPostingRepository.GetAllJobPostings(pageNumber, 10, jobTitle, location);
            }
            else
            {
                JobPostings.Data = new List<JobPosting>();
                JobPostings.PageNumber = pageNumber;
                JobPostings.PageSize = 10;
                JobPostings.TotalRecords = 0;

                JobTitles = _jobPostingRepository.GetDistinctJobTitles();
            }
        }
    }
}
