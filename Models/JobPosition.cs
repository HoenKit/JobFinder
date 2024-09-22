namespace JobFinder.Models
{
    public class JobPosition
    {
        public int Id { get; set; }
        public string JobPositionName { get; set; }
        public ICollection<JobSeeker> JobSeekers { get; set; }
        public ICollection<JobPosting> JobPostings { get; set; }

    }
}
