namespace WebApplication1.Data.Entities
{
    public class User
    {
        public Guid      Id           { get; set; }
        public String    Name         { get; set; } = null!;
        public String    Email        { get; set; } = null!;
        public String?   ConfirmCode  { get; set; }
        public String    Login        { get; set; } = null!;
        public String    PasswordHash { get; set; } = null!;
        public String    Avatar       { get; set; } = null!;
        public DateTime  CreatedDt    { get; set; }
        public DateTime? DeletedDt    { get; set; }
    }
}
