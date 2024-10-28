using JobFinder.Interface;
using JobFinder.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace JobFinder.Areas.Identity.Pages.RecruiterInfo
{
    public class AboutModel : PageModel
    {
        private readonly IRecruiterRepository _recruiterRepository;

        public AboutModel(IRecruiterRepository recruiterRepository)
        {
            _recruiterRepository = recruiterRepository;
        }

        public Recruiter Recruiter { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Recruiter = await _recruiterRepository.GetRecruiterByUserIdAsync(userId);
            if (Recruiter == null)
            {
                return NotFound();
            }


            return Page();
        }
    }
}
