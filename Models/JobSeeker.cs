namespace JobFinder.Models
{
    public class JobSeeker
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthday { get; set; }
        public string Address { get; set; }
        public string EducationLevel { get; set; }
        public string Specialized {  get; set; }
        public string Experience { get; set; }
        public string CV { get; set; }
        public DateTime RegisterDate { get; set; } = DateTime.Now;
        public JobPosition? JobPosition { get; set; }
        public int JobPositionId { get; set; }
        public AppUser? User { get; set; }
        public string UserId { get; set; }
        public ICollection<Application>? Applications { get; set; }
        
    }
}
