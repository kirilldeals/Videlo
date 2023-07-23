using GamevaWeb.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Reflection.Emit;
using Videlo.Models.Database;

namespace Videlo.Data;

public class VideloContext : IdentityDbContext<User>
{
    public VideloContext(DbContextOptions<VideloContext> options)
        : base(options)
    {
    }

    public DbSet<Video> Videos { get; set; }
    public DbSet<VideoFeedback> VideoFeedbacks { get; set; }
    public DbSet<VideoComment> VideoComments { get; set; }
    public DbSet<VideoCommentFeedback> VideoCommentFeedbacks { get; set; }
    public DbSet<UserSubscription> UserSubscriptions { get; set; }
    public DbSet<WatchHistory> WatchHistories { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<VideoComment>()
            .HasOne(vc => vc.Video)
            .WithMany(v => v.VideoComments)
            .HasForeignKey(vc => vc.VideoId)
            .OnDelete(DeleteBehavior.ClientCascade);

        builder.Entity<VideoFeedback>()
            .HasOne(vf => vf.Video)
            .WithMany(v => v.VideoFeedbacks)
            .HasForeignKey(vf => vf.VideoId)
            .OnDelete(DeleteBehavior.ClientCascade);

        builder.Entity<VideoCommentFeedback>()
            .HasOne(vcf => vcf.VideoComment)
            .WithMany(vc => vc.VideoCommentFeedbacks)
            .HasForeignKey(vcf => vcf.CommentId)
            .OnDelete(DeleteBehavior.ClientCascade);

        builder.Entity<UserSubscription>()
            .HasOne(us => us.User)
            .WithMany(u => u.UserSubscriptions)
            .HasForeignKey(us => us.UserId)
            .OnDelete(DeleteBehavior.ClientCascade);

        builder.Entity<UserSubscription>()
            .HasOne(us => us.UserChannel)
            .WithMany(u => u.UserSubscribers)
            .HasForeignKey(us => us.UserChannelId)
            .OnDelete(DeleteBehavior.ClientCascade);

        builder.Entity<WatchHistory>()
            .HasOne(wh => wh.Video)
            .WithMany(v => v.WatchHistories)
            .HasForeignKey(wh => wh.VideoId)
            .OnDelete(DeleteBehavior.ClientCascade);
    }
}
