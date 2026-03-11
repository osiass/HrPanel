using HrPanel.Data;
using HrPanel.Models;
using Microsoft.EntityFrameworkCore;

namespace HrPanel.Services
{
    /*
        Destek servisi
    */
    public class SupportService
    {
        private readonly AppDbContext _context;

        public SupportService(AppDbContext context)
        {
            _context = context;
        }

        // Tüm destek taleplerini ilişkili personel verisiyle birlikte getirir
        public async Task<List<SupportRequest>> GetAllRequestsAsync()
        {
            return await _context.SupportRequests
                .Include(s => s.RequesterEmployee)
                .OrderByDescending(s => s.CreatedDate)
                .ToListAsync();
        }

        //giriş yapan çalışanın kendi taleplerini getir
        public async Task<List<SupportRequest>> GetMyRequestsAsync(int employeeId)
        {
            return await _context.SupportRequests
                .Where(s => s.RequesterEmployeeId == employeeId)
                .OrderByDescending(s => s.CreatedDate)
                .ToListAsync();
        }

        //yeni bir destek talebi ekle
        public async Task AddRequestAsync(SupportRequest request)
        {
            _context.SupportRequests.Add(request);
            await _context.SaveChangesAsync();
        }

        // Talebin durumunu güncelle
        public async Task UpdateStatusAsync(int id, string newStatus)
        {
            var request = await _context.SupportRequests.FindAsync(id);
            if (request != null)
            {
                request.Status = newStatus;
                await _context.SaveChangesAsync();
            }
        }
    }
}
