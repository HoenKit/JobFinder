namespace JobFinder.Models
{
    public class JobType
    {
        public int Id { get; set; }
        public string JobTypeName { get; set; }
        public ICollection<JobPosting> JobPostings { get; set; }
    }
}
