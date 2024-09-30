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
                return RedirectToPage("./Index"); // Chuyển hướng về danh sách nếu không tìm thấy người dùng
            }

            return Page();
        }

        public async Task<IActionResult> OnPostToggleStatusAsync(string id)
        {
            AppUser = await _appUserRepository.GetUserByIdAsync(id);
            if (AppUser == null)
            {
                return RedirectToPage("./Index"); // Chuyển hướng về danh sách nếu không tìm thấy người dùng
            }

            var success = await _appUserRepository.ToggleBanStatusAsync(AppUser);
            if (success)
            {
                return RedirectToPage("./Index");
            }

            return Page(); // Vẫn trả về trang chi tiết nếu không thành công
        }
    }
}
