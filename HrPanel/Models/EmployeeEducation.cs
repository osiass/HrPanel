using System.ComponentModel.DataAnnotations;

namespace HrPanel.Models
{
    public class EmployeeEducation
    {
        public int Id { get; set; }

        [Required]
        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }

        [Required]
        public string CourseName { get; set; } = string.Empty;

        public string? Provider { get; set; } //eğitimi veren kuruluş 

        public string? Status { get; set; } //devam ediyor tamamlandı 

        public DateTime? CompletionDate { get; set; }
    }
}
