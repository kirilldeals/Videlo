using Videlo.Models.Database;

namespace Videlo.Models.ViewModels
{
    public class ChannelVideosViewModel
    {
        public ChannelVideosViewModel(ICollection<Video> videos, User user) 
        {
            Videos = videos;
            User = user;
        }

        public ICollection<Video> Videos { get; set; } = new List<Video>();
        public User User { get; set; } = null!;
    }
}
