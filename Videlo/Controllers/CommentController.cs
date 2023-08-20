using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Videlo.Data;
using Videlo.Models.Database;
using Videlo.Models.ViewModels;
using Videlo.Services;

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

        public async Task<IActionResult> VideoComments(string videoId, bool byPopular, int pageIndex, int pageSize = 10)
        {
            var curUser = await _userManager.GetUserAsync(User);

            var query = _db.VideoComments
                .Where(c => c.VideoId == videoId)
                .Include(c => c.User)
                .Include(c => c.VideoCommentFeedbacks)
                .OrderByDescending(c => curUser != null && c.UserId == curUser.Id);
            if (byPopular)
            {
                query = query
                    .ThenByDescending(c => c.VideoCommentFeedbacks.Count(f => f.IsLike))
                    .ThenByDescending(c => c.VideoCommentFeedbacks.Count(f => !f.IsLike));
            }
            query = query
                .ThenByDescending(c => c.CreatedAt);

            var model = await query
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .Select(c => new CommentViewModel(c, curUser != null && c.UserId == curUser.Id))
                .ToListAsync();

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
        public async Task<IActionResult> Delete(string commentId)
        {
            var comment = _db.VideoComments
                .Include(c => c.VideoCommentFeedbacks)
                .FirstOrDefault(c => c.Id == commentId);
            if (comment != null)
            {
                _db.VideoComments.Remove(comment);
            }

            await _db.SaveChangesAsync();

            return Ok();
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
