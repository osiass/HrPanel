namespace HrPanel.Models
{
    public enum LeaveStatus { Pending, Approved, Rejected }
    public enum LeaveType { Annual, Sick, Unpaid }
    public class Leave
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }
        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime EndDate { get; set; } = DateTime.Now.AddDays(1);
        public string? Description { get; set; }
        public LeaveStatus Status { get; set; } = LeaveStatus.Pending;
        public LeaveType Type { get; set; } = LeaveType.Annual;
    }
}
