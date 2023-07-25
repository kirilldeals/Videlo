using System.ComponentModel.DataAnnotations;

namespace Videlo.Models.ViewModels
{
    public class VideoEditViewModel
    {
        public VideoEditViewModel()
        {
        }

        public VideoEditViewModel(string id, string title, string? description)
        {
            Id = id;
            Title = title;
            Description = description;
        }

        public string Id { get; set; } = null!;

        [MaxLength(100)]
        public string Title { get; set; } = null!;

        [MaxLength(5000)]
        public string? Description { get; set; }
    }
}
