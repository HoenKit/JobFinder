using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using JobFinder.Models;
using JobFinder.Interface;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;


namespace JobFinder.Areas.Identity.Pages.Admin.ManageJobPosting
{
    [Authorize(Roles = "Administrator")]
    public class AcceptRejectModel : PageModel
    {
        private readonly IJobPostingRepository _jobPostingRepository;

        public AcceptRejectModel(IJobPostingRepository jobPostingRepository)
        {
            _jobPostingRepository = jobPostingRepository;
        }

        [BindProperty]
        public JobPosting JobPosting { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            JobPosting = await _jobPostingRepository.GetJobPostingByIdAsync(id);

            if (JobPosting == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var JobPosting = await _jobPostingRepository.GetJobPostingByIdAsync(id);

            if (JobPosting == null)
            {
                return NotFound();
            }

            await _jobPostingRepository.ToggleBanStatusAsync(JobPosting);

            return RedirectToPage("./Index");
        }
    }
}
