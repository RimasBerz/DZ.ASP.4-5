using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Models.User
{
    public class SignUpFormModel
    {
        [FromForm(Name = "user-login")]
        public string Login { get; set; } = null!; 

        [FromForm(Name = "user-password")]
        public string Password { get; set; } = null!;

        [FromForm(Name = "user-repeat")]
        public string RepeatPassword { get; set; } = null!;

        [FromForm(Name = "user-name")]
        public string? RealName { get; set; } = null!;

        [FromForm(Name = "user-email")]
        public string? Email { get; set; } = null!;

        [FromForm(Name = "user-avatar")]
        public IFormFile Avatar { get; set; } = null!; // Для аватарки

        [FromForm(Name = "user-confirm")]
        public bool IsAgree { get; set; }

    }
}
