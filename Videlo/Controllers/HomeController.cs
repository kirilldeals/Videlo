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
            ViewBag.SearchQuery = searchQuery;
            return View();
        }

        public async Task<IActionResult> IndexVideos(string searchQuery, int pageIndex = 0, int pageSize = 24)
        {
            IQueryable<Video> model = _db.Videos;
            if (!string.IsNullOrEmpty(searchQuery))
            {
                model = model.Where(s => s.Title.Contains(searchQuery));
            }
            model = model
                .Include(v => v.User)
                .OrderByDescending(v => v.CreatedAt)
                .Skip(pageIndex * pageSize)
                .Take(pageSize);

            return PartialView("_VideoCardPage", await model.ToListAsync());
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

        [Authorize]
        public IActionResult Subscriptions()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> SubscriptionVideos(int pageIndex = 0, int pageSize = 24)
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
                .OrderByDescending(v => v.CreatedAt)
                .Skip(pageIndex * pageSize)
                .Take(pageSize);

            return PartialView("_VideoCardPage", await model.ToListAsync());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}