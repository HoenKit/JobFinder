using JobFinder.Interface;
using JobFinder.Models;
using JobFinder.Repository;
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

        public RegisterJobSeekerModel(IJobSeekerRepository seekerRepository, IJobPositionRepository positionRepository)
        {
            _positionRepository = positionRepository;
            _seekerRepository = seekerRepository;
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
        public string Address { get; set; }

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
                Address = Address,
                EducationLevel = EducationLevel,
                Specialized = Specialized,
                Experience = Experience,
                JobPositionId = JobPositionId,
                UserId = UserId
            };

            if (CV != null && CV.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

                // Create the uploads directory if it doesn't exist
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Generate a unique filename using GUID
                var fileExtension = Path.GetExtension(CV.FileName); 
                var uniqueFileName = Guid.NewGuid().ToString() + fileExtension; 

                var cvFilePath = Path.Combine(uploadsFolder, uniqueFileName);
                var relativePath = Path.Combine("uploads", uniqueFileName).Replace("\\", "/");

                // Save the file to the server
                using (var stream = new FileStream(cvFilePath, FileMode.Create))
                {
                    await CV.CopyToAsync(stream);
                }

                // Store the relative path in the database
                jobSeeker.CV = relativePath; 
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
