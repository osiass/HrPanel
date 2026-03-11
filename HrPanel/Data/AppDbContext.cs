using Microsoft.EntityFrameworkCore;
using HrPanel.Models;
namespace HrPanel.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Leave> Leaves { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<KanbanTask> KanbanTasks { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<AdvanceRequest> AdvanceRequests { get; set; }
        public DbSet<EmployeeEducation> EmployeeEducations { get; set; }
        public DbSet<SupportRequest> SupportRequests { get; set; }
        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<CompanyEvent> CompanyEvents { get; set; }
    }
}
