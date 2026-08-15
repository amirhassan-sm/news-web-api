using Application.Contrast.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Hosting;

namespace Infrastructure.Security.Identity.FileStorage
{
    public class UserProfileStorage : IUserProfileStorage
    {
        private readonly IWebHostEnvironment environment;
        public UserProfileStorage(IWebHostEnvironment environment)
        {
            this.environment = environment;

        }
        public Task DeleteProfileAsync(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return Task.CompletedTask;
            }

            var filePath = Path.Combine(
        environment.WebRootPath,
        url.TrimStart('/')
           .Replace('/', Path.DirectorySeparatorChar));

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            return Task.CompletedTask;
        }

        public async Task<string> SaveProfileAsync(Stream fileStream, string fileName)
        {
            var folderPath = Path.Combine(environment.WebRootPath, "images", "profile");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            var extention = Path.GetExtension(fileName);
            var newFileName = $"{Guid.NewGuid()}{extention}";

            var fullPath = Path.Combine(folderPath, newFileName);
            using (var outPut = new FileStream(fullPath, FileMode.Create))
            {
                await fileStream.CopyToAsync(outPut);
            }
            return $"/images/profile/{newFileName}";

        }
    }
}
