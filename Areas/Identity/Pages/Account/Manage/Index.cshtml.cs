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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace JobFinder.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IJobSeekerRepository _jobSeekerRepository;

        public IndexModel(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IJobSeekerRepository jobSeekerRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jobSeekerRepository = jobSeekerRepository;

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
            [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "First name cannot contain numbers.")]
            public string FirstName { get; set; }

            [Required(ErrorMessage = "Last name is required")]
            [Display(Name = "Last name")]
            [StringLength(30, ErrorMessage = "Last name cannot be longer than 30 characters.")]
            [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Last name cannot contain numbers.")]
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

            var jobSeeker = await _jobSeekerRepository.GetJobSeekerByUserIdAsync(user.Id);


            if (jobSeeker != null)
            {
                /* var addressParts = jobSeeker.Address?.Split(",");
                 string address = addressParts.Length > 0 ? addressParts[0] : string.Empty;
                 string district = addressParts.Length > 1 ? addressParts[1] : string.Empty;
                 string province = addressParts.Length > 2 ? addressParts[2] : string.Empty;*/

                Input.FirstName = jobSeeker.FirstName;
                Input.LastName = jobSeeker.LastName;
                Input.Birthday = jobSeeker.Birthday;
                Input.Address = jobSeeker.Address;
                Input.EducationLevel = jobSeeker.EducationLevel;
                Input.Specialized = jobSeeker.Specialized;
                Input.Experience = jobSeeker.Experience;
                /* Input.Province = province;   
                 Input.District = district;*/
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
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
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

            var jobSeeker = await _jobSeekerRepository.GetJobSeekerByUserIdAsync(user.Id);
            if (jobSeeker == null)
            {
                return NotFound($"Unable to load job seeker profile for user with ID '{_userManager.GetUserId(User)}'.");
            }

            jobSeeker.FirstName = Input.FirstName;
            jobSeeker.LastName = Input.LastName;
            jobSeeker.Birthday = Input.Birthday;
            jobSeeker.Address = Input.Address;
            jobSeeker.EducationLevel = Input.EducationLevel;
            jobSeeker.Specialized = Input.Specialized;
            jobSeeker.Experience = Input.Experience;
            jobSeeker.UserId = user.Id;
            /* jobSeeker.CV = Input.CV;
               jobSeeker.JobPositionId = Input.JobPositionId;*/

            // Save changes to JobSeeker
            /*await _context.SaveChangesAsync();*/
            _jobSeekerRepository.UpdateJobSeeker(jobSeeker);

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
