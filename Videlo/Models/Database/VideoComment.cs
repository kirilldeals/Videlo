using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Videlo.Models.Database
{
    public class VideoComment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; } = null!;

        public string Content { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        [ForeignKey("Video")]
        public string VideoId { get; set; } = null!;

        [ForeignKey("User")]
        public string UserId { get; set; } = null!;

        public Video Video { get; set; } = null!;
        public User User { get; set; } = null!;
        public ICollection<VideoCommentFeedback> VideoCommentFeedbacks { get; set; } = new List<VideoCommentFeedback>();
    }
}
