namespace JobFinder.Interface
{
    public interface IDashboardRepository
    {
        Task<int> GetTotalUsersAsync();

        Task<double> GetUserGrowthPercentageAsync();

        Task<int> GetTotalJobPostingsAsync();

        Task<double> GetJobPostingGrowthPercentageByMonthAsync();

        Task<List<int>> GetMonthlyJobPostingsAsync(int year);

        Task<double> GetJobPostingGrowthPercentageByYearAsync();
    }
}
