
namespace WebApplication1.Models.User
{
    public class SignUpViewModel
    {
        public string? LoginMessage { get; set; }
        public string? PasswordMessage { get; set; }
        public string? RepeatMessage { get; set; }
        public string? RealNameMessage { get; set; }
        public string? EmailMessage { get; set; }
        public string? AvatarMessage { get; set; }
        public string? ConfirmMessage { get; set; }
        public string SuccessMessage { get; set; }
        public SignUpFormModel? FormModel { get; set; }
    }
}
