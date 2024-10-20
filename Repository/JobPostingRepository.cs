using JobFinder.Data;
using JobFinder.Dtos;
using JobFinder.Interface;
using JobFinder.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace JobFinder.Repository
{
    public class JobPostingRepository : IJobPostingRepository
    {
        private readonly ApplicationDbContext _context;
        public JobPostingRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<JobPosting> LatestJobPostings { get; set; } = new List<JobPosting>();
        public PaginatedResult<JobPosting> GetAllJobPostings(int pageNumber, int pageSize, string[] jobTypeFilter, string[] experienceFilter, int? postedWithin, decimal? minSalary, decimal? maxSalary, int jobTypeId, string address)
        {
            var query = _context.JobPosting
                .Include(p => p.JobNature)
                .Include(o => o.JobPosition)
                .Include(c => c.Recruiter)
                .Include(t => t.JobType)
                .Where(p => p.JobPostingStatus == 0);

            if (jobTypeFilter != null && jobTypeFilter.Length > 0)
            {
                query = query.Where(p => jobTypeFilter.Contains(p.JobNature.JobNatureName));
            }

            if (experienceFilter != null && experienceFilter.Length > 0)
            {
                var queryResult = query.AsEnumerable(); // Switch to client-side processing

                foreach (var experience in experienceFilter)
                {
                    if (experience == "6-more")
                    {
                        queryResult = queryResult.Where(p =>
                        {
                            // Handle cases like "6-more"
                            if (int.TryParse(p.RequiredExperience.Split('-').First(), out var expYears))
                            {
                                return expYears >= 6;
                            }
                            return false;
                        });
                    }
                    else if (experience.Contains("-"))
                    {
                        var range = experience.Split('-');
                        if (int.TryParse(range[0], out var min) && int.TryParse(range[1], out var max))
                        {
                            queryResult = queryResult.Where(p =>
                            {
                                // Parse the minimum value from the range in RequiredExperience
                                if (int.TryParse(p.RequiredExperience.Split('-').First(), out var expYears))
                                {
                                    return expYears >= min && expYears <= max;
                                }
                                return false;
                            });
                        }
                    }
                    else
                    {
                        if (int.TryParse(experience, out var singleValue))
                        {
                            queryResult = queryResult.Where(p =>
                            {
                                if (int.TryParse(p.RequiredExperience.Split('-').First(), out var expYears))
                                {
                                    return expYears == singleValue;
                                }
                                return false;
                            });
                        }
                    }
                }

                query = queryResult.AsQueryable();
            }

            if (postedWithin.HasValue && postedWithin.Value > 0)
            {
                DateTime minDate = DateTime.Now.AddDays(-postedWithin.Value);
                query = query.Where(p => p.PostDate >= minDate);
            }

            if (minSalary.HasValue)
            {
                query = query.Where(p => p.Salary >= minSalary.Value);
            }

            if (maxSalary.HasValue)
            {
                query = query.Where(p => p.Salary <= maxSalary.Value);
            }
            if (jobTypeId > 0)
            {
                query = query.Where(p => p.JobTypeId == jobTypeId);
            }
            if (!string.IsNullOrEmpty(address))
            {
                query = query.Where(p => p.JobLocation.Contains(address));
            }

            var totalRecords = query.Count();

            var paginatedData = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            foreach (var job in paginatedData)
            {
                var locationParts = job.JobLocation.Split(',');
                job.JobLocation = string.Join(", ", locationParts.Skip(Math.Max(0, locationParts.Length - 2)));
                job.Salary = Math.Round(job.Salary, job.Salary % 1 == 0 ? 0 : 2);
            }

            return new PaginatedResult<JobPosting>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                Data = paginatedData
            };
        }

        public PaginatedResult<JobPosting> GetAllJobPostings(int pageNumber, int pageSize, string jobTitle, string location)
        {
            var query = _context.JobPosting
                .Include(p => p.JobNature)
                .Include(o => o.JobPosition)
                .Include(c => c.Recruiter)
                .Include(t => t.JobType)
                .Where(p => p.JobPostingStatus == 0);

            if (!string.IsNullOrEmpty(jobTitle))
            {
                query = query.Where(p => p.JobTitle.Contains(jobTitle));
            }

            if (!string.IsNullOrEmpty(location))
            {
                query = query.Where(p => p.JobLocation.Contains(location));
            }

            var totalRecords = query.Count();

            var paginatedData = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PaginatedResult<JobPosting>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                Data = paginatedData
            };
        }

        public IEnumerable<string> GetDistinctJobTitles()
        {
            return _context.JobPosting
                .Where(p => p.JobPostingStatus == 0)
                .Select(p => p.JobTitle)
                .Distinct()
                .ToList();
        }

        public PaginatedResult<JobPosting> GetAllJobPostings(int pageNumber, int pageSize, int? selectedStatus)
        {
            var query = _context.JobPosting
                                .Include(p => p.JobNature)
                                .Include(o => o.JobPosition)
                                .Include(c => c.Recruiter)
                                .Include(t => t.JobType)
                                .AsQueryable();

            if (selectedStatus.HasValue)
            {
                query = query.Where(p => p.JobPostingStatus == selectedStatus.Value);
            }

            query = query.OrderByDescending(p => p.PostDate);

            var totalRecords = query.Count();

            var paginatedData = query.Skip((pageNumber - 1) * pageSize)
                                     .Take(pageSize)
                                     .ToList();

            return new PaginatedResult<JobPosting>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                Data = paginatedData
            };
        }

        public async Task<bool> UpdateJobPostingAsync(JobPosting jobPosting)
        {
            _context.JobPosting.Update(jobPosting);

            var result = await _context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<JobPosting> GetJobPostingByIdAsync(int id)
        {
            return _context.JobPosting
                .Include(j => j.Recruiter)
                .Include(j => j.JobNature)
                .FirstOrDefault(j => j.Id == id);
        }

        public async Task<bool> ToggleBanStatusAsync(JobPosting job)
        {
            job.JobPostingStatus = job.JobPostingStatus == 1 ? 0 : 1;
            var result = await UpdateJobPostingAsync(job);
            return result;
        }

        public  List<JobPosting> GetLatestJobPostings(int count = 5)
=>
             _context.JobPosting
              .Include(j => j.Recruiter)
              .Include(j => j.JobNature)
              .OrderByDescending(j => j.PostDate)
              .Take(count)
              .ToList();
        
        public PaginatedResult<JobPosting> GetJobPostingsByJobType(int jobTypeId, int pageNumber, int pageSize)
        {
            var query = _context.JobPosting
                .Include(j => j.JobType)
                .Include(j => j.Recruiter)
                .Include(j => j.JobNature)
                .Where(j => j.JobTypeId == jobTypeId)
                .AsQueryable();

            var totalRecords = query.Count();

            var jobPostings = query.Skip((pageNumber - 1) * pageSize)
                                   .Take(pageSize)
                                   .ToList();

            return new PaginatedResult<JobPosting>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                Data = jobPostings
            };

        }
    }
}
