using System;
using System.ComponentModel.DataAnnotations;

namespace HrPanel.Models
{
    /*duyuru modeli*/
    public class Announcement
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Başlık zorunludur.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "İçerik zorunludur.")]
        public string Content { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Duyurunun aktiflik durumu
        public bool IsActive { get; set; } = true;

        // öncelik
        public string Priority { get; set; } = "Normal";
    }
}
