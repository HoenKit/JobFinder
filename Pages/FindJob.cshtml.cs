using JobFinder.Dtos;
using JobFinder.Interface;
using JobFinder.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace JobFinder.Pages
{
    public class FindJobModel : PageModel
    {
        private readonly IJobPostingRepository _jobPostingRepository;
        private readonly IJobTypeRepository _jobTypeRepository;
        public FindJobModel(IJobPostingRepository jobPostingRepository, IJobTypeRepository jobTypeRepository)
        {
            _jobPostingRepository = jobPostingRepository;
            _jobTypeRepository = jobTypeRepository;
        }

        public PaginatedResult<JobPosting> JobPostings { get; set; }
        public ICollection<JobType> JobTypes { get; set; }
        [BindProperty(SupportsGet = true)]
        public string fullAddress { get; set; }
        [BindProperty(SupportsGet = true)]
        public int JobTypeId { get; set; }
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
            JobTypes = _jobTypeRepository.GetAllJobType();
            CurrentPage = pageNumber;
            JobPostings = _jobPostingRepository.GetAllJobPostings(pageNumber, PageSize, JobTypeFilter, ExperienceFilter, postedWithin, MinSalary, MaxSalary, JobTypeId, fullAddress);
            return Page();
        }

        public void OnPost()
        {
        }
    }
}
