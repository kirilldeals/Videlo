using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Videlo.Data;
using Videlo.Models.Database;

namespace Videlo.Controllers
{
    public class CommentController : Controller
    {
        private readonly VideloContext _db;
        private readonly UserManager<User> _userManager;

        public CommentController(VideloContext db, UserManager<User> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task<IActionResult> VideoComments(string videoId, bool byPopular)
        {
            var curUser = await _userManager.GetUserAsync(User);

            var model = _db.VideoComments
                .Where(c => c.VideoId == videoId)
                .Include(c => c.User)
                .Include(c => c.VideoCommentFeedbacks)
                .OrderByDescending(c => curUser != null && c.UserId == curUser.Id);
            if (byPopular)
            {
                model = model
                    .ThenByDescending(c => c.VideoCommentFeedbacks.Count(f => f.IsLike))
                    .ThenByDescending(c => c.VideoCommentFeedbacks.Count(f => !f.IsLike));
            }
            model = model
                .ThenByDescending(c => c.CreatedAt);

            return PartialView("_VideoComments", model);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string videoId, string comment)
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

            if (comment.IsNullOrEmpty())
            {
                ModelState.AddModelError("comment", "Comment is empty");
                return RedirectToAction("Watch", "Video", new { videoId = video.Id });
            }

            var curUser = await _userManager.GetUserAsync(User);
            if (curUser == null)
            {
                return NotFound();
            }

            await _db.VideoComments.AddAsync(new VideoComment()
            {
                Content = comment,
                CreatedAt = DateTime.UtcNow,

                VideoId = video.Id,
                UserId = curUser.Id
            });
            await _db.SaveChangesAsync();

            return RedirectToAction("Watch", "Video", new { videoId = video.Id });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdateFeedback(string commentId, bool isLike)
        {
            if (commentId == null)
            {
                return NotFound();
            }

            var curUser = await _userManager.GetUserAsync(User);
            if (curUser == null)
            {
                return NotFound();
            }

            var feedback = await _db.VideoCommentFeedbacks.FindAsync(commentId, curUser.Id);
            if (feedback == null)
            {
                await _db.AddAsync(new VideoCommentFeedback
                {
                    CommentId = commentId,
                    UserId = curUser.Id,
                    IsLike = isLike
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
        public async Task<IActionResult> DeleteFeedback(string commentId)
        {
            if (commentId == null)
            {
                return NotFound();
            }

            var curUser = await _userManager.GetUserAsync(User);
            if (curUser == null)
            {
                return NotFound();
            }

            var feedback = await _db.VideoCommentFeedbacks.FindAsync(commentId, curUser.Id);
            if (feedback != null)
            {
                _db.Remove(feedback);
            }
            await _db.SaveChangesAsync();

            return Ok();
        }
    }
}
