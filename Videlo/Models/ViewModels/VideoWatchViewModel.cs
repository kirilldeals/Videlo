using Videlo.Models.Database;

namespace Videlo.Models.ViewModels
{
    public class VideoWatchViewModel
    {
        public VideoWatchViewModel(Video video)
        {
            Video = video;
        }

        public Video Video { get; set; }
        public VideoFeedback? UserFeedback { get; set; }
        public bool? IsSubbed { get; set; }
    }
}
