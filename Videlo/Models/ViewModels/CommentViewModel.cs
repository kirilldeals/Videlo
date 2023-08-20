using Videlo.Models.Database;

namespace Videlo.Models.ViewModels
{
    public class CommentViewModel
    {
        public CommentViewModel(VideoComment comment, bool canUserEdit) 
        { 
            Comment = comment;
            CanUserEdit = canUserEdit;
        }

        public VideoComment Comment { get; set; }

        public bool CanUserEdit { get; set; }
    }
}
