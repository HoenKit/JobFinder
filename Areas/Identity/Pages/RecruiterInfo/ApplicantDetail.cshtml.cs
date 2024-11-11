using JobFinder.Interface;
using JobFinder.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JobFinder.Areas.Identity.Pages.RecruiterInfo
{
    [Authorize(Roles = "Recruiter")]
    public class ApplicantDetailModel : PageModel
    {
        private readonly IApplicationRepository _applicationRepository;
        private readonly IEmailSender _emailSender;

        public ApplicantDetailModel(IApplicationRepository applicationRepository, IEmailSender emailSender)
        {
            _applicationRepository = applicationRepository;
            _emailSender = emailSender;
        }

        [BindProperty]
        public Application Application { get; set; }

        public async Task<IActionResult> OnGetAsync(int applicationId)
        {
            Application = await _applicationRepository.GetApplicationWithDetails(applicationId);
            if (Application == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int applicationId, string action)
        {
            Application = await _applicationRepository.GetApplicationWithDetails(applicationId);
            if (Application == null)
            {
                return NotFound();
            }

            if (action == "accept")
            {
                Application.ApplicationStatus = 0;
            }
            else if (action == "reject")
            {
                Application.ApplicationStatus = 1;
            }

            await _applicationRepository.UpdateApplication(Application);

            var userEmail = Application.JobSeeker.User.Email;
            var subject = action == "accept" ? "Your job posting application has been accepted" : "Your job posting Application Has Been rejected";
            var message = action == "accept" ? "Congratulations! Your application has been accepted. Now wait for the company to contact you directly." : "We regret to inform you that your application has been rejected.";

            await _emailSender.SendEmailAsync(userEmail, subject, message);

            return RedirectToPage("/RecruiterInfo/ListJobSeeker", new { jobPostingId = Application.JobPostingId });
        }

    }
}
