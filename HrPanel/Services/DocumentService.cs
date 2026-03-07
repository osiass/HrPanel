using HrPanel.Data;
using HrPanel.Models;
using Microsoft.EntityFrameworkCore;

namespace HrPanel.Services
{
    public class DocumentService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public DocumentService(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task SaveDocumentAsync(Stream fileStream, string fileName, int employeeId)
        {
            var uploadPath = Path.Combine(_environment.WebRootPath, "uploads");

            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            var extension = Path.GetExtension(fileName);
            var uniqueFileName = $"{Guid.NewGuid()}{extension}";// Benzersiz bir dosya adı oluştur id
            var path = Path.Combine(uploadPath, uniqueFileName);

            using (var fileStreamOutput = new FileStream(path, FileMode.Create))
            {
                await fileStream.CopyToAsync(fileStreamOutput);
            }

            var document = new Document
            {
                FileName = fileName,
                FilePath = $"/uploads/{uniqueFileName}",
                EmployeeId = employeeId,
                UploadDate = DateTime.UtcNow
            };

            _context.Documents.Add(document);
            await _context.SaveChangesAsync();
        }

        // Personelin belgelerini getiren metot
        public async Task<List<Document>> GetEmployeeDocumentsAsync(int employeeId)
        {
            return await _context.Documents
                .Where(d => d.EmployeeId == employeeId)
                .OrderByDescending(d => d.UploadDate)
                .ToListAsync();
        }

        // Belgeyi silme metodu
        public async Task DeleteDocumentAsync(int documentId)
        {
            var doc = await _context.Documents.FindAsync(documentId);
            if (doc != null)
            {
                // Fiziksel dosyayı sil
                var physicalPath = Path.Combine(_environment.WebRootPath, "uploads", doc.FilePath);
                if (File.Exists(physicalPath))
                {
                    File.Delete(physicalPath);
                }

                // Veritabanından sil
                _context.Documents.Remove(doc);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<byte[]> GetFileBytesAsync(int documentId)
        {
            var doc = await _context.Documents.FindAsync(documentId);
            if (doc == null)
            {
                return null;
            }
            var physicalPath = Path.Combine(_environment.WebRootPath, "uploads", doc.FilePath);
            if (!File.Exists(physicalPath)) return null;

            return await File.ReadAllBytesAsync(physicalPath);
        }
    }
}
