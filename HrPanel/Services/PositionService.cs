using HrPanel.Data;
using HrPanel.Models;
using System.ComponentModel;
using Microsoft.EntityFrameworkCore;

namespace HrPanel.Services
{
    public class PositionService
    {
        private readonly AppDbContext _context;
        public PositionService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Position>> GetAllPositionsAsync()
        {
            return await _context.Positions.ToListAsync();
        }

        public async Task<Position?> GetPositionByIdAsync(int id)
        {
            return await _context.Positions.FindAsync(id);
        }

        public async Task AddPositionAsync(Position position)
        { 
            _context.Positions.Update(position);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePositionAsync(int id)
        {
            var position = await _context.Positions.FindAsync(id);
            if (position != null)
            {
                _context.Positions.Remove(position);
                await _context.SaveChangesAsync();
            }

        }
    }
}
