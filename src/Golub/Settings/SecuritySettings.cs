namespace Golub.Settings
{
    /// <summary>
    /// Represents configuration settings for security.
    /// Contains PrivateKey property used for decryption of API Key
    /// and PublicKey property used for encryption of API Key.
    /// </summary>
    public class SecuritySettings
    {
        /// <summary>
        /// Public key used for encryption of API Key on the client side
        /// </summary>
        public string PublicKey { get; set; } = null!;

        /// <summary>
        /// Private key used for decryption of API Key on the server side
        /// </summary>
        public string PrivateKey { get; set; } = null!;
    }
}
