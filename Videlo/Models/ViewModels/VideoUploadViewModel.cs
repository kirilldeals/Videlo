using System.ComponentModel.DataAnnotations;

namespace Videlo.Models.ViewModels
{
    public class VideoUploadViewModel
    {
        [Required]
        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        [Required]
        [Display(Name = "Video file")]
        public IFormFile VideoFile { get; set; } = null!;

        [Display(Name = "Thumbnail image")]
        public IFormFile? ThumbnailImage { get; set; }
    }
}
