using HrPanel.Data;
using HrPanel.Models;
using Microsoft.EntityFrameworkCore;

namespace HrPanel.Services
{
    public class AdvanceService
    {
        private readonly AppDbContext _context;

        public AdvanceService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<AdvanceRequest>> GetAllAdvancesAsync()
        {
            return await _context.AdvanceRequests
                .Include(a => a.Employee)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task AddAdvanceRequestAsync(AdvanceRequest request)
        {
            _context.AdvanceRequests.Add(request);
            await _context.SaveChangesAsync();
        }

        public async Task<List<AdvanceRequest>> GetMyAdvancesAsync(int employeeId)
        {
            return await _context.AdvanceRequests
                .Include(a => a.Employee)
                .Where(a => a.EmployeeId == employeeId)
                .OrderByDescending(a => a.RequestDate)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<AdvanceRequest>> GetPendingAdvancesAsync(int currentUserId, EmployeeRole role)
        {
            if (role == EmployeeRole.Employee)
            {
                return new List<AdvanceRequest>();
            }

            var query = _context.AdvanceRequests
                .Include(a => a.Employee)
                .ThenInclude(e => e.Department)
                .AsQueryable();

            if (role == EmployeeRole.Manager)
            {
                // Manager kendisine bağlı personelin onay bekleyenlerini görür.
                query = query.Where(a => a.Employee.ManagerId == currentUserId && a.Status == AdvanceStatus.Pending);
            }
            else if (role == EmployeeRole.Admin)
            {
                // Admin tüm onay bekleyenleri görebilir.
                query = query.Where(a => a.Status == AdvanceStatus.Pending);
            }

            return await query.AsNoTracking().ToListAsync();
        }

        public async Task ProcessAdvanceApprovalAsync(int requestId, bool isApproved, string? note)
        {
            var request = await _context.AdvanceRequests.FindAsync(requestId);
            if (request == null) return;

            if (isApproved)
            {
                request.Status = AdvanceStatus.Approved;
            }
            else
            {
                request.Status = AdvanceStatus.Rejected;
                request.ManagerNote = note;
            }

            await _context.SaveChangesAsync();
        }
    }
}
