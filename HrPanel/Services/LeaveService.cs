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

        public async Task<List<Leave>> GetPendingLeavesAsync(int currentUserId, EmployeeRole role)
        {
            // KRİTİK: Eğer kullanıcı Employee ise onay bekleyen hiçbir şeyi göremez.
            if (role == EmployeeRole.Employee)
            {
                return new List<Leave>();
            }

            var query = _context.Leaves
                .Include(l => l.Employee)
                .ThenInclude(e => e.Department)
                .AsQueryable();

            if (role == EmployeeRole.Manager)
            {
                // Manager sadece kendine bağlı personellerin 
                // ve amir onayı bekleyen PendingManager izinlerini görür.
                query = query.Where(l => l.Employee.ManagerId == currentUserId && l.Status == LeaveStatus.PendingManager);
            }
            else if (role == EmployeeRole.Admin)
            {
                // Admin hem amir onayındakileri hem de İK onayındakileri (PendingHR) görür.
                query = query.Where(l => l.Status == LeaveStatus.PendingHR || l.Status == LeaveStatus.PendingManager);
            }

            return await query.ToListAsync();
        }

        //çok aşamalı onaylama
        public async Task ProcessApprovalAsync(int leaveId, bool isApproved, string? note)
        {
            var leave = await _context.Leaves.FindAsync(leaveId);
            if (leave == null) return;

            if (isApproved)//amir onaylandıysa ikya gönder ik onayladıysa onaylandı yap
            {
                if (leave.Status == LeaveStatus.PendingManager)
                {
                    leave.Status = LeaveStatus.PendingHR;
                }
                else if (leave.Status == LeaveStatus.PendingHR)
                {
                    leave.Status = LeaveStatus.Approved;
                }
            }
            else
            {
                leave.Status = LeaveStatus.Rejected;
                leave.ManagerNote = note;
            }

            await _context.SaveChangesAsync();
        }

        public async Task<List<Leave>> GetMyLeavesAsync(int employeeId)
        {
            return await _context.Leaves
                .Where(l => l.EmployeeId == employeeId)
                .OrderByDescending(l => l.StartDate)
                .ToListAsync();
        }
    }
}