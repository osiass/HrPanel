using System;
using System.ComponentModel.DataAnnotations;

namespace HrPanel.Models
{
    /*etkinlik modeli takvim üzerinde gösterilecek tatiller,- toplantılar ve özel günler için kullanılır.*/
    public class CompanyEvent
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Etkinlik adı zorunludur.")]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        //etkinlik tipi
        public string Type { get; set; } = "Toplantı";

        public string Color { get; set; } = "#4e73df";
    }
}
