using Videlo.Models.Database;

namespace Videlo.Models.ViewModels
{
    public class PlaylistViewModel
    {
        public PlaylistViewModel(IEnumerable<VideoFeedback> videos, bool likeFilter) 
        { 
            Feedbacks = videos;
            LikeFilter = likeFilter;
        }

        public IEnumerable<VideoFeedback> Feedbacks { get; set; }
        public bool LikeFilter { get; set; }
    }
}
