using HrPanel.Data;
using HrPanel.Models;
using Microsoft.EntityFrameworkCore;

namespace HrPanel.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;

        public AuthService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> LoginAsync(string username, string password)
        {
            return await _context.Users
                .Include(u => u.Employee)
                .FirstOrDefaultAsync(u => u.Username == username && u.Password == password);
        }

        public async Task<bool> ChangePasswordAsync(int employeeId, string newPassword)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.EmployeeId == employeeId);
            if (user != null)
            {
                user.Password = newPassword;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
