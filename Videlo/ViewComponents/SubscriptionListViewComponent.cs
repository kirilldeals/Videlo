using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Videlo.Data;
using Videlo.Models.Database;

namespace Videlo.Components
{
    public class SubscriptionListViewComponent : ViewComponent
    {
        private readonly VideloContext _db;
        private readonly UserManager<User> _userManager;

        public SubscriptionListViewComponent(VideloContext db, UserManager<User> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var curUser = await _userManager.GetUserAsync(HttpContext.User);
            if (curUser == null)
            {
                return View("_SubscriptionList");
            }

            var model = _db.UserSubscriptions
                .Include(s => s.UserChannel)
                .Where(u => u.UserId == curUser.Id)
                .OrderBy(s => s.UserChannel.UserName);

            return View("_SubscriptionList", model);
        }
    }
}
