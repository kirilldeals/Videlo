using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Videlo.Models.Database
{
    [PrimaryKey(nameof(VideoId), nameof(UserId))]
    public class VideoFeedback
    {
        [ForeignKey("Video")]
        public string VideoId { get; set; } = null!;

        [ForeignKey("User")]
        public string UserId { get; set; } = null!;

        public bool IsLike { get; set; }

        public DateTime CreatedAt { get; set; }

        public Video Video { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}
