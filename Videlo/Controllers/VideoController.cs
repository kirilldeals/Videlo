using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Videlo.Data;
using Videlo.Models.Database;
using Videlo.Models.ViewModels;

namespace Videlo.Controllers
{
    public class VideoController : Controller
    {
        private readonly VideloContext _db;
        private readonly UserManager<User> _userManager;

        public VideoController(VideloContext db, UserManager<User> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Watch(string videoId)
        {
            if (videoId == null)
            {
                return NotFound();
            }

            var video = await _db.Videos
                .Include(v => v.User)
                .Include(v => v.VideoFeedbacks)
                .Include(v => v.VideoComments)
                .FirstOrDefaultAsync(v => v.Id == videoId);
            if (video == null)
            {
                return NotFound();
            }

            var model = new VideoWatchViewModel(video);

            var curUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (curUserId != null)
            {
                var curUser = await _db.Users
                    .Include(u => u.UserSubscriptions)
                    .Include(u => u.VideoFeedbacks)
                    .FirstOrDefaultAsync(u => u.Id == curUserId);
                if (curUser != null)
                {
                    model.UserFeedback = curUser.VideoFeedbacks
                        .FirstOrDefault(f => f.VideoId == video.Id);
                    model.IsSubbed = curUser.UserSubscriptions
                        .Any(sub => sub.UserChannelId == video.UserId);
                }
            }

            return View(model);
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdateFeedback(string videoId, bool isLike)
        {
            if (videoId == null)
            {
                return NotFound();
            }

            var curUser = await _userManager.GetUserAsync(User);
            if (curUser == null)
            {
                return NotFound();
            }

            var feedback = await _db.VideoFeedbacks.FindAsync(videoId, curUser.Id);
            if (feedback == null)
            {
                await _db.AddAsync(new VideoFeedback
                {
                    VideoId = videoId,
                    UserId = curUser.Id,
                    IsLike = isLike,
                    CreatedAt = DateTime.UtcNow
                });
            }
            else
            {
                feedback.IsLike = isLike;
                _db.Update(feedback);
            }
            await _db.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DeleteFeedback(string videoId)
        {
            if (videoId == null)
            {
                return NotFound();
            }

            var curUser = await _userManager.GetUserAsync(User);
            if (curUser == null)
            {
                return NotFound();
            }

            var feedback = await _db.VideoFeedbacks.FindAsync(videoId, curUser.Id);
            if (feedback != null)
            {
                _db.Remove(feedback);
            }
            await _db.SaveChangesAsync();

            return Ok();
        }


        [HttpPost]
        public async Task<IActionResult> IncrementViewCount(string videoId)
        {
            if (videoId == null)
            {
                return NotFound();
            }

            var video = await _db.Videos.FindAsync(videoId);
            if (video == null)
            {
                return NotFound();
            }

            video.ViewCount++;
            _db.Update(video);
            await _db.SaveChangesAsync();

            if (User.Identity?.IsAuthenticated == true)
            {
                await UpdateWatchHistory(videoId);
            }

            return Ok();
        }

        private async Task UpdateWatchHistory(string videoId)
        {
            var curUser = await _userManager.GetUserAsync(User);
            if (curUser == null)
            {
                return;
            }

            var history = await _db.WatchHistories.FindAsync(curUser.Id, videoId);
            if (history == null)
            {
                await _db.AddAsync(new WatchHistory
                {
                    VideoId = videoId,
                    UserId = curUser.Id,
                    WatchDate = DateTime.UtcNow
                });
            }
            else
            {
                history.WatchDate = DateTime.UtcNow;
                _db.Update(history);
            }
            await _db.SaveChangesAsync();
        }
    }
}
