namespace JobFinder.Models
{
    public class Application
    {
        public int Id { get; set; }
        public int ApplicationStatus { get; set; } = 2;
        public DateTime ApplicationDate { get; set; } = DateTime.Now;
        public DateTime ReviewDate { get; set; }
        public DateTime ResultDate { get; set; }
        public JobSeeker? JobSeeker { get; set; }
        public int JobSeekerId { get; set; }
        public JobPosting? JobPosting { get; set; }
        public int JobPostingId { get; set; }
    }
}
