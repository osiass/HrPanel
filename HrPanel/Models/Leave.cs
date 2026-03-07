namespace HrPanel.Models
{
    public enum LeaveStatus { Approved, Rejected, PendingHR, PendingManager }
    public enum LeaveType { Annual, Sick, Unpaid }
    public enum EmployeeRole { Admin, Manager, Employee }
    public class Leave
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }
        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime EndDate { get; set; } = DateTime.Now.AddDays(1);
        public string? Description { get; set; }
        public LeaveStatus Status { get; set; } = LeaveStatus.PendingManager;
        public LeaveType Type { get; set; } = LeaveType.Annual;
        public string? ManagerNote { get; set; }
    }
}
