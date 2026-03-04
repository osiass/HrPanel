using HrPanel.Data;
using HrPanel.Models;
using Microsoft.EntityFrameworkCore;

namespace HrPanel.Services
{
    public class ReportService
    {
        private readonly AppDbContext _context;
        public ReportService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Employee>> GetFilteredEmployeesAsync(string? searchTerm, int? deptId, decimal? minSalary)
        {
            var query = _context.Employees.Include(e => e.Department).Include(e=> e.Position).AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(e => e.FullName.Contains(searchTerm));
            }

            if (deptId.HasValue && deptId > 0)
            {
                query = query.Where(e => e.DepartmentId == deptId.Value);
            }

            if (minSalary.HasValue)
            {
                query = query.Where(e => e.Salary >= minSalary.Value);
            }

            return await query.ToListAsync();
        }
    }
}