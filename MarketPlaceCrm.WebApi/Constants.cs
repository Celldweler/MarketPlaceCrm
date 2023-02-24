namespace MarketPlaceCrm.WebApi
{
    public class Constants
    {
        public const string CorsPolicy = "CorsPolicy";
        public const string AdminPolicy = nameof(AdminPolicy);
        public const string ModeratorPolicy = nameof(ModeratorPolicy);
        public const string AdminRole = "admin";
        public const string ModeratorClaim = "moderator";
        public bool AutoApproved { get; set; } = true;
    }
}