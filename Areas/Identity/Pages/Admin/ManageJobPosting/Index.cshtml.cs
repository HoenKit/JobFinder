using JobFinder.Dtos;
using JobFinder.Interface;
using JobFinder.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JobFinder.Areas.Identity.Pages.Admin.ManageJobPosting
{
    public class IndexModel : PageModel
    {
        private readonly IJobPostingRepository _jobPostingRepository;

        public IndexModel(IJobPostingRepository jobPostingRepository)
        {
            _jobPostingRepository = jobPostingRepository;
        }

        public PaginatedResult<JobPosting> JobPostings { get; set; }
        [BindProperty(SupportsGet = true)]
        public int? SelectedStatus { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; } = 10;

        public async Task<IActionResult> OnGetAsync(int pageNumber = 1)
        {
            CurrentPage = pageNumber;

            JobPostings = _jobPostingRepository.GetAllJobPostings(pageNumber, PageSize, SelectedStatus);
            return Page();
        }
    }
}
