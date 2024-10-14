using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using JobFinder.Models;
using JobFinder.Interface;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace JobFinder.Areas.Identity.Pages.Admin.ManageUser
{
    [Authorize(Roles = "Administrator")]
    public class BanUnbanModel : PageModel
    {
        private readonly IAppUserRepository _appUserRepository;

        public BanUnbanModel(IAppUserRepository appUserRepository)
        {
            _appUserRepository = appUserRepository;
        }

        [BindProperty]
        public AppUser AppUser { get; set; }

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

            var user = await _appUserRepository.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            await _appUserRepository.ToggleBanStatusAsync(user);

            return RedirectToPage("./Index");
        }
    }
}
