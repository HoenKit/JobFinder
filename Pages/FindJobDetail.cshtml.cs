using JobFinder.Interface;
using JobFinder.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JobFinder.Pages
{
    public class FindJobDetailModel : PageModel
    {
        private readonly IJobDetailRepository _jobDetailRepository;

        public FindJobDetailModel(IJobDetailRepository jobDetailRepository)
        {
            _jobDetailRepository = jobDetailRepository;
        }
        public bool IsApplied { get; set; } = false;  
        public bool IsAuthenticated => User.Identity.IsAuthenticated;
        public JobPosting JobPosting { get; set; }

        public void OnGet(int id)
        {
            JobPosting = _jobDetailRepository.GetJobPostingById(id);
        }

        public IActionResult OnPost(int id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("./Account/Login", new { area = "Identity" });
            }

            JobPosting = _jobDetailRepository.GetJobPostingById(id);

            IsApplied = true;


            return Page(); 
        }
    }
}
