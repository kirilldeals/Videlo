using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace Videlo.Models.Database
{
    [PrimaryKey(nameof(UserId), nameof(VideoId))]
    public class WatchHistory
    {
        [ForeignKey("User")]
        public string UserId { get; set; } = null!;

        [ForeignKey("Video")]
        public string VideoId { get; set; } = null!;

        public DateTime WatchDate { get; set; }

        public User User { get; set; } = null!;
        public Video Video { get; set; } = null!;
    }
}
