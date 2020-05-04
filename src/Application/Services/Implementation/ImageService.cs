using Application.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Application.Services.Implementation
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;

        public ImageService(IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
        }

        public async Task<string> UploadImage(IFormFile img)
        {
            if (img == null)
            {
                return null;
            }

            var folderName = _configuration.GetSection("StorageConfiguration")["FolderForBookImages"];
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, folderName);
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + img.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            var dbPath = Path.Combine(folderName, uniqueFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await img.CopyToAsync(fileStream);
            }
            return dbPath;

        }

        public void DeleteImage(string imagePath)
        {
            string deletePath = Path.Combine(_webHostEnvironment.WebRootPath, imagePath);
            var file = new FileInfo(deletePath);
            if (file.Exists)
            {
                file.Delete();
            }
        }
    }
}
