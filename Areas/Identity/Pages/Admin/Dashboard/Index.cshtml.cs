using JobFinder.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JobFinder.Areas.Identity.Pages.Admin.Dashboard
{
    public class IndexModel : PageModel
    {
        private readonly IDashboardRepository _dashboardRepository;

        public IndexModel(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }

        public int TotalUsers { get; set; }
        public double GrowthPercentage { get; set; }
        public int totalJobPosting { get; set; }
        public double jobPostingGrowthPercentageByMonth { get; set; }
        public List<int> MonthlyJobPostings { get; set; }

        public double GetJobPostingGrowthPercentageByYear {  get; set; }

        public async Task OnGetAsync()
        {
            TotalUsers = await _dashboardRepository.GetTotalUsersAsync();
            GrowthPercentage = await _dashboardRepository.GetUserGrowthPercentageAsync();
            totalJobPosting = await _dashboardRepository.GetTotalJobPostingsAsync();
            jobPostingGrowthPercentageByMonth = await _dashboardRepository.GetJobPostingGrowthPercentageByMonthAsync();

            int currentMonth = DateTime.Now.Month;
            int currentYear = DateTime.Now.Year;
            var monthlyJobPostings = await _dashboardRepository.GetMonthlyJobPostingsAsync(currentYear);
            MonthlyJobPostings = monthlyJobPostings.Take(currentMonth).ToList();

            GetJobPostingGrowthPercentageByYear = await _dashboardRepository.GetJobPostingGrowthPercentageByYearAsync();
        }
    }

}
