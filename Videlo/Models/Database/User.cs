using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace Videlo.Models.Database
{
    public class User : IdentityUser
    {
        [Column(TypeName = "date")]
        public DateTime CreatedAt { get; set; }

        public ICollection<Video> Videos { get; set; } = new List<Video>();
        public ICollection<VideoFeedback> VideoFeedbacks { get; set; } = new List<VideoFeedback>();
        public ICollection<VideoComment> VideoComments { get; set; } = new List<VideoComment>();
        public ICollection<VideoCommentFeedback> VideoCommentFeedbacks { get; set; } = new List<VideoCommentFeedback>();
        public ICollection<UserSubscription> UserSubscriptions { get; set; } = new List<UserSubscription>();
        public ICollection<UserSubscription> UserSubscribers { get; set; } = new List<UserSubscription>();
        public ICollection<WatchHistory> WatchHistories { get; set; } = new List<WatchHistory>();
    }
}
