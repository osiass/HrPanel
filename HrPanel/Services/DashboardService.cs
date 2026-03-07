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

            //Toplam Personel admin hariç
            stats.TotalEmployees = await _context.Employees
                .CountAsync(e => e.Role != EmployeeRole.Admin);

            //Departman Sayısı
            stats.TotalDepartments = await _context.Departments.CountAsync();

            // aktif çalışan admin hariç
            stats.ActiveEmployees = await _context.Employees
                .CountAsync(e => e.IsActive && e.Role != EmployeeRole.Admin);

            // ort maaş admin hariç
            if (stats.TotalEmployees > 0)
            {
                stats.AverageSalary = await _context.Employees
                    .Where(e => e.Role != EmployeeRole.Admin && e.Salary > 0)
                    .AverageAsync(e => e.Salary);
            }

            //son eklenen personeller admin hariç
            stats.RecentEmployees = await _context.Employees
                .Include(e => e.Department)
                .Where(e => e.Role != EmployeeRole.Admin) 
                .OrderByDescending(e => e.Id)
                .Take(5)
                .ToListAsync();

            return stats;
        }
    }
}