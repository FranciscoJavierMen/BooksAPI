using System.Threading.Tasks;

namespace MoviesApi.Services
{
    public interface IFilesStorage
    {
        Task<string> EditFile(byte[] content, string extension, string container, string route, string contentType);

        Task RemoveFile(string route, string container);

        Task<string> SaveFile(byte[] content, string extension, string container, string contentType);
    }
}
