using JobFinder.Dtos;
using JobFinder.Models;
using System.Threading.Tasks;

namespace JobFinder.Interface
{
    public interface IAppUserRepository
    {
        Task<PaginatedResult<AppUser>> GetPaginatedUsersAsync(int pageNumber, int pageSize, int? profileStatusFilter);
        Task<AppUser> GetUserByIdAsync(string id);
        Task<bool> ToggleBanStatusAsync(AppUser user);
    }

}
