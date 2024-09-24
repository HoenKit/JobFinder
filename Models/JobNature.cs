namespace JobFinder.Models
{
    public class JobNature
    {
        public int Id { get; set; }
        public string JobNatureName { get; set; }
        public ICollection<JobPosting> JobPostings { get; set; }
    }
}
