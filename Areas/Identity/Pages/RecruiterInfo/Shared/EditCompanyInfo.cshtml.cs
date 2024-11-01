using JobFinder.Interface;
using JobFinder.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace JobFinder.Areas.Identity.Pages.RecruiterInfo.Shared
{
    public class EditCompanyInfoModel : PageModel
    {
        private readonly IRecruiterRepository _recruiterRepository;

        public EditCompanyInfoModel(IRecruiterRepository recruiterRepository)
        {
            _recruiterRepository = recruiterRepository;
        }

        [BindProperty]
        public Recruiter Recruiter { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Recruiter = await _recruiterRepository.GetRecruiterByUserIdAsync(userId);
            if (Recruiter == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string CompanyDescription)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var existingRecruiter = await _recruiterRepository.GetRecruiterByUserIdAsync(userId);

            if (existingRecruiter == null)
            {
                TempData["ErrorMessage"] = "Recruiter not found.";
                return NotFound();
            }

            // Cập nhật thông tin công ty
            existingRecruiter.CompanyName = Recruiter.CompanyName;
            existingRecruiter.CompanyPhone = Recruiter.CompanyPhone;
            existingRecruiter.CompanyEmail = Recruiter.CompanyEmail;
            existingRecruiter.CompanyWebsite = Recruiter.CompanyWebsite;
            existingRecruiter.StaffSize = Recruiter.StaffSize;
            existingRecruiter.Address = Recruiter.Address;

            // Sử dụng giá trị từ input ẩn
            existingRecruiter.CompanyDescription = CompanyDescription;

            try
            {
                await _recruiterRepository.UpdateCompanyAsync(existingRecruiter);
                TempData["SuccessMessage"] = "Company information updated successfully!";
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Error updating company information. Please try again.";
            }

            return RedirectToAction("/Index");
        }

    }
}
