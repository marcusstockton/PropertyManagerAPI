using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using PropertyManager.Api.Interfaces;

namespace PropertyManager.Api.Services
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _env;

        public FileService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<string> SaveFile(IFormFile file, Guid entityId)
        {
            var folderName = Path.Combine("Uploads", "Images", entityId.ToString());
            var pathToSave = Path.Combine(_env.ContentRootPath, folderName);
            if (!Directory.Exists(pathToSave) && file.Length > 0)
            {
                Directory.CreateDirectory(pathToSave);
            }
            if (file.Length > 0)
            {
                var filename = GetSafeFileName(file.FileName);
                using (var fileStream = new FileStream(Path.Combine(pathToSave, filename), FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                    return Path.Combine(folderName, filename);
                }
            }
            return string.Empty;
        }

        public async Task<string> FileToBase64String(string fileLocation)
        {
            if (string.IsNullOrEmpty(fileLocation))
            {
                return string.Empty;
            }
            var bytes = await File.ReadAllBytesAsync(Path.Combine(_env.ContentRootPath, fileLocation));
            return Convert.ToBase64String(bytes);
        }

        private static string GetSafeFileName(string name, char replace = '_')
        {
            char[] invalids = Path.GetInvalidFileNameChars();
            return new string(name.Select(c => invalids.Contains(c) ? replace : c).ToArray());
        }
    }
}