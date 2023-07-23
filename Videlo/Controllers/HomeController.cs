using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Videlo.Data;
using Videlo.Models.Database;
using Videlo.Models.ViewModels;

namespace Videlo.Controllers
{
    public class HomeController : Controller
    {
        private readonly VideloContext _db;
        private readonly UserManager<User> _userManager;

        public HomeController(VideloContext db, UserManager<User> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public IActionResult Index(string searchQuery)
        {
            IQueryable<Models.Database.Video> model = _db.Videos;
            if (!string.IsNullOrEmpty(searchQuery))
            {
                model = model.Where(s => s.Title.Contains(searchQuery));
            }
            model = model
                .Include(v => v.User)
                .OrderByDescending(v => v.CreatedAt);
            ViewBag.SearchQuery = searchQuery;

            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Subscriptions()
        {
            var curUser = await _userManager.GetUserAsync(User);
            if (curUser == null)
            {
                return NotFound();
            }

            var model = _db.UserSubscriptions
                .Where(us => us.UserId == curUser.Id)
                .Include(us => us.UserChannel)
                .ThenInclude(uc => uc.Videos)
                .ThenInclude(v => v.User)
                .SelectMany(us => us.UserChannel.Videos)
                .OrderByDescending(v => v.CreatedAt);

            return View(model);
        }

        [HttpGet]
        public IActionResult Search(string query)
        {
            var searchResults = _db.Videos
                .Where(e => e.Title.Contains(query))
                .OrderByDescending(v => v.ViewCount)
                .Select(e => e.Title)
                .Take(10);

            return Json(searchResults);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}