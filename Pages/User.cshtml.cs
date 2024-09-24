using JobFinder.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace JobFinder.Pages
{
    [Authorize(Roles = "Users")]
    public class UserModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;

        public AppUser appUser;
        public UserModel(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task OnGet()
        {
            var task = _userManager.GetUserAsync(User);
            task.Wait();
            appUser = task.Result;
        }
    }
}
