namespace JobFinder.Models
{
    public class Recruiter
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string? CompanyDescription { get; set; }
        public string? CompanyWebsite { get; set; }
        public string CompanyPhone { get; set; }
        public string CompanyEmail { get; set; }
        public DateTime RegisterDate { get; set; } = DateTime.UtcNow;
        public string Address { get; set; }
        public int StaffSize { get; set; }
        public AppUser? User { get; set; }
        public string UserId { get; set; }
        public ICollection<JobPosting> JobPostings { get; set; }
        public string? CompanyImage {  get; set; }
    }
}
