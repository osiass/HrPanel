using HrPanel.Data;
using HrPanel.Models;
using Microsoft.EntityFrameworkCore;

namespace HrPanel.Services
{
    public class KanbanService
    {
        private readonly AppDbContext _context;
        public KanbanService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<KanbanTask>> GetAllTaskAsync()
        {
            return await _context.KanbanTasks
                .Include(t => t.AssignedEmployee)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task CreateTaskAsync(KanbanTask task)
        {
            task.CreatedAt = DateTime.UtcNow;
            _context.KanbanTasks.Add(task);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTaskStatusAsync(int taskId, HrPanel.Models.TaskStatus newStatus)
            {
                var task = await _context.KanbanTasks.FindAsync(taskId);
                if (task != null)
                {
                    task.Status = (Models.TaskStatus)newStatus;
                    await _context.SaveChangesAsync();
                }
        }

        public async Task DeleteTaskAsync(int taskId)
        {
            var task = await _context.KanbanTasks.FindAsync(taskId);
            if (task != null)
            {
                _context.KanbanTasks.Remove(task);
                await _context.SaveChangesAsync();
            }
        }
    }
}
