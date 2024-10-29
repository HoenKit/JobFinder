using JobFinder.Interface;
using JobFinder.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace JobFinder.Pages
{
    public class FindJobDetailModel : PageModel
    {
        private readonly IJobPostingRepository _jobPostingRepository;
        private readonly IApplicationRepository _applicationRepository;
        private readonly IJobSeekerRepository _jobSeekerRepository;
        public FindJobDetailModel(IJobPostingRepository jobPostingRepository, IApplicationRepository applicationRepository, IJobSeekerRepository jobSeekerRepository)
        {
            _jobPostingRepository = jobPostingRepository;
            _applicationRepository = applicationRepository;
            _jobSeekerRepository = jobSeekerRepository;
        }
        public bool IsApplied { get; set; } = false;  
        public bool IsAuthenticated => User.Identity.IsAuthenticated;
        public JobPosting JobPosting { get; set; }

        public async Task OnGetAsync(int id)
        {
            JobPosting = await _jobPostingRepository.GetJobPostingByIdAsync(id);

            if (IsAuthenticated)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var jobSeeker = await _jobSeekerRepository.GetJobSeekerByUserIdAsync(userId);

                if (jobSeeker != null)
                {
                    var existingApplication = await _applicationRepository.GetApplicationExistAsync(jobSeeker.Id, id);
                    IsApplied = existingApplication != null; 
                }
            }
        }


        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!IsAuthenticated)
            {
                return RedirectToPage("./Account/Login", new { area = "Identity" });
            }

            JobPosting = await _jobPostingRepository.GetJobPostingByIdAsync(id);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var jobSeeker = await _jobSeekerRepository.GetJobSeekerByUserIdAsync(userId);
            if (jobSeeker == null)
            {
                ModelState.AddModelError(string.Empty, "Job seeker not found.");
                return Page();
            }

            var existingApplication = await _applicationRepository.GetApplicationExistAsync(jobSeeker.Id, id);

            if (existingApplication != null)
            {
                IsApplied = true; 
                return Page();
            }

            var application = new Application
            {
                JobSeekerId = jobSeeker.Id,
                JobPostingId = id,
                ApplicationDate = DateTime.Now
            };

            await _applicationRepository.AddApplication(application);
            IsApplied = true; 

            return Page();
        }
    }
}
