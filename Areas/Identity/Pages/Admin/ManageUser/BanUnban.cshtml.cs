using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using JobFinder.Models;
using JobFinder.Interface;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace JobFinder.Areas.Identity.Pages.Admin.ManageUser
{
    [Authorize(Roles = "Administrator")]
    public class BanUnbanModel : PageModel
    {
        private readonly IAppUserRepository _appUserRepository;
        private readonly IEmailSender _emailSender;

        public BanUnbanModel(IAppUserRepository appUserRepository, IEmailSender emailSender)
        {
            _appUserRepository = appUserRepository;
            _emailSender = emailSender;
        }

        [BindProperty]
        public AppUser AppUser { get; set; }

        [BindProperty]
        public string Reason { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            AppUser = await _appUserRepository.GetUserByIdAsync(id);

            if (AppUser == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (string.IsNullOrWhiteSpace(Reason))
            {
                ModelState.AddModelError("Reason", "Please provide a reason for this action.");
                AppUser = await _appUserRepository.GetUserByIdAsync(id);
                return Page();
            }

            var user = await _appUserRepository.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }
            await _appUserRepository.ToggleBanStatusAsync(user);

            var banStatus = user.ProfileStatus == 1 ? "banned" : "unbanned";
            await _emailSender.SendEmailAsync(
                user.Email,
                "Account Ban/Unban Notification",
                $@"<div style='font-family: Arial, sans-serif;'>
            <h2 style='color: #2c3e50;'>Account {banStatus}</h2>
            <p style='font-size: 16px; color: #34495e;'>
                Your account has been {banStatus} by the admin. Because {Reason}.
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
