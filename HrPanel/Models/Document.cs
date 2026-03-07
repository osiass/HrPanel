namespace HrPanel.Models
{
    public class Document
    {
        public int Id { get; set; }
        public string FileName { get; set; } = string.Empty; 
        public string FilePath { get; set; } = string.Empty; 
        public string FileType { get; set; } = string.Empty; 
        public DateTime UploadDate { get; set; } = DateTime.Now;

        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }
    }
}