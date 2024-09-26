using JobFinder.Interface;
using JobFinder.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace JobFinder.Pages
{
    public class RegisterRecruiterModel : PageModel
    {
        private readonly IRecruiterRepository _recruiterRepository;

        public RegisterRecruiterModel(IRecruiterRepository recruiterRepository)
        {
            _recruiterRepository = recruiterRepository;
        }

        [BindProperty]
        [Required(ErrorMessage = "Company Name is required.")]
        [StringLength(100, ErrorMessage = "Company Name cannot exceed 100 characters.")]
        public string CompanyName { get; set; }

        [BindProperty]
        [StringLength(500, ErrorMessage = "Company Description cannot exceed 500 characters.")]
        public string? CompanyDescription { get; set; }

        [BindProperty]
        [Url(ErrorMessage = "Please enter a valid URL.")]
        public string? CompanyWebsite { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Company Phone is required.")]
        [Phone(ErrorMessage = "Please enter a valid phone number.")]
        public string CompanyPhone { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Company Email is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string CompanyEmail { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Address is required.")]
        public string fullAddress { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Staff Size is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Staff Size must be greater than 0.")]
        public int StaffSize { get; set; }

        [BindProperty]
        public IFormFile? CompanyImage { get; set; }

        public string UserId { get; set; }

        public async Task<IActionResult> OnPostAsync(string userId)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var recruiter = new Recruiter
            {
                CompanyName = CompanyName,
                CompanyDescription = CompanyDescription,
                CompanyWebsite = CompanyWebsite,
                CompanyPhone = CompanyPhone,
                CompanyEmail = CompanyEmail,
                Address = fullAddress,
                StaffSize = StaffSize,
                UserId = userId
            };

            if (CompanyImage != null && CompanyImage.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(CompanyImage.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                var relativePath = Path.Combine("uploads", uniqueFileName).Replace("\\", "/");

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await CompanyImage.CopyToAsync(fileStream);
                }

                recruiter.CompanyImage = relativePath;
            }

            _recruiterRepository.AddRecruiter(recruiter);
            return RedirectToPage("/Index");
        }
    }
}
