using JobFinder.Interface;
using JobFinder.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace JobFinder.Areas.Identity.Pages.RecruiterInfo
{
    [Authorize(Roles = "Recruiter")]
    public class IndexModel : PageModel
    {
        private readonly IRecruiterRepository _recruiterRepository;
        private readonly IJobPostingRepository _postingRepository;

        public IndexModel(IRecruiterRepository recruiterRepository, IJobPostingRepository postingRepository)
        {
            _recruiterRepository = recruiterRepository;
            _postingRepository = postingRepository;
        }

        public Recruiter Recruiter { get; set; }
        public List<JobPosting> JobPostings { get; set; } = new List<JobPosting>();

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Recruiter = await _recruiterRepository.GetRecruiterByUserIdAsync(userId);
            if (Recruiter == null)
            {
                return NotFound();
            }
            JobPostings = await _postingRepository.GetJobPostingsByRecruiterAsync(Recruiter.Id);

            return Page();
        }

        public async Task<IActionResult> OnPostToggleStatusAsync(int jobPostingId)
        {
            var jobPosting = await _postingRepository.GetJobPostingByIdAsync(jobPostingId);
            if (jobPosting == null)
            {
                return NotFound();
            }
            jobPosting.JobPostingStatus = jobPosting.JobPostingStatus == 0 ? 1 : 0;
            await _postingRepository.UpdateJobPostingAsync(jobPosting);

            return RedirectToPage();
        }
    }
}
