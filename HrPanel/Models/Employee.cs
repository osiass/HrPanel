namespace HrPanel.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int PositionId { get; set; }
        public decimal Salary { get; set; }
        public int DepartmentId { get; set; }
        public Department? Department { get; set; }
        public bool IsActive { get; set; } = true;
        public Position? Position { get; set; }
    }
}
