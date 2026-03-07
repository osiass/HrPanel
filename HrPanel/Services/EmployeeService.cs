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

        //tüm personelleri getira
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
            return await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Position)
                .Include(e => e.Manager) // Amirini getir
                .Include(e => e.Subordinates) // Ona bağlı çalışanları getir
                .Include(e => e.Leaves)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        // yeni personel ekle
        public async Task AddEmployeeAsync(Employee employee)
        {
            //Veritabanı işlemleri için bir transaction 
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                //Email kontrolü split işleminde hata almamak için 
                if (string.IsNullOrWhiteSpace(employee.Email))
                    throw new Exception("E-posta adresi olmadan kullanıcı oluşturulamaz.");

                // personeli employees tablosuna ekliyoruz
                _context.Employees.Add(employee);
                await _context.SaveChangesAsync(); // Personel id burada oluşur

                //  OTOMATİK KULLANICI HESABI OLUŞTURMA
                var newUser = new User
                {
                    // Kullanıcı adını emailin ilk kısmından veya isminden otomatik türetiyoruz
                    Username = employee.Email.Split('@')[0],
                    Password = employee.IdentityNumber, //şifre oto tc no oldu
                    FullName = employee.FullName,
                    Role = employee.Role.ToString(),
                    EmployeeId = employee.Id // Personel tablosuna bağlıyoruz
                };

                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();

                //eğer buraya kadar hatasız gelindiyse, tüm işlemleri sqle kalıcı olarak işle
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                // bir hata oluşursa  eklenen personeli de geri siler iptal eder
                await transaction.RollbackAsync();
                throw; // Hatayı yukarıya (arayüze) fırlat
            }
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
                existingEmployee.Role = employee.Role;

                // kullanıcı tablosunuda güncelle
                var linkedUser = await _context.Users.FirstOrDefaultAsync(u => u.EmployeeId == employee.Id);
                if (linkedUser != null)
                {
                    linkedUser.FullName = employee.FullName;
                    linkedUser.Role = employee.Role.ToString();
                }

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
                // Bağlı kullanıcıyı da sil 
                var user = await _context.Users.FirstOrDefaultAsync(u => u.EmployeeId == id);
                if (user != null) _context.Users.Remove(user);

                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Employee?> GetEmployeeWithDetailsAsync(int id)
        {
            return await _context.Employees
                .Include(e => e.Department)    //departman bilgisi gelsin
                .Include(e => e.Position)      //pozisyon bilgisi gelsin
                .Include(e => e.Manager)       //personelin amiri kimse o gelsin
                .Include(e => e.Subordinates)  //personelin altındaki ekip üyeleri  gelsin
                    .ThenInclude(s => s.Position)
                .Include(e => e.Leaves)        //izin geçmişi listesi gelsin
                .FirstOrDefaultAsync(e => e.Id == id);
        }
    }
}