using System;
using System.IO;
using System.Threading.Tasks;
using Crypto.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;

namespace WebUI.Services
{
    public class ImageAccessor : IImageAccessor
    {
        private readonly IDateTime _dateTime;

        public ImageAccessor(IDateTime dateTime)
        {
            _dateTime = dateTime;
        }

        public async Task<string> Upload(IFormFile file, string userId, string fileName)
        {
            try
            {
                var root = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

                var usersFolder = Path.Combine(root, "users");
                if (!Directory.Exists(usersFolder))
                    Directory.CreateDirectory(usersFolder);
                
                var userFolder = Path.Combine(usersFolder, userId);
                if (!Directory.Exists(userFolder))
                    Directory.CreateDirectory(userFolder);
                
                var date = _dateTime.Now.ToLongDateString();
                fileName = date + " - " + fileName;
                var extension = Path.GetExtension(file.FileName);
                fileName += extension;
                
                var filePath = Path.Combine(userFolder, fileName);
                await using var stream = new FileStream(filePath, FileMode.Create);
                await file.CopyToAsync(stream);

                return $"/users/{userId}/{fileName}";
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}