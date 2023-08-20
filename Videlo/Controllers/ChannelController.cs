using GamevaWeb.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Videlo.Data;
using Videlo.Data.Enums;
using Videlo.Models.Database;

namespace Videlo.Controllers
{
    public class ChannelController : Controller
    {
        private readonly VideloContext _db;
        private readonly UserManager<User> _userManager;

        public ChannelController(VideloContext db, UserManager<User> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task<IActionResult> Videos(string userId)
        {
            if (userId == null)
            {
                return NotFound();
            }

            var user = await _db.Users
                .Include(u => u.Videos)
                .Include(u => u.UserSubscribers)
                .FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                return NotFound();
            }

            var curUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (curUserId != null)
            {
                var curUser = await _db.Users
                    .Include(u => u.UserSubscriptions)
                    .FirstOrDefaultAsync(u => u.Id == curUserId);
                if (curUser != null)
                {
                    ViewBag.IsSubbed = curUser.UserSubscriptions
                        .Any(sub => sub.UserChannelId == userId);
                }
            }

            return View(user);
        }

        public async Task<IActionResult> GetVideos(string userId, ChannelVideosFilter filter, string viewName, int pageIndex, int pageSize = 24)
        {
            var curUser = await _userManager.GetUserAsync(User);

            IQueryable<Video> query = _db.Videos
                .Where(c => c.UserId == userId)
                .Include(c => c.User);

            if (filter == ChannelVideosFilter.Newest)
            {
                query = query
                    .OrderByDescending(v => v.CreatedAt);
            }
            else if (filter == ChannelVideosFilter.Oldest)
            {
                query = query
                    .OrderBy(v => v.CreatedAt);
            }
            else if (filter == ChannelVideosFilter.Popular)
            {
                query = query
                    .OrderByDescending(v => v.ViewCount)
                    .ThenByDescending(v => v.CreatedAt);
            }
            var model = await query
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return PartialView(viewName, model);
        }

        [Authorize(Roles = RoleConstants.User)]
        public async Task<IActionResult> Studio()
        {
            var curUser = await _userManager.GetUserAsync(User);
            if (curUser == null)
            {
                return NotFound();
            }

            return View(curUser);
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Subscribe(string channelId)
        {
            var curUser = await _userManager.GetUserAsync(User);
            if (curUser == null)
            {
                return NotFound();
            }

            var channel = await _userManager.FindByIdAsync(channelId);
            if (channel == null)
            {
                return NotFound();
            }

            await _db.UserSubscriptions
                .AddAsync(new UserSubscription()
                {
                    UserId = curUser.Id,
                    UserChannelId = channel.Id,
                    SubscriptionDate = DateTime.UtcNow
                });
            await _db.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Unsubscribe(string channelId)
        {
            var curUser = await _userManager.GetUserAsync(User);
            if (curUser == null)
            {
                return NotFound();
            }

            var channel = await _userManager.FindByIdAsync(channelId);
            if (channel == null)
            {
                return NotFound();
            }

            var subscription = await _db.UserSubscriptions.FindAsync(curUser.Id, channel.Id);
            if (subscription != null)
            {
                _db.UserSubscriptions.Remove(subscription);
            }
            await _db.SaveChangesAsync();

            return Ok();
        }
    }
}
