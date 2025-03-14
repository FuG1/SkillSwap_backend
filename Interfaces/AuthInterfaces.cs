namespace SkillSwap.Interfaces
{
    public class IRegisterData
    {
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public string? About { get; set; }
    }

    public class ILoginData
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }


}