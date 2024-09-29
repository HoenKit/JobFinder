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
        public int PageSize { get; set; } = 7;
        public async Task<IActionResult> OnGetAsync(int pageNumber = 1)
        {
            CurrentPage = pageNumber;
            JobPostings = _jobPostingRepository.GetAllJobPostings(pageNumber, PageSize);
            return Page();
        }
        public void OnPost()
        {
        }
    }
}
