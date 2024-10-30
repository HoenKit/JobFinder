using JobFinder.Interface;
using JobFinder.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace JobFinder.Areas.Identity.Pages.RecruiterInfo
{
    public class PostDetailModel : PageModel
    {
        private readonly IJobPostingRepository _jobPostingRepository;
        private readonly IApplicationRepository _applicationRepository;
        private readonly IJobSeekerRepository _jobSeekerRepository;
        public PostDetailModel(IJobPostingRepository jobPostingRepository, IApplicationRepository applicationRepository, IJobSeekerRepository jobSeekerRepository)
        {
            _jobPostingRepository = jobPostingRepository;
            _applicationRepository = applicationRepository;
            _jobSeekerRepository = jobSeekerRepository;
        }

        public JobPosting JobPosting { get; set; }
        public List<JobSeeker> AppliedJobSeekers { get; set; } = new List<JobSeeker>();
        public async Task OnGetAsync(int id)
        {
            JobPosting = await _jobPostingRepository.GetJobPostingByIdAsync(id);

            AppliedJobSeekers = await _applicationRepository.GetJobSeekersByJobPostingIdAsync(id);
        }

    }
}
