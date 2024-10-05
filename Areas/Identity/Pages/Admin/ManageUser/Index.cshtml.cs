using JobFinder.Dtos;
using JobFinder.Interface;
using JobFinder.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;

namespace JobFinder.Areas.Identity.Pages.Admin.ManageUser
{
    public class IndexModel : PageModel
    {
        private readonly IAppUserRepository _appUserRepository;

        public IndexModel(IAppUserRepository appUserRepository)
        {
            _appUserRepository = appUserRepository;
        }

        public PaginatedResult<AppUser> PaginatedUsers { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; } = 10;

        public int? CurrentProfileStatusFilter { get; set; }

        public async Task OnGetAsync(int pageNumber = 1, int? profileStatusFilter = null)
        {
            CurrentPage = pageNumber;
            CurrentProfileStatusFilter = profileStatusFilter;

            PaginatedUsers = await _appUserRepository.GetPaginatedUsersAsync(pageNumber, PageSize, profileStatusFilter);
        }

        public async Task<IActionResult> OnPostToggleStatusAsync(string id)
        {
            var user = await _appUserRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var success = await _appUserRepository.ToggleBanStatusAsync(user);
            if (success)
            {
                TempData["StatusMessage"] = "User status updated successfully.";
                return RedirectToPage(new { pageNumber = CurrentPage, profileStatusFilter = CurrentProfileStatusFilter });
            }

            return Page();
        }
    }

}
