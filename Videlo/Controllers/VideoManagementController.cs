using GamevaWeb.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Videlo.Data;
using Videlo.Models.Database;
using Videlo.Models.ViewModels;
using Videlo.Models;
using Videlo.Services;
using Videlo.Utilities;
using Microsoft.Extensions.Options;
using Videlo.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using Videlo.Data.Enums;
using Microsoft.EntityFrameworkCore;
using Videlo.Services.Interfaces;

namespace Videlo.Controllers
{
    [Authorize(Roles = RoleConstants.User)]
    public class VideoManagementController : Controller
    {
        private readonly VideloContext _db;
        private readonly UserManager<User> _userManager;
        private readonly IStorageService _s3Service;
        private readonly IOptions<AWSConfiguration> _awsOptions;
        private readonly IUploadTaskRepository _taskRepository;

        public VideoManagementController(VideloContext db, UserManager<User> userManager, IStorageService s3Service, IOptions<AWSConfiguration> awsOptions, IUploadTaskRepository taskRepository)
        {
            _db = db;
            _userManager = userManager;
            _s3Service = s3Service;
            _awsOptions = awsOptions;
            _taskRepository = taskRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Upload()
        {
            var curUser = await _userManager.GetUserAsync(User);

            if (curUser != null)
            {
                var status = _taskRepository.GetUploadStatus(curUser.Id);

                if (status != UploadStatus.InProgress)
                {
                    return View();
                }

                return View("_UploadProgress");
            }

            return Forbid();
        }

        [HttpPost]
        public async Task<IActionResult> Upload(VideoUploadViewModel model)
        {
            var curUser = await _userManager.GetUserAsync(User);

            if (ModelState.IsValid && curUser != null)
            {
                var status = _taskRepository.GetUploadStatus(curUser.Id);

                if (status != UploadStatus.InProgress)
                {
                    Task task = CreateVideoAsync(model, curUser.Id);
                    if (_taskRepository.TryAdd(curUser.Id, task))
                    {
                        await task;
                        return Ok();
                    }
                }

                return StatusCode(503);
            }

            return BadRequest();
        }

        private async Task CreateVideoAsync(VideoUploadViewModel model, string userId)
        {
            try
            {
                var videoStream = new MemoryStream();
                await model.VideoFile.CopyToAsync(videoStream);

                var videoInfo = new FormFileInfo(
                    videoStream,
                    model.VideoFile.FileName,
                    model.VideoFile.ContentType);
                var videoPath = await _s3Service.UploadVideoAsync(videoInfo);

                if (!videoPath.IsNullOrEmpty())
                {
                    FormFileInfo imgInfo;
                    if (model.ThumbnailImage == null)
                    {
                        imgInfo = FFMpeg.GetVideoThumbnail($"{_awsOptions.Value.BaseURL}{videoPath}");
                    }
                    else
                    {
                        var imgStream = new MemoryStream();
                        await model.ThumbnailImage.CopyToAsync(imgStream);

                        imgInfo = new FormFileInfo(
                            imgStream,
                            model.ThumbnailImage.FileName,
                            model.ThumbnailImage.ContentType);
                    }
                    var imgPath = await _s3Service.UploadVideoThumbnailAsync(imgInfo);
                    imgInfo.Dispose();

                    Video video = new()
                    {
                        Title = model.Title,
                        Description = model.Description,
                        CreatedAt = DateTime.UtcNow,
                        VideoPath = videoPath,
                        ThumbnailPath = imgPath,
                        UserId = userId
                    };

                    await _db.AddAsync(video);
                    await _db.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
        }

        [HttpGet]
        public IActionResult UploadProgress()
        {
            return PartialView("_UploadProgress");
        }


        [HttpGet]
        public async Task<IActionResult> Edit(string videoId)
        {
            if (videoId == null || _db.Videos == null)
            {
                return NotFound();
            }

            var video = await _db.Videos.FindAsync(videoId);
            if (video == null)
            {
                return NotFound();
            }

            return View(new VideoEditViewModel(video.Id, video.Title, video.Description));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(VideoEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var video = await _db.Videos.FindAsync(model.Id);

                if (video == null)
                {
                    return NotFound();
                }

                video.Title = model.Title;
                video.Description = model.Description;

                _db.Update(video);
                await _db.SaveChangesAsync();

                return RedirectToAction("Studio", "Channel");
            }
            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Delete(string videoId)
        {
            if (videoId == null || _db.Videos == null)
            {
                return NotFound();
            }

            var video = await _db.Videos
                .FirstOrDefaultAsync(m => m.Id == videoId);
            if (video == null)
            {
                return NotFound();
            }

            return View(video);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_db.Videos == null)
            {
                return Problem("Entity set 'VideloContext.Videos'  is null.");
            }
            var video = _db.Videos
                .Include(v => v.VideoFeedbacks)
                .Include(v => v.VideoComments).ThenInclude(c => c.VideoCommentFeedbacks)
                .Include(v => v.WatchHistories)
                .FirstOrDefault(v => v.Id == id);
            if (video != null)
            {
                await _s3Service.DeleteFileAsync(video.ThumbnailPath);
                await _s3Service.DeleteFileAsync(video.VideoPath);
                _db.Videos.Remove(video);
            }

            await _db.SaveChangesAsync();
            return RedirectToAction("Studio", "Channel");
        }
    }
}
