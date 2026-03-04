using HrPanel.Data;
using HrPanel.Models;
using Microsoft.EntityFrameworkCore;

namespace HrPanel.Services
{
    public class LeaveService
    {
        private readonly AppDbContext _context;

        public LeaveService(AppDbContext context)
        {
            _context = context;
        }

        //izinleri personelleriyle beraber çekme
        public async Task<List<Leave>> GetAllLeavesAsync()
        {
            return await _context.Leaves
                .Include(l => l.Employee)
                .ToListAsync();
        }

        //yeni izin talebi
        public async Task AddLeaveAsync(Leave leave)
        {
            _context.Leaves.Add(leave);
            await _context.SaveChangesAsync();
        }

        //izin durumunu güncelleme 
        public async Task UpdateLeaveStatusAsync(int id, LeaveStatus newStatus)
        {
            var leave = await _context.Leaves.FindAsync(id);
            if (leave != null)
            {
                leave.Status = newStatus;
                await _context.SaveChangesAsync();
            }
        }
    }
}