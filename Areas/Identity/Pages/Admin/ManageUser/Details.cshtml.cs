using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using JobFinder.Models;
using JobFinder.Interface;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace JobFinder.Areas.Identity.Pages.Admin.ManageUser
{
    [Authorize(Roles = "Administrator")]
    public class DetailsModel : PageModel
    {
        private readonly IAppUserRepository _appUserRepository;
        private readonly IJobSeekerRepository _jobSeekerRepository;
        private readonly IRecruiterRepository _recruiterRepository;

        public DetailsModel(IAppUserRepository appUserRepository,
                            IJobSeekerRepository jobSeekerRepository,
                            IRecruiterRepository recruiterRepository)
        {
            _appUserRepository = appUserRepository;
            _jobSeekerRepository = jobSeekerRepository;
            _recruiterRepository = recruiterRepository;
        }

        public AppUser AppUser { get; set; }
        public JobSeeker JobSeeker { get; set; }
        public Recruiter Recruiter { get; set; }
        public bool HasJobSeeker { get; set; } = false;
        public bool HasRecruiter { get; set; } = false;
        public IList<string> Roles { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {

            AppUser = await _appUserRepository.GetUserByIdAsync(id);
            if (AppUser == null)
            {
                return RedirectToPage("./Index");
            }

            JobSeeker = await _jobSeekerRepository.GetJobSeekerByUserIdAsync(id);
            Recruiter = await _recruiterRepository.GetRecruiterByUserIdAsync(id);

            HasJobSeeker = JobSeeker != null;
            HasRecruiter = Recruiter != null;

            Roles = await _appUserRepository.GetUserRolesAsync(AppUser);

            return Page();
        }
    }
}
