using JobFinder.Data;
using JobFinder.Interface;
using JobFinder.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace JobFinder.Repository
{
    public class DashboardRepository: IDashboardRepository
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ApplicationDbContext _context;

        public DashboardRepository(UserManager<AppUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        //Dashboard User
        public async Task<int> GetTotalUsersAsync()
        {
            var totalUsers = await _userManager.Users
                .Where(u => u.ProfileStatus == 0)
                .CountAsync();

            return totalUsers;
        }

        public async Task<int> GetNewUsersForMonthAsync(DateTime date)
        {
            var month = date.Month;
            var year = date.Year;


            var validUserIds = _userManager.Users
                .Where(u => u.ProfileStatus == 0)
                .Select(u => u.Id)
                .ToList();

           var newRecruiters = await _context.Recruiter
                .Where(r => validUserIds.Contains(r.UserId) && r.RegisterDate.Month == month && r.RegisterDate.Year == year)
                .CountAsync();

            var newJobSeekers = await _context.JobSeeker
                .Where(js => validUserIds.Contains(js.UserId) && js.RegisterDate.Month == month && js.RegisterDate.Year == year)
                .CountAsync();

            return newRecruiters + newJobSeekers;
        }

        public async Task<double> GetUserGrowthPercentageAsync()
        {
            var currentMonthUsers = await GetNewUsersForMonthAsync(DateTime.Now);
            var lastMonth = DateTime.Now.AddMonths(-1);
            var lastMonthUsers = await GetNewUsersForMonthAsync(lastMonth);

            if (lastMonthUsers > 0)
            {
                return ((double)(currentMonthUsers - lastMonthUsers) / lastMonthUsers) * 100;
            }
            else
            {
                return currentMonthUsers > 0 ? 100 : 0;
            }
        }

        //Dashboard Post 
        public async Task<int> GetTotalJobPostingsAsync()
        {
            var totalJobPostings = await _context.JobPosting
                .Where(jp => jp.JobPostingStatus == 0)
                .CountAsync();

            return totalJobPostings;
        }

        public async Task<int> GetNewJobPostingsForMonthAsync(DateTime date)
        {
            var month = date.Month;
            var year = date.Year;

            var newJobPostings = await _context.JobPosting
                .Where(jp => jp.PostDate.Month == month && jp.PostDate.Year == year && jp.JobPostingStatus == 0)
                .CountAsync();

            return newJobPostings;
        }

        public async Task<double> GetJobPostingGrowthPercentageByMonthAsync()
        {
            var currentMonthJobPostings = await GetNewJobPostingsForMonthAsync(DateTime.Now);
            var lastMonth = DateTime.Now.AddMonths(-1);
            var lastMonthJobPostings = await GetNewJobPostingsForMonthAsync(lastMonth);

            if (lastMonthJobPostings > 0)
            {
                return ((double)(currentMonthJobPostings - lastMonthJobPostings) / lastMonthJobPostings) * 100;
            }

            return currentMonthJobPostings > 0 ? 100 : 0;
        }

        public async Task<List<int>> GetMonthlyJobPostingsAsync(int year)
        {
            var jobPostingsPerMonth = new List<int>();

            for (int month = 1; month <= 12; month++)
            {
                var count = await _context.JobPosting
                    .Where(j => j.PostDate.Year == year && j.PostDate.Month == month)
                    .CountAsync();

                jobPostingsPerMonth.Add(count);
            }

            return jobPostingsPerMonth;
        }
        public async Task<int> GetNewJobPostingsForYearAsync(int year)
        {
            var newJobPostings = await _context.JobPosting
                .Where(jp => jp.PostDate.Year == year && jp.JobPostingStatus == 0)
                .CountAsync();

            return newJobPostings;
        }

        public async Task<double> GetJobPostingGrowthPercentageByYearAsync()
        {
            var currentYearJobPostings = await GetNewJobPostingsForYearAsync(DateTime.Now.Year);
            var lastYear = DateTime.Now.AddYears(-1).Year;
            var lastYearJobPostings = await GetNewJobPostingsForYearAsync(lastYear);

            if (lastYearJobPostings > 0)
            {
                return ((double)(currentYearJobPostings - lastYearJobPostings) / lastYearJobPostings) * 100;
            }

            return currentYearJobPostings > 0 ? 100 : 0;
        }
    }
}
