using JobFinder.Dtos;
using JobFinder.Interface;
using JobFinder.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JobFinder.Pages
{
    public class FindJobModel : PageModel
    {
        private readonly IJobPostingRepository _jobPostingRepository;
        public FindJobModel(IJobPostingRepository jobPostingRepository)
        {
            _jobPostingRepository = jobPostingRepository;
        }

        public PaginatedResult<JobPosting> JobPostings { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; } = 10;

        [BindProperty(SupportsGet = true)]
        public string[] JobTypeFilter { get; set; }

        [BindProperty(SupportsGet = true)]
        public string[] ExperienceFilter { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? postedWithin { get; set; }
        [BindProperty(SupportsGet = true)]
        public decimal? MinSalary { get; set; }

        [BindProperty(SupportsGet = true)]
        public decimal? MaxSalary { get; set; }

        public async Task<IActionResult> OnGetAsync(int pageNumber = 1)
        {
            CurrentPage = pageNumber;
            JobPostings = _jobPostingRepository.GetAllJobPostings(pageNumber, PageSize, JobTypeFilter, ExperienceFilter, postedWithin, MinSalary, MaxSalary);
            return Page();
        }

        public void OnPost()
        {
        }
    }
}
