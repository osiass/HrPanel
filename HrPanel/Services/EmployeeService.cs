using HrPanel.Data;
using HrPanel.Models;
using Microsoft.EntityFrameworkCore;

namespace HrPanel.Services
{
    public class EmployeeService
    {
        private readonly AppDbContext _context;

        // dependency injection Veritabanı bağlantısını constructor üzerinden alıyoruz
        public EmployeeService(AppDbContext context)
        {
            _context = context;
        }

        //tüm personelleri getir
        public async Task<List<Employee>> GetAllEmployeesAsync()
        {
            return await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Position)
                .AsNoTracking() // Performans sadece okuma yaparken kullanılır
                .ToListAsync();
        }

        //idye göre tek personel getir
        public async Task<Employee?> GetEmployeeByIdAsync(int id)
        {
            return await _context.Employees.FindAsync(id);
        }

        // yeni personel ekle
        public async Task AddEmployeeAsync(Employee employee)
        {
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
        }

        // mevcut personel günncelle
        public async Task UpdateEmployeeAsync(Employee employee)
        {
            //veritabanındaki orijinal kaydı bul
            var existingEmployee = await _context.Employees.FindAsync(employee.Id);

            if (existingEmployee != null)
            {
                //formdan gelen değerlerle güncelle
                _context.Entry(existingEmployee).CurrentValues.SetValues(employee);

                //veritabanında güncelle
                await _context.SaveChangesAsync();
            }
        }
        //personeli sil
        public async Task DeleteEmployeeAsync(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
            }
        }
    }
}