namespace HrPanel.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string IdentityNumber { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public DateTime? BirthDate { get; set; }
        public DateTime HireDate { get; set; } = DateTime.Now; 
        public string Address { get; set; } = string.Empty;
        public decimal Salary { get; set; }
        public bool IsActive { get; set; } = true;
        public EmployeeRole Role { get; set; } = EmployeeRole.Employee;

        // HİYERARŞİ 
        public int? ManagerId { get; set; }
        public Employee? Manager { get; set; } //yönetici
        public List<Employee> Subordinates { get; set; } = new(); // yöneiciye bağlı çalışanlar
        public int DepartmentId { get; set; }
        public Department? Department { get; set; }
        public int? PositionId { get; set; }
        public Position? Position { get; set; }

        public List<Leave> Leaves { get; set; } = new();
    }
}
