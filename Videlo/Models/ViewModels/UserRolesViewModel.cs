using Microsoft.AspNetCore.Identity;

namespace Videlo.Models.ViewModels
{
    public class UserRolesViewModel
    {
        public string UserId { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? UserName { get; set; }
        public IList<IdentityRole> AllRoles { get; set; } = new List<IdentityRole>();
        public IList<string> UserRoles { get; set; } = new List<string>();
    }
}
