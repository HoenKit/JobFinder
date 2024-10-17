using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using JobFinder.Models;
using JobFinder.Interface;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace JobFinder.Areas.Identity.Pages.Admin.ManageJobPosting
{
    [Authorize(Roles = "Administrator")]
    public class AcceptRejectModel : PageModel
    {
        private readonly IJobPostingRepository _jobPostingRepository;
        private readonly IEmailSender _emailSender;
        private readonly IAppUserRepository _appUserRepository;
        private readonly IRecruiterRepository _recruiterRepository;

        public AcceptRejectModel(IJobPostingRepository jobPostingRepository, IEmailSender emailSender, IAppUserRepository appUserRepository, IRecruiterRepository recruiterRepository)
        {
            _jobPostingRepository = jobPostingRepository;
            _emailSender = emailSender;
            _appUserRepository = appUserRepository;
            _recruiterRepository = recruiterRepository;
        }

        [BindProperty]
        public JobPosting JobPosting { get; set; }
        public AppUser AppUser { get; set; }
        public Recruiter Recruiter { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id == 0)
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
            if (id == 0)
            {
                return NotFound();
            }

            var jobPosting = await _jobPostingRepository.GetJobPostingByIdAsync(id);

            if (jobPosting == null)
            {
                return NotFound();
            }

            Recruiter = await _recruiterRepository.GetRecruiterByUserIdAsync(jobPosting.Recruiter.UserId);

            if (Recruiter == null)
            {
                return NotFound();
            }

            AppUser = await _appUserRepository.GetUserByIdAsync(Recruiter.UserId);

            if (AppUser == null)
            {
                return NotFound();
            }

            await _jobPostingRepository.ToggleBanStatusAsync(jobPosting);

            var status = jobPosting.JobPostingStatus == 1 ? "rejected" : "accepted";

            await _emailSender.SendEmailAsync(
                AppUser.Email,
                $"Job Posting {status}",
                $@"<div style='font-family: Arial, sans-serif;'>
                    <h2 style='color: #2c3e50;'>Your job posting has been {status}</h2>
                    <p style='font-size: 16px; color: #34495e;'>
                        Your job posting titled '{jobPosting.JobTitle}' has been {status} by the admin.
                    </p>
                    <p style='font-size: 16px; color: #34495e;'>If you have any questions, please contact support.</p>
                    <p style='font-size: 14px; color: #7f8c8d; margin-top: 20px;'>
                        This is an automated message, please do not reply.
                    </p>
                </div>"
            );

            return RedirectToPage("./Index");
        }
    }
}
