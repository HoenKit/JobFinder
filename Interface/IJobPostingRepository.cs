﻿using JobFinder.Dtos;
using JobFinder.Models;

namespace JobFinder.Interface
{
    public interface IJobPostingRepository
    {
        PaginatedResult<JobPosting> GetAllJobPostings(int pageNumber, int pageSize, string[] jobTypeFilter, string[] experienceFilter, int? postedWithin, decimal? minSalary, decimal? maxSalary);
    }
}
