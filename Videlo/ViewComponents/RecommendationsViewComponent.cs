using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Videlo.Data;
using Videlo.Models.Database;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Videlo.Components
{
    public class RecommendationsViewComponent : ViewComponent
    {
        private readonly VideloContext _db;
        private readonly UserManager<User> _userManager;

        public RecommendationsViewComponent(VideloContext db, UserManager<User> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(string videoId, string channelId, int pageIndex, int pageSize = 10)
        {
            var model = _db.Videos
                .Where(v => v.UserId == channelId)
                .Include(v => v.User)
                .Select(v => v);

            var curUser = await _userManager.GetUserAsync(HttpContext.User);
            if (curUser != null)
            {
                var curUserSubscriptionVideos = _db.UserSubscriptions
                    .Where(s => s.UserId == curUser.Id)
                    .Include(s => s.UserChannel).ThenInclude(u => u.Videos)
                    .SelectMany(s => s.UserChannel.Videos)
                    .Include(v => v.User);

                model = model
                    .Union(curUserSubscriptionVideos);
            }

            model = model
                .Where(v => v.Id != videoId)
                .OrderByDescending(v => v.CreatedAt)
                .Skip(pageIndex * pageSize)
                .Take(pageSize);

            return View("_Recommendations", await model.ToListAsync());
        }
    }
}
