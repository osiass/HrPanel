using HrPanel.Data;
using HrPanel.Models;
using Microsoft.EntityFrameworkCore;

namespace HrPanel.Services
{
    /* duyuru servisi*/
    public class AnnouncementService
    {
        private readonly AppDbContext _context;

        public AnnouncementService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Announcement>> GetAllAnnouncementsAsync()
        {
            return await _context.Announcements
                .OrderByDescending(a => a.CreatedDate)
                .ToListAsync();
        }

        public async Task<List<Announcement>> GetActiveAnnouncementsAsync()
        {
            return await _context.Announcements
                .Where(a => a.IsActive)
                .OrderByDescending(a => a.Priority == "Yüksek" ? 1 : 0)
                .ThenByDescending(a => a.CreatedDate)
                .ToListAsync();
        }

        public async Task AddAnnouncementAsync(Announcement announcement)
        {
            _context.Announcements.Add(announcement);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAnnouncementAsync(int id)
        {
            var announcement = await _context.Announcements.FindAsync(id);
            if (announcement != null)
            {
                _context.Announcements.Remove(announcement);
                await _context.SaveChangesAsync();
            }
        }
    }
}
