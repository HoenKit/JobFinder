using JobFinder.Interface;
using JobFinder.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace JobFinder.Areas.Identity.Pages.RecruiterInfo
{
    [Authorize(Roles = "Recruiter")]
    public class ListJobSeekerModel : PageModel
    {
        private readonly IJobPostingRepository _jobPostingRepository;
        private readonly IApplicationRepository _applicationRepository;
        private readonly IJobSeekerRepository _jobSeekerRepository;
        private readonly IRecruiterRepository _recruiterRepository;

        public ListJobSeekerModel(
            IJobPostingRepository jobPostingRepository,
            IApplicationRepository applicationRepository,
            IJobSeekerRepository jobSeekerRepository,
            IRecruiterRepository recruiterRepository)
        {
            _jobPostingRepository = jobPostingRepository;
            _applicationRepository = applicationRepository;
            _jobSeekerRepository = jobSeekerRepository;
            _recruiterRepository = recruiterRepository;
        }

        public List<Application> Applications { get; set; }

        public async Task<IActionResult> OnGetAsync(int jobPostingId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var recruiter = await _recruiterRepository.GetRecruiterByUserIdAsync(userId);
            var jobPosting = await _jobPostingRepository.GetJobPostingByIdAsync(jobPostingId);
            if (jobPosting.RecruiterId != recruiter.Id)
            {
               return RedirectToPage("/Error");
            }
            Applications = await _applicationRepository.GetApplicationsByJobPosting(jobPostingId);
            return Page();    
        }
    }
}
