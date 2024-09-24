using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JobFinder.Pages
{
    [Authorize(Roles = "Administrator")]
    public class AdminModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
