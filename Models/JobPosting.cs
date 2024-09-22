namespace JobFinder.Models
{
    public class JobPosting
    {
        public int Id { get; set; }
        public string JobTitle { get; set; }
        public string JobDescription { get; set; }
        public string RequiredLevel { get; set; }
        public string RequiredExperience { get; set; }
        public string JobLocation { get; set; }
        public decimal Salary { get; set; }
        public DateTime PostDate { get; set; } = DateTime.UtcNow;
        public DateTime ExpirationDate { get; set; }
        public int JobPostingStatus { get; set; } = 0;
        public int Vacancy { get; set; }
        public JobNature? JobNature { get; set; }
        public int JobNatureId { get; set; }
        public JobPosition? JobPosition { get; set; }
        public int JobPositionId { get; set; }
        public JobType? JobType { get; set; }
        public int JobTypeId { get; set; }
        public Recruiter? Recruiter { get; set; }
        public int RecruiterId { get; set; }
        public ICollection<Application> Applications { get; set; }


    }
}
