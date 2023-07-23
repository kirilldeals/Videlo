using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace Videlo.Models.Database
{
    [PrimaryKey(nameof(UserId), nameof(UserChannelId))]
    public class UserSubscription
    {
        [ForeignKey("User")]
        public string UserId { get; set; } = null!;

        [ForeignKey("UserChannel")]
        public string UserChannelId { get; set; } = null!;

        [Column(TypeName = "date")]
        public DateTime SubscriptionDate { get; set; }

        public User User { get; set; } = null!;
        public User UserChannel { get; set; } = null!;
    }
}
