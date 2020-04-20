namespace ReleaseNotes.Config
{
    public class AuthTokenClientConfiguration
    {
        public string IdentityServerBaseUrl { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string[] Scopes { get; set; }
    }
}
