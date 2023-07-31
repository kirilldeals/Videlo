using Videlo.Data.Enums;

namespace Videlo.Services.Interfaces
{
    public interface IUploadTaskRepository
    {
        UploadStatus GetUploadStatus(string userId);

        bool TryAdd(string userId, Task task);
    }
}
