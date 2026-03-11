using HrPanel.Data;
using HrPanel.Models;
using Microsoft.EntityFrameworkCore;

namespace HrPanel.Services
{
    public class EducationService
    {
        private readonly AppDbContext _context;

        public EducationService(AppDbContext context)
        {
            _context = context;
        }

        // Bir personele ait tüm eğitim kayıtlarını getirir
        public async Task<List<EmployeeEducation>> GetEmployeeEducationsAsync(int employeeId)
        {
            return await _context.EmployeeEducations
                .Where(e => e.EmployeeId == employeeId)
                .OrderByDescending(e => e.CompletionDate)
                .AsNoTracking()
                .ToListAsync();
        }

        // Yeni eğitim kaydı ekler
        public async Task AddEducationAsync(EmployeeEducation education)
        {
            _context.EmployeeEducations.Add(education);
            await _context.SaveChangesAsync();
        }

        // Eğitim kaydını siler
        public async Task DeleteEducationAsync(int id)
        {
            var education = await _context.EmployeeEducations.FindAsync(id);
            if (education != null)
            {
                _context.EmployeeEducations.Remove(education);
                await _context.SaveChangesAsync();
            }
        }
    }
}
