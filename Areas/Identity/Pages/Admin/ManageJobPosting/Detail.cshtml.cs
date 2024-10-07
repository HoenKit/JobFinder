using JobFinder.Interface;
using JobFinder.Models;
using JobFinder.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JobFinder.Areas.Identity.Pages.Admin.ManageJobPosting
{
        public class DetailModel : PageModel
        {
            private readonly IJobDetailRepository _jobDetailRepository;
            private readonly IJobPostingRepository _JobPostingRepository;

            public DetailModel(IJobPostingRepository jobPostingRepository, IJobDetailRepository jobDetailRepository)
            {
                _jobDetailRepository = jobDetailRepository;
                _JobPostingRepository = jobPostingRepository;
            }

            public JobPosting JobPosting { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            JobPosting = await _JobPostingRepository.GetJobPostingByIdAsync(id);
            if (JobPosting == null)
            {
                return RedirectToPage("./Index");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostToggleStatusAsync(int id)
        {
            JobPosting = await _JobPostingRepository.GetJobPostingByIdAsync(id);
            if (JobPosting == null)
            {
                return RedirectToPage("./Index");
            }

            var success = await _JobPostingRepository.ToggleBanStatusAsync(JobPosting);
            if (success)
            {
                return RedirectToPage("./Index");
            }

            return Page();
        }
    }
}
