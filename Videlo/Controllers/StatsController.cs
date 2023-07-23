using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Videlo.Data;
using Videlo.Models.Database;
using Videlo.Models.ViewModels;

namespace Videlo.Controllers
{
    public class StatsController : Controller
    {
        private readonly VideloContext _db;
        private readonly UserManager<User> _userManager;

        public StatsController(VideloContext db, UserManager<User> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        [Authorize]
        public async Task<IActionResult> History()
        {
            var curUser = await _userManager.GetUserAsync(User);
            if (curUser == null)
            {
                return NotFound();
            }

            var model = _db.WatchHistories
                .Where(f => f.UserId == curUser.Id)
                .Include(f => f.Video).ThenInclude(v => v.User)
                .OrderByDescending(f => f.WatchDate);

            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Playlist(bool likeFilter)
        {
            var curUser = await _userManager.GetUserAsync(User);
            if (curUser == null)
            {
                return NotFound();
            }

            var model = _db.VideoFeedbacks
                .Where(f => f.UserId == curUser.Id && f.IsLike == likeFilter)
                .Include(f => f.Video).ThenInclude(v => v.User)
                .OrderByDescending(f => f.CreatedAt);

            return View(new PlaylistViewModel(model, likeFilter));
        }
    }
}
