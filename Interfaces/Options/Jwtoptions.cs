namespace SkillSwap.Interfaces.Options
{
    public class IJwtOptions
    {
        public required string SecurityKey { get; set; }
        public required string EncryptionAlgorithm { get; set; }
        public required double Expiration { get; set; }
        public class IFieldsOptions
        {
            public required string UserId { get; set; }
            public required string Expiration { get; set; }
        }
        public required IFieldsOptions Fields { get; set; }
    }
}