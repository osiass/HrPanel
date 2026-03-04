namespace HrPanel.Models
{
    public class Position
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        List<Employee> Employees { get; set; } = new List<Employee>();
    }
}
