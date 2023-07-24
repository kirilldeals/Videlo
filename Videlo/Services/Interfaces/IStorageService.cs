using System.Net;
using Videlo.Models;

namespace Videlo.Services.Interfaces
{
    public interface IStorageService
    {
        Task<string> UploadVideoAsync(FormFileInfo file);

        Task<string> UploadVideoThumbnailAsync(FormFileInfo file);

        Task<HttpStatusCode> DeleteFileAsync(string filePath);
    }
}
