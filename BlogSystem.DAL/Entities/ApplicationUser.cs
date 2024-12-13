using Microsoft.AspNetCore.Identity;

namespace BlogSystem.DAL.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string? Token { get; set; }
        public string? PictureUrl { get; set; }
        public string? PicturePublicId { get; set; }
        public bool IsBlocked { get; set; }
        public DateTime? BlockPeriod { get; set; }
    }
}
