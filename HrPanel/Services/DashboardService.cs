using HrPanel.Data;
using HrPanel.Models;
using Microsoft.EntityFrameworkCore;

namespace HrPanel.Services
{
    // Dashboard verilerini taşımak için bir sınıf
    public class DashboardStats
    {
        public int TotalEmployees { get; set; }
        public int ActiveEmployees { get; set; }
        public int TotalDepartments { get; set; }
        public decimal AverageSalary { get; set; }
        public List<Employee> RecentEmployees { get; set; } = new List<Employee>();
        
        // Grafik Verileri için eklemeler
        public double[] LeaveTypeData { get; set; } = new double[3]; // Annual, Sick, Unpaid
        public string[] LeaveTypeLabels { get; set; } = { "Yıllık İzin", "Hastalık", "Ücretsiz İzin" };
        public double[] MonthlyHiringData { get; set; } = new double[6]; // Son 6 ay
        public string[] MonthlyHiringLabels { get; set; } = new string[6];
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

            //izin dağılmı chart
            stats.LeaveTypeData[0] = await _context.Leaves.CountAsync(l => l.Type == LeaveType.Annual);
            stats.LeaveTypeData[1] = await _context.Leaves.CountAsync(l => l.Type == LeaveType.Sick);
            stats.LeaveTypeData[2] = await _context.Leaves.CountAsync(l => l.Type == LeaveType.Unpaid);

            //son 6 aylık işe alım chart 
            for (int i = 0; i < 6; i++)
            {
                var targetMonth = DateTime.Now.AddMonths(-i);//bugünden i ay geriye git
                stats.MonthlyHiringLabels[5 - i] = targetMonth.ToString("MMM");//ocaa şub gibi al
                stats.MonthlyHiringData[5 - i] = await _context.Employees//5-i diziyi sondan başa doldur
                    .CountAsync(e => e.HireDate.Month == targetMonth.Month && e.HireDate.Year == targetMonth.Year);
            }

            return stats;
        }
    }
}