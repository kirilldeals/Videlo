using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Videlo.Models.Database
{
    [PrimaryKey(nameof(CommentId), nameof(UserId))]
    public class VideoCommentFeedback
    {
        [ForeignKey("VideoComment")]
        public string CommentId { get; set; } = null!;

        [ForeignKey("User")]
        public string UserId { get; set; } = null!;

        public bool IsLike { get; set; }

        public VideoComment VideoComment { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}
