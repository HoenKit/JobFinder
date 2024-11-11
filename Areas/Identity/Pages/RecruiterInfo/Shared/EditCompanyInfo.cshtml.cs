using JobFinder.Interface;
using JobFinder.Models;
using JobFinder.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace JobFinder.Areas.Identity.Pages.RecruiterInfo.Shared
{
    public class EditCompanyInfoModel : PageModel
    {
        private readonly IRecruiterRepository _recruiterRepository;
        private readonly BlobStorageService _blobStorageService;

        public EditCompanyInfoModel(IRecruiterRepository recruiterRepository, BlobStorageService blobStorageService)
        {
            _recruiterRepository = recruiterRepository;
            _blobStorageService = blobStorageService;
        }

        [BindProperty]
        public Recruiter Recruiter { get; set; } = new();

        [BindProperty]
        public IFormFile UploadAvatar { get; set; }
        public string? CompanyImage { get; set; }

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

            existingRecruiter.CompanyName = Recruiter.CompanyName;
            existingRecruiter.CompanyPhone = Recruiter.CompanyPhone;
            existingRecruiter.CompanyEmail = Recruiter.CompanyEmail;
            existingRecruiter.CompanyWebsite = Recruiter.CompanyWebsite;
            existingRecruiter.StaffSize = Recruiter.StaffSize;
            existingRecruiter.Address = Recruiter.Address;
            existingRecruiter.CompanyDescription = CompanyDescription;


            // Handle CV upload
            if (UploadAvatar != null && UploadAvatar.Length > 0)
            {
                var allowedExtensions = new[] { ".png", ".jpg", ".jpeg", ".gif" };
                var fileExtension = Path.GetExtension(UploadAvatar.FileName);

                if (!allowedExtensions.Contains(fileExtension))
                {
                    ModelState.AddModelError("avatar", "Only files of type PNG, JPG, JPEG, or GIF are accepted.");
                    return Page();
                }

                // Delete the old CV if it exists
                if (!string.IsNullOrEmpty(Recruiter.CompanyImage))
                {
                    var oldAvatarName = Path.GetFileName(new Uri(Recruiter.CompanyImage).LocalPath);
                    await _blobStorageService.DeleteFileIfExistsAsync(oldAvatarName);
                }

                var uniqueFileName = Guid.NewGuid().ToString() + fileExtension;

                using (var stream = UploadAvatar.OpenReadStream())
                {
                    await _blobStorageService.UploadFileAsync(uniqueFileName, stream);
                }

                existingRecruiter.CompanyImage = $"https://jobfinderuploads.blob.core.windows.net/uploads/{uniqueFileName}";
            }

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
