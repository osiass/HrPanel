using HrPanel.Data;
using HrPanel.Models;
using Microsoft.EntityFrameworkCore;

namespace HrPanel.Services
{
    /*etkinlik servisi*/
    public class EventService
    {
        private readonly AppDbContext _context;

        public EventService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<CompanyEvent>> GetAllEventsAsync()
        {
            return await _context.CompanyEvents
                .OrderBy(e => e.StartDate)
                .ToListAsync();
        }

        // Belirli bir aydaki etkinlikleri getir
        public async Task<List<CompanyEvent>> GetEventsByMonthAsync(int month, int year)
        {
            return await _context.CompanyEvents
                .Where(e => e.StartDate.Month == month && e.StartDate.Year == year)
                .OrderBy(e => e.StartDate)
                .ToListAsync();
        }

        public async Task AddEventAsync(CompanyEvent companyEvent)
        {
            _context.CompanyEvents.Add(companyEvent);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteEventAsync(int id)
        {
            var companyEvent = await _context.CompanyEvents.FindAsync(id);
            if (companyEvent != null)
            {
                _context.CompanyEvents.Remove(companyEvent);
                await _context.SaveChangesAsync();
            }
        }
    }
}
