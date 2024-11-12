// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using JobFinder.Data;
using JobFinder.Interface;
using JobFinder.Models;
using JobFinder.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace JobFinder.Areas.Identity.Pages.Account.Manage
{
    [Authorize(Roles = "Users")]
    public class IndexModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IJobSeekerRepository _jobSeekerRepository;
        private readonly BlobStorageService _blobStorageService;
        private readonly IJobPositionRepository _positionRepository;

        public IndexModel(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IJobSeekerRepository jobSeekerRepository,
            BlobStorageService blobStorageService,
            IJobPositionRepository positionRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jobSeekerRepository = jobSeekerRepository;
            _blobStorageService = blobStorageService;
            _positionRepository = positionRepository;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        /// 
        public string Username { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        public JobSeeker JobSeeker { get; set; }

        [BindProperty]
        /*[Required(ErrorMessage = "CV is required")]*/
        public IFormFile UploadCV { get; set; }

        public string CV { get; set; }

        public ICollection<JobPosition> JobPositions { get; set; } = new List<JobPosition>();

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            [Required(ErrorMessage = "Phone number is required.")]
            [Phone(ErrorMessage = "Please enter a valid phone number.")]
            [Display(Name = "Phone number")]
            [RegularExpression(@"^(\+84\d{9,10}|0\d{9,10})$", ErrorMessage = "Please enter a valid Phone number and contain 10 to 11 digits.")]
            public string PhoneNumber { get; set; }

            [Required(ErrorMessage = "First name is required")]
            [Display(Name = "First name")]
            [StringLength(30, ErrorMessage = "First name cannot be longer than 30 characters.")]
            [RegularExpression(@"^[a-zA-ZÀ-ÿ\u1EA0-\u1EFF\s]+$", ErrorMessage = "First name cannot contain numbers or special characters.")]
            public string FirstName { get; set; }

            [Required(ErrorMessage = "Last name is required")]
            [Display(Name = "Last name")]
            [StringLength(30, ErrorMessage = "Last name cannot be longer than 30 characters.")]
            [RegularExpression(@"^[a-zA-ZÀ-ÿ\u1EA0-\u1EFF\s]+$", ErrorMessage = "Last name cannot contain numbers or special characters.")]
            public string LastName { get; set; }


            [Required(ErrorMessage = "Birthday is required")]
            [DataType(DataType.Date, ErrorMessage = "Please enter a valid date.")]
            [Display(Name = "Date of Birth")]
            public DateTime Birthday { get; set; }


            [Display(Name = "Province/City")]
            public string Province { get; set; }


            [Display(Name = "District")]
            public string District { get; set; }

            [Required(ErrorMessage = "Address is required")]
            [Display(Name = "Full Address")]
            public string Address { get; set; }

            [Required(ErrorMessage = "Education Level is required")]
            [Display(Name = "Education Level")]
            public string EducationLevel { get; set; }

            [Required(ErrorMessage = "Specialization is required")]
            [Display(Name = "Specialized")]
            [StringLength(30, ErrorMessage = "Cannot be longer than 30 characters.")]
            [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Cannot contain numbers.")]
            public string Specialized { get; set; }


            [Required(ErrorMessage = "Experience is required")]
            [Display(Name = "Experience")]
            [RegularExpression(@"^[0-9]+$", ErrorMessage = "Experience must be a positive number.")]
            public string Experience { get; set; }

            public int JobPositionId { get; set; }


            /* [Display(Name = "CV URL")]
             public string CV { get; set; }

             public int JobPositionId { get; set; }

             // These fields could map to User and JobPosition relationships

             public string JobPosition { get; set; }*/
            public string UserId { get; set; }
        }

        private async Task LoadAsync(AppUser user)
        {

            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;

            Input = new InputModel
            {
                UserId = user.Id,
                PhoneNumber = phoneNumber
            };

            JobPositions = _positionRepository.GetJobPositions() ?? new List<JobPosition>();

            var jobSeeker = await _jobSeekerRepository.GetJobSeekerByUserIdAsync(user.Id);


            if (jobSeeker != null)
            {
                Input.FirstName = jobSeeker.FirstName;
                Input.LastName = jobSeeker.LastName;
                Input.Birthday = jobSeeker.Birthday;
                Input.Address = jobSeeker.Address;
                Input.EducationLevel = jobSeeker.EducationLevel;
                Input.Specialized = jobSeeker.Specialized;
                Input.Experience = jobSeeker.Experience;
                Input.JobPositionId = jobSeeker.JobPositionId;
                JobSeeker = jobSeeker;
                Console.WriteLine("Loaded CV URL: " + JobSeeker?.CV);
                ModelState.AddModelError(string.Empty, "JobSeeker profile not found. Please complete your profile.");
                return;
            }
        }


        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            Console.WriteLine("CV URL: " + JobSeeker?.CV);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(IFormFile UploadCV)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                foreach (var modelError in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(modelError.ErrorMessage);
                }

                await LoadAsync(user);
                return Page();
            }
            // Check if user is over 18
            if ((DateTime.Today.Year - Input.Birthday.Year) < 18 || (DateTime.Today.Year - Input.Birthday.Year == 18 && Input.Birthday.Date > DateTime.Today.AddYears(-18)))
            {
                ModelState.AddModelError("Input.Birthday", "You must be at least 18 years old.");
                await LoadAsync(user);
                return Page();
            }

            // Handle phone number update
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            // Get or create job seeker profile
            var jobSeeker = await _jobSeekerRepository.GetJobSeekerByUserIdAsync(user.Id);
            if (jobSeeker == null)
            {
                jobSeeker = new JobSeeker
                {
                    UserId = user.Id,
                    FirstName = Input.FirstName,
                    LastName = Input.LastName,
                    Birthday = Input.Birthday,
                    Address = Input.Address,
                    EducationLevel = Input.EducationLevel,
                    Specialized = Input.Specialized,
                    Experience = Input.Experience,
                    JobPositionId = Input.JobPositionId,
                };

                if (UploadCV != null && UploadCV.Length > 0)
                {
                    // Generate a unique filename using GUID
                    var fileExtension = Path.GetExtension(UploadCV.FileName);
                    var uniqueFileName = Guid.NewGuid().ToString() + fileExtension;

                    // Upload to Azure Blob Storage
                    using (var stream = UploadCV.OpenReadStream())
                    {
                        await _blobStorageService.UploadFileAsync(uniqueFileName, stream);
                    }

                    // Store the full URL in the database (update the blob URL with your storage account name)
                    jobSeeker.CV = $"https://jobfinderuploads.blob.core.windows.net/uploads/{uniqueFileName}";
                }

                else
                {
                    ModelState.AddModelError("CV", "Please upload your CV.");
                    return RedirectToPage();
                }

                try
                {
                    _jobSeekerRepository.AddJobSeeker(jobSeeker);
                    await _signInManager.RefreshSignInAsync(user);
                    StatusMessage = "Your JobSeeker profile has been updated.";
                    return RedirectToPage();
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error saving data: {ex.Message}");
                    return RedirectToPage();
                }

            }
            else
            {
                // Update existing jobSeeker properties
                jobSeeker.FirstName = Input.FirstName;
                jobSeeker.LastName = Input.LastName;
                jobSeeker.Birthday = Input.Birthday;
                jobSeeker.Address = Input.Address;
                jobSeeker.EducationLevel = Input.EducationLevel;
                jobSeeker.Specialized = Input.Specialized;
                jobSeeker.Experience = Input.Experience;
                jobSeeker.JobPositionId = Input.JobPositionId;
                JobPositions = _positionRepository.GetJobPositions() ?? new List<JobPosition>();

            }

            // Handle CV upload
            if (UploadCV != null && UploadCV.Length > 0)
            {
                var allowedExtensions = new[] { ".pdf", ".png", ".jpg", ".jpeg", ".gif" };
                var fileExtension = Path.GetExtension(UploadCV.FileName);

                if (!allowedExtensions.Contains(fileExtension))
                {
                    ModelState.AddModelError("CV", "Only files of type PDF, PNG, JPG, JPEG, or GIF are accepted.");
                    await LoadAsync(user);
                    return Page();
                }

                // Delete the old CV if it exists
                if (!string.IsNullOrEmpty(jobSeeker.CV))
                {
                    var oldCVName = Path.GetFileName(new Uri(jobSeeker.CV).LocalPath);
                    await _blobStorageService.DeleteFileIfExistsAsync(oldCVName);
                }

                var uniqueFileName = Guid.NewGuid().ToString() + fileExtension;

                using (var stream = UploadCV.OpenReadStream())
                {
                    await _blobStorageService.UploadFileAsync(uniqueFileName, stream);
                }

                // Update CV URL
                jobSeeker.CV = $"https://jobfinderuploads.blob.core.windows.net/uploads/{uniqueFileName}";
            }

            // Save all changes to the JobSeeker
            await _jobSeekerRepository.UpdateJobSeeker(jobSeeker);
            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated.";
            return RedirectToPage();
        }
    }
}
