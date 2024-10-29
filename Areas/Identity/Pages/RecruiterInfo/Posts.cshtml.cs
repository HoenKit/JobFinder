using JobFinder.Interface;
using JobFinder.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Drawing.Printing;
using System.Security.Claims;

namespace JobFinder.Areas.Identity.Pages.RecruiterInfo
{
    public class PostsModel : PageModel
    {
        private readonly IRecruiterRepository _recruiterRepository;
        private readonly IJobPostingRepository _postingRepository;

        public PostsModel(IRecruiterRepository recruiterRepository, IJobPostingRepository postingRepository)
        {
            _recruiterRepository = recruiterRepository;
            _postingRepository = postingRepository;
        }

        public Recruiter Recruiter { get; set; }
        public List<JobPosting> JobPostings { get; set; } = new List<JobPosting>();
        public async Task<IActionResult> OnGetAsync(int pageNumber = 1, int pageSize = 10)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Recruiter = await _recruiterRepository.GetRecruiterByUserIdAsync(userId);
            if (Recruiter == null)
            {
                return NotFound();
            }

            var paginatedResult = await _postingRepository.GetJobPostingsByRecruiterAsync(Recruiter.Id, pageNumber, pageSize);
            JobPostings = paginatedResult.Data;

            return Page();
        }
    }
}
