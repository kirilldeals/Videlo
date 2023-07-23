using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Videlo.Models.Database
{
    public class Video
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; } = null!;

        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public int ViewCount { get; set; }

        public DateTime CreatedAt { get; set; }

        public string VideoPath { get; set; } = null!;

        public string ThumbnailPath { get; set; } = null!;

        [ForeignKey("User")]
        public string UserId { get; set; } = null!;

        public User User { get; set; } = null!;
        public ICollection<VideoFeedback> VideoFeedbacks { get; set; } = new List<VideoFeedback>();
        public ICollection<VideoComment> VideoComments { get; set; } = new List<VideoComment>();
        public ICollection<WatchHistory> WatchHistories { get; set; } = new List<WatchHistory>();
    }
}
