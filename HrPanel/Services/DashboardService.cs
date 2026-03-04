using HrPanel.Data;
using HrPanel.Models;
using Microsoft.EntityFrameworkCore;

namespace HrPanel.Services
{
    // Dashboard verilerini taşımak için basit bir sınıf
    public class DashboardStats
    {
        public int TotalEmployees { get; set; }
        public int ActiveEmployees { get; set; }
        public int TotalDepartments { get; set; }
        public decimal AverageSalary { get; set; }
        public List<Employee> RecentEmployees { get; set; } = new List<Employee>();
    }

    public class DashboardService
    {
        private readonly AppDbContext _context;
        public DashboardService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardStats> GetDashboardStatsAsync()
        {
            var stats = new DashboardStats();

            stats.TotalEmployees = await _context.Employees.CountAsync();
            stats.TotalDepartments = await _context.Departments.CountAsync();
            stats.ActiveEmployees = await _context.Employees.CountAsync(e => e.IsActive);
            stats.RecentEmployees = await _context.Employees
                .OrderByDescending(e => e.Id)
                .Take(5)
                .Include(e => e.Department)
                .ToListAsync();

            if (stats.TotalEmployees > 0)
            {
                stats.AverageSalary = await _context.Employees.AverageAsync(e => e.Salary);
            }

            return stats;
        }
    }
}