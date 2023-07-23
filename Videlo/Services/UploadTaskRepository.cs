using System.Threading.Tasks;
using Videlo.Data.Enums;

namespace Videlo.Services
{
    public class UploadTaskRepository
    {
        private readonly Dictionary<string, Task> _tasks;
        private readonly object _lock;

        public UploadTaskRepository()
        {
            _tasks = new Dictionary<string, Task>();
            _lock = new object();
        }

        public UploadStatus GetAndUpdateUploadStatus(string userId)
        {
            lock (_lock)
            {
                if (_tasks.TryGetValue(userId, out var task))
                {
                    if (task.IsCompleted)
                    {
                        _tasks.Remove(userId);
                        return UploadStatus.Completed;
                    }
                    return UploadStatus.InProgress;
                }

                return UploadStatus.NotStarted;
            }
        }

        public void SaveTask(Task task, string userId)
        {
            lock (_lock)
            {
                if (!_tasks.ContainsKey(userId))
                {
                    _tasks[userId] = task;
                }
            }
        }
    }
}
