using JobFinder.Interface;
using JobFinder.Models;
using JobFinder.Repository;
using JobFinder.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace JobFinder.Pages
{
    [AllowAnonymous]
    public class RegisterJobSeekerModel : PageModel
    {
        private readonly IJobPositionRepository _positionRepository;
        private readonly IJobSeekerRepository _seekerRepository;
        private readonly BlobStorageService _blobStorageService;

        public RegisterJobSeekerModel(IJobSeekerRepository seekerRepository, IJobPositionRepository positionRepository, BlobStorageService blobStorageService)
        {
            _positionRepository = positionRepository;
            _seekerRepository = seekerRepository;
            _blobStorageService = blobStorageService;
        }

        [BindProperty]
        [Required(ErrorMessage = "First Name is required.")]
        [StringLength(50, ErrorMessage = "First Name can't be longer than 50 characters.")]
        public string FirstName { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Last Name is required.")]
        [StringLength(50, ErrorMessage = "Last Name can't be longer than 50 characters.")]
        public string LastName { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Birthday is required.")]
        [DataType(DataType.Date)]
        public DateTime Birthday { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Address is required.")]
        [StringLength(100, ErrorMessage = "Address can't be longer than 100 characters.")]
        public string fullAddress { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Education Level is required.")]
        public string EducationLevel { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Specialized is required.")]
        [StringLength(100, ErrorMessage = "Specialized can't be longer than 100 characters.")]
        public string Specialized { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Experience is required.")]
        [Range(0, 50, ErrorMessage = "Experience must be between 1 and 50 years.")]
        public string Experience { get; set; }

        [BindProperty]
        public IFormFile CV { get; set; }

        [BindProperty]
        public int JobPositionId { get; set; }

        public string UserId { get; set; }
        public ICollection<JobPosition> JobPositions { get; set; } = new List<JobPosition>();

        public void OnGet()
        {
            LoadJobPositions(); 
        }

        public async Task<IActionResult> OnPostAsync(string userId)
        {
            UserId = userId;
            if (!ModelState.IsValid)
            {
                LoadJobPositions();
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Console.WriteLine(error.ErrorMessage);
                }
                return Page();
            }

            if (string.IsNullOrEmpty(UserId))
            {
                ModelState.AddModelError(string.Empty, "User ID is required.");
                return Page();
            }

            var jobSeeker = new JobSeeker
            {
                FirstName = FirstName,
                LastName = LastName,
                Birthday = Birthday,
                Address = fullAddress,
                EducationLevel = EducationLevel,
                Specialized = Specialized,
                Experience = Experience,
                JobPositionId = JobPositionId,
                UserId = UserId
            };

            if (CV != null && CV.Length > 0)
            {
                // Generate a unique filename using GUID
                var fileExtension = Path.GetExtension(CV.FileName);
                var uniqueFileName = Guid.NewGuid().ToString() + fileExtension;

                // Upload to Azure Blob Storage
                using (var stream = CV.OpenReadStream())
                {
                    await _blobStorageService.UploadFileAsync(uniqueFileName, stream);
                }

                // Store the full URL in the database (update the blob URL with your storage account name)
                jobSeeker.CV = $"https://jobfinderuploads.blob.core.windows.net/uploads/{uniqueFileName}";
            }
            else
            {
                ModelState.AddModelError("CV", "Please upload your CV.");
                return Page();
            }

            try
            {
                _seekerRepository.AddJobSeeker(jobSeeker);
                return RedirectToPage("/Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error saving data: {ex.Message}");
                LoadJobPositions();
                return Page();
            }
        }

        private void LoadJobPositions()
        {
            JobPositions = _positionRepository.GetJobPositions() ?? new List<JobPosition>();
        }
    }

}
