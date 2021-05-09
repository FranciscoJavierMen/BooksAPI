using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MoviesApi.Services
{
    public class LocalStorage : IFilesStorage
    {
        private readonly IWebHostEnvironment _env;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LocalStorage(IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
        {
            _env = env;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> EditFile(byte[] content, string extension, string container, string route, string contentType)
        {
            await RemoveFile(route, container);
            return await SaveFile(content, extension, container, contentType);
        }

        public Task RemoveFile(string route, string container)
        {
            if(route != null)
            {
                var fileName = Path.GetFileName(route);
                string fileDirectory = Path.Combine(_env.WebRootPath, container, fileName);

                if (File.Exists(fileDirectory))
                {
                    File.Delete(fileDirectory);
                }
            }
            return Task.FromResult(0);
        }

        public async Task<string> SaveFile(byte[] content, string extension, string container, string contentType)
        {
            var fileName = $"{Guid.NewGuid()}{extension}";
            string folder = Path.Combine(_env.WebRootPath, container);

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            string route = Path.Combine(folder, fileName);
            await File.WriteAllBytesAsync(route, content);

            var request = _httpContextAccessor.HttpContext.Request;
            var actualPath = $"{request.Scheme}://{request.Host}";
            var fileUrl = Path.Combine(actualPath, container, fileName).Replace("\\", "/");
            return fileUrl;
        }
    }
}
