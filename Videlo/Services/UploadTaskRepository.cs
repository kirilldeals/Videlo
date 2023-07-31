using System.Collections.Concurrent;
using System.Threading.Tasks;
using Videlo.Data.Enums;
using Videlo.Services.Interfaces;

namespace Videlo.Services
{
    public class UploadTaskRepository : IUploadTaskRepository
    {
        private readonly ConcurrentDictionary<string, Task> _tasks;

        public UploadTaskRepository()
        {
            _tasks = new ConcurrentDictionary<string, Task>();
        }

        public UploadStatus GetUploadStatus(string userId)
        {
            if (_tasks.TryGetValue(userId, out var task))
            {
                if (task.IsCompleted)
                {
                    _tasks.TryRemove(userId, out _);
                    return UploadStatus.Completed;
                }
                return UploadStatus.InProgress;
            }

            return UploadStatus.NotStarted;
        }

        public bool TryAdd(string userId, Task task)
        {
            return _tasks.TryAdd(userId, task);
        }
    }
}
