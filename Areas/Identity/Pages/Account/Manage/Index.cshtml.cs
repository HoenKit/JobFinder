// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using JobFinder.Data;
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
        private ApplicationDbContext _context;  

        public IndexModel(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
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
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            [Required(ErrorMessage = "First name is required")]
            [Display(Name = "First name")]
            [StringLength(100, ErrorMessage = "First name cannot be longer than 100 characters.")]
            public string FirstName { get; set; }

            [Required(ErrorMessage = "Last name is required")]
            [Display(Name = "Last name")]
            [StringLength(100, ErrorMessage = "Last name cannot be longer than 100 characters.")]
            public string LastName { get; set; }

            [Required(ErrorMessage = "Birthday is required")]
            [DataType(DataType.Date, ErrorMessage = "Please enter a valid date.")]
            [Display(Name = "Date of Birth")]
            public DateTime Birthday { get; set; }

            [Required(ErrorMessage = "Address is required")]
            [Display(Name = "Address")]
            public string Address { get; set; }

            [Required(ErrorMessage = "Education Level is required")]
            [Display(Name = "Education Level")]
            public string EducationLevel { get; set; }

            [Required(ErrorMessage = "Specialization is required")]
            [Display(Name = "Specialized")]
            public string Specialized { get; set; }


            [Required(ErrorMessage = "Experience is required")]
            [Display(Name = "Experience")]
            public string Experience { get; set; }

         /*   [Display(Name = "CV URL")]
            public string CV { get; set; }

            public int JobPositionId { get; set; }

            // These fields could map to User and JobPosition relationships
            public string UserId { get; set; }
            public string JobPosition { get; set; }*/
        }

        private async Task LoadAsync(AppUser user)
        {
            // Get the username and phone number of the user
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            // Retrieve JobSeeker profile based on the user ID
            var jobSeeker = await _context.JobSeeker.FirstOrDefaultAsync(js => js.UserId == user.Id);

            if (jobSeeker == null)
            {
                // Handle case where JobSeeker profile does not exist
                return;
            }

            // Assign the values to the Input model from JobSeeker and user data
            Username = userName;

            Input = new InputModel
            {
                FirstName = jobSeeker.FirstName,
                LastName = jobSeeker.LastName,
                Birthday = jobSeeker.Birthday,
                Address = jobSeeker.Address,
                EducationLevel = jobSeeker.EducationLevel,
                Specialized = jobSeeker.Specialized,
                Experience = jobSeeker.Experience,
              /*  CV = jobSeeker.CV,
                JobPositionId = jobSeeker.JobPositionId,*/
                PhoneNumber = phoneNumber // Set the phone number from the user profile
            };
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

            var jobSeeker = await _context.JobSeeker.FirstOrDefaultAsync(js => js.UserId == user.Id);
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
/*          jobSeeker.CV = Input.CV;
            jobSeeker.JobPositionId = Input.JobPositionId;*/

            // Save changes to JobSeeker
            _context.JobSeeker.Update(jobSeeker);
            await _context.SaveChangesAsync();

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
