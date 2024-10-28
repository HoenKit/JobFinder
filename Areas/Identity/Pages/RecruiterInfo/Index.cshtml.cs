using JobFinder.Interface;
using JobFinder.Models;
using JobFinder.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace JobFinder.Areas.Identity.Pages.RecruiterInfo
{
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
    }
}
