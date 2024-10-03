using JobFinder.Interface;
using JobFinder.Models;
using JobFinder.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace JobFinder.Pages
{
    public class RegisterRecruiterModel : PageModel
    {
        private readonly IRecruiterRepository _recruiterRepository;
        private readonly BlobStorageService _blobStorageService;

        public RegisterRecruiterModel(IRecruiterRepository recruiterRepository, BlobStorageService blobStorageService)
        {
            _recruiterRepository = recruiterRepository;
            _blobStorageService = blobStorageService;
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
                // Generate a unique filename using GUID
                var fileExtension = Path.GetExtension(CompanyImage.FileName);
                var uniqueFileName = Guid.NewGuid().ToString() + fileExtension;

                // Upload to Azure Blob Storage
                using (var stream = CompanyImage.OpenReadStream())
                {
                    await _blobStorageService.UploadFileAsync(uniqueFileName, stream);
                }

                // Store the full URL in the database (update the blob URL with your storage account name)
                recruiter.CompanyImage = $"https://jobfinderuploads.blob.core.windows.net/uploads/{uniqueFileName}";
            }

            _recruiterRepository.AddRecruiter(recruiter);
            return RedirectToPage("/Index");
        }
    }
}
