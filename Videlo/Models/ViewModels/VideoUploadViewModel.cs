using System.ComponentModel.DataAnnotations;

namespace Videlo.Models.ViewModels
{
    public class VideoUploadViewModel
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = null!;

        [MaxLength(5000)]
        public string? Description { get; set; }

        [Required]
        [Display(Name = "Video file")]
        public IFormFile VideoFile { get; set; } = null!;

        [Display(Name = "Thumbnail image")]
        public IFormFile? ThumbnailImage { get; set; }
    }
}
