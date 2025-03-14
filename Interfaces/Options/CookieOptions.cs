namespace SkillSwap.Interfaces.Options
{
    public class ICookieOptions
    {
        public required string JwtToken { get; set; }
        public required string Domain { get; set; }
        public required double Expiration { get; set; }
    }
}