using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HrPanel.Models
{
    public enum AdvanceStatus { Pending, Approved, Rejected }

    public class AdvanceRequest
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }

        [Required(ErrorMessage = "Tutar girilmesi zorunludur.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        public DateTime RequestDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Açıklama girilmesi zorunludur.")]
        public string Description { get; set; } = string.Empty;

        public AdvanceStatus Status { get; set; } = AdvanceStatus.Pending;

        public int InstallmentCount { get; set; } = 1; // Taksit sayısı

        public string? ManagerNote { get; set; }
    }
}
