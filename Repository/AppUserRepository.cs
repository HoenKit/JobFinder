using JobFinder.Models;
using JobFinder.Interface;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using JobFinder.Dtos;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace JobFinder.Repository
{
    public class AppUserRepository : IAppUserRepository
    {
        private readonly UserManager<AppUser> _userManager;

        public AppUserRepository(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<PaginatedResult<AppUser>> GetPaginatedUsersAsync(int pageNumber, int pageSize, int? profileStatusFilter = null)
        {
            var query = _userManager.Users.AsQueryable();

            if (profileStatusFilter.HasValue)
            {
                query = query.Where(u => u.ProfileStatus == profileStatusFilter.Value);
            }

            var totalRecords = await query.CountAsync();

            var users = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedResult<AppUser>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                Data = users
            };
        }


        public async Task<AppUser?> GetUserByIdAsync(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<bool> ToggleBanStatusAsync(AppUser user)
        {
            user.ProfileStatus = user.ProfileStatus == 1 ? 0 : 1;
            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }

        public async Task<IList<string>> GetUserRolesAsync(AppUser user)
        {
            return await _userManager.GetRolesAsync(user);
        }
    }
}
