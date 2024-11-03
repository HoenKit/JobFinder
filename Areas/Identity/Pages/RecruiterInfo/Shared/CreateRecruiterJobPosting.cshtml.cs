using System;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using JobFinder.Data;
using JobFinder.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using JobFinder.Models;
using JobFinder.Interface;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Shared;
using Microsoft.AspNetCore.Identity;
using JobFinder.Repository;
using System.Security.Claims;

namespace JobFinder.Areas.Identity.Pages.RecruiterInfo.Shared
{
    [Authorize(Roles = "Recruiter")]
    public class CreateRecruiterJobPostingModel : PageModel
    {
        private readonly IJobNatureRepository _jobNatureRepository;
        private readonly IJobPostingRepository _jobPostingRepository;
        private readonly IJobTypeRepository _jobTypeRepository;
        private readonly IJobPositionRepository _jobPositionRepository;
        private readonly IRecruiterRepository _recruiterRepository;

        public CreateRecruiterJobPostingModel(
           IJobNatureRepository jobNatureRepository,
           IJobPostingRepository jobPostingRepository,
           IJobTypeRepository jobTypeRepository,
           IJobPositionRepository jobPositionRepository,
           IRecruiterRepository recruiterRepository)
        {
            _jobNatureRepository = jobNatureRepository;
            _jobPostingRepository = jobPostingRepository;
            _jobTypeRepository = jobTypeRepository;
            _jobPositionRepository = jobPositionRepository;
            _recruiterRepository = recruiterRepository;

        }

        [BindProperty]
        public JobPostingInputModel Input { get; set; }

        public string? CompanyImage { get; set; }
        public int RecruiterId { get; set; }
        public Recruiter Recruiter { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public ICollection<JobPosition> JobPositions { get; set; } = new List<JobPosition>();

        [BindProperty]
        public int JobPositionId { get; set; }

        public ICollection<JobNature> JobNature { get; set; } = new List<JobNature>();

        [BindProperty]
        public int JobNatureId { get; set; }

        public ICollection<JobType> JobType { get; set; } = new List<JobType>();

        [BindProperty]
        public int JobTypeId { get; set; }


        [BindProperty]
        public int JobPostingStatus { get; set; } = 0;

        [BindProperty]
        [Required(ErrorMessage = "Location is required.")]
        [StringLength(100, ErrorMessage = "Location can't be longer than 100 characters.")]
        public string fullJobLocation { get; set; }


        public class JobPostingInputModel
        {
            [BindProperty]
            [Required(ErrorMessage = "Title is required.")]
            [StringLength(100, ErrorMessage = "Job Title can't be longer than 100 characters.")]
            public string JobTitle { get; set; }

            [BindProperty]
            [Required(ErrorMessage = "Description is required.")]
            [StringLength(1000, ErrorMessage = "Job Description can't be longer than 1000 characters.")]
            public string JobDescription { get; set; }

            [BindProperty]
            [Required(ErrorMessage = "Level is required.")]
            public string RequiredLevel { get; set; }

            [BindProperty]
            [Required(ErrorMessage = "Experience is required.")]
            [Range(0, int.MaxValue, ErrorMessage = "Please enter a valid number for experience.")]
            public string RequiredExperience { get; set; }

            [BindProperty]
            [Required(ErrorMessage = "Salary is required.")]
            [Range(0, double.MaxValue, ErrorMessage = "Salary must be a positive number.")]
            public decimal Salary { get; set; }

            [BindProperty]
            [DataType(DataType.Date)]
            [Required(ErrorMessage = "Expiration Date is required.")]
            public DateTime ExpirationDate { get; set; }

            [BindProperty]
            [Required(ErrorMessage = "Vacancy is required.")]
            [Range(1, int.MaxValue, ErrorMessage = "Vacancy must be at least 1.")]
            public int Vacancy { get; set; }

        }

        /*     public IActionResult OnGet(string userId)
             {
                 if (string.IsNullOrEmpty(userId))
                 {
                     ModelState.AddModelError(string.Empty, "User ID is required.");
                     return Page();
                 }

                 var recruiter = _recruiterRepository.GetRecruiterByUserIdAsync(userId).GetAwaiter().GetResult();

                 if (recruiter == null)
                 {
                     ModelState.AddModelError(string.Empty, "Recruiter not found.");
                     return Page();
                 }
                 CompanyImage = recruiter.CompanyImage;
                 RecruiterId = recruiter.Id;

                 LoadComponent();

                 ModelState.Clear();

                 return Page();
             }*/



        public async Task<IActionResult> OnPostAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                ModelState.AddModelError(string.Empty, "User ID is required.");
                LoadComponent();
                return Page();
            }

            var recruiter = await _recruiterRepository.GetRecruiterByUserIdAsync(userId);
            if (recruiter == null)
            {
                ModelState.AddModelError(string.Empty, "Recruiter not found.");
                LoadComponent();
                return RedirectToPage("/RecruiterInfo/Index");
            }

            RecruiterId = recruiter.Id;

            if (!ModelState.IsValid)
            {
                LoadComponent();
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Console.WriteLine(error.ErrorMessage);
                }
                return Page();
            }

            var jobPosting = new JobPosting
            {
                JobTitle = Input.JobTitle,
                JobDescription = Input.JobDescription,
                RequiredLevel = Input.RequiredLevel,
                RequiredExperience = Input.RequiredExperience,
                Salary = Input.Salary,
                ExpirationDate = Input.ExpirationDate,
                Vacancy = Input.Vacancy,
                JobLocation = fullJobLocation,
                JobPositionId = JobPositionId,
                JobNatureId = JobNatureId,
                JobTypeId = JobTypeId,
                RecruiterId = RecruiterId, 
            };

            _jobPostingRepository.AddJobPosting(jobPosting);
            StatusMessage = "New job posting has been created.";
            return RedirectToPage("/RecruiterInfo/Index");
        }

        private void LoadComponent()
        {
            JobPositions = _jobPositionRepository.GetJobPositions() ?? new List<JobPosition>();
            JobType = _jobTypeRepository.GetAllJobType() ?? new List<JobType>();
            JobNature = _jobNatureRepository.GetJobNature() ?? new List<JobNature>();

        }

    }
}
