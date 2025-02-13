namespace Golub.Settings
{
    /// <summary>
    /// Represents configuration settings for security.
    /// Contains PrivateKey property used for decryption of API Key.
    /// </summary>
    public class SecuritySettings
    {
        public string PrivateKey { get; set; } = null!;
    }
}
