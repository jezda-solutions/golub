using Golub.Settings;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;

namespace Golub.Services.ApiKeyServices
{
    /// <summary>
    /// API key service
    /// Responsible for decrypting and returning the API key
    /// </summary>
    /// <param name="securityOptions"></param>
    public class ApiKeyService(IOptions<SecuritySettings> securityOptions)
    {
        private readonly SecuritySettings _securitySettings = securityOptions.Value;

        /// <summary>
        /// Decryption of the API key with valid private key
        /// </summary>
        /// <param name="encryptedApiKey"></param>
        /// <returns></returns>
        public string DecryptApiKey(string encryptedApiKey)
        {
            using var rsa = RSA.Create();
            rsa.ImportFromPem(_securitySettings.PrivateKey);

            var encryptedData = Convert.FromBase64String(encryptedApiKey);
            var decryptedData = rsa.Decrypt(encryptedData, RSAEncryptionPadding.OaepSHA256);

            int clientIdLength = decryptedData[0];

            var clientIdBytes = new byte[clientIdLength];
            Buffer.BlockCopy(decryptedData, 1, clientIdBytes, 0, clientIdLength);

            return Encoding.UTF8.GetString(clientIdBytes);
        }
    }
}