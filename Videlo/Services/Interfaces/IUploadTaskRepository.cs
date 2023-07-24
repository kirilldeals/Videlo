using Videlo.Data.Enums;

namespace Videlo.Services.Interfaces
{
    public interface IUploadTaskRepository
    {
        UploadStatus GetAndUpdateUploadStatus(string userId);

        void SaveTask(Task task, string userId);
    }
}
