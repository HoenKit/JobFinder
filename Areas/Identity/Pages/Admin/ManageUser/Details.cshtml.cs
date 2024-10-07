using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using JobFinder.Models;
using JobFinder.Interface;
using System.Threading.Tasks;

namespace JobFinder.Areas.Identity.Pages.Admin.ManageUser
{
    public class DetailsModel : PageModel
    {
        private readonly IAppUserRepository _appUserRepository;

        public DetailsModel(IAppUserRepository appUserRepository)
        {
            _appUserRepository = appUserRepository;
        }

        public AppUser AppUser { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            AppUser = await _appUserRepository.GetUserByIdAsync(id);
            if (AppUser == null)
            {
                return RedirectToPage("./Index");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostToggleStatusAsync(string id)
        {
            AppUser = await _appUserRepository.GetUserByIdAsync(id);
            if (AppUser == null)
            {
                return RedirectToPage("./Index");
            }

            var success = await _appUserRepository.ToggleBanStatusAsync(AppUser);
            if (success)
            {
                return RedirectToPage("./Index");
            }

            return Page();
        }
    }
}
