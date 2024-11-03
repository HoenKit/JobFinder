using JobFinder.Interface;
using JobFinder.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Claims;

namespace JobFinder.Areas.Identity.Pages.Account.Manage
{
    [Authorize(Roles = "Users")]
    public class ApplicationJobModel : PageModel
    {
        private readonly IApplicationRepository _applicationRepository;
        private readonly IJobSeekerRepository _jobSeekerRepository;

        public ApplicationJobModel(IApplicationRepository applicationRepository, IJobSeekerRepository jobSeekerRepository)
        {
            _applicationRepository = applicationRepository;
            _jobSeekerRepository = jobSeekerRepository;
        }

        public string? CompanyImage { get; set; }
        public int JobSeekerId { get; set; }
        public List<Application> Applications { get; set; } = new List<Application>();

        private async Task LoadAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                ModelState.AddModelError(string.Empty, "Unable to load user ID.");
                return;
            }

            var jobSeeker = await _jobSeekerRepository.GetJobSeekerByUserIdAsync(userId);
            if (jobSeeker == null)
            {
                ModelState.AddModelError(string.Empty, "Job Seeker not found.");
                return;
            }

            JobSeekerId = jobSeeker.Id;
            Applications = await _applicationRepository.GetApplicationsWithDetailsByJobSeekerIdAsync(JobSeekerId);
        }

        public async Task<IActionResult> OnGetAsync()
        {
            await LoadAsync();

            if (!ModelState.IsValid)
            {
                return Page();
            }

            return Page();
        }
    }
}
