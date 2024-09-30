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
            // Khởi tạo truy vấn với tất cả người dùng
            var query = _userManager.Users.AsQueryable();

            // Nếu có bộ lọc, thêm điều kiện vào truy vấn
            if (profileStatusFilter.HasValue)
            {
                query = query.Where(u => u.ProfileStatus == profileStatusFilter.Value);
            }

            // Lấy tổng số lượng người dùng theo bộ lọc
            var totalRecords = await query.CountAsync();

            // Thực hiện phân trang với Skip và Take
            var users = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Trả về kết quả phân trang
            return new PaginatedResult<AppUser>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                Data = users
            };
        }


        public async Task<AppUser> GetUserByIdAsync(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<bool> ToggleBanStatusAsync(AppUser user)
        {
            user.ProfileStatus = user.ProfileStatus == 1 ? 0 : 1;
            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }
    }
}
