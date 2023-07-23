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
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
    }
}
