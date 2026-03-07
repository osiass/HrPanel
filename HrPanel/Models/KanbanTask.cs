namespace HrPanel.Models
{
    public enum TaskStatus
    {
        ToDo,
        InProgress,
        Done
    }
    public enum TaskPriority
    {
        Low,
        Medium,
        High
    }
    public class KanbanTask
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TaskPriority Priority { get; set; } = TaskPriority.Medium;
        public TaskStatus Status { get; set; } = TaskStatus.ToDo;
        public int AssignedEmployeeId { get; set; }
        public Employee? AssignedEmployee { get; set; }//ilişki
        public DateTime CreatedAt { get; set; }
        
    }
}
