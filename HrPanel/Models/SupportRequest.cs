using System;
using System.ComponentModel.DataAnnotations;

namespace HrPanel.Models
{

    public class SupportRequest
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Konu başlığı zorunludur.")]
        public string Subject { get; set; } = string.Empty;

        [Required(ErrorMessage = "Açıklama zorunludur.")]
        public string Description { get; set; } = string.Empty;

        // Talebi açan personel
        public int RequesterEmployeeId { get; set; }
        public Employee? RequesterEmployee { get; set; }

        public string Product { get; set; } = "İnsan Kaynakları";


        public string Status { get; set; } = "Yeni";

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public int CommentCount { get; set; } = 0;
        public int FileCount { get; set; } = 0;
    }
}
