using EllipticCurve;
using Golub.Settings;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;

namespace Golub.Services.ApiKeyServices
{
    public class ApiKeyService(IOptions<SecuritySettings> securityOptions)
    {
        private readonly SecuritySettings _securitySettings = securityOptions.Value;

        /// <summary>
        /// Generating an encrypted API key with a valid public key
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public string GenerateEncryptedApiKey(string clientId)
        {
            using var rsa = RSA.Create();
            rsa.ImportFromPem(_securitySettings.PublicKey);

            var randomBytes = new byte[16];
            RandomNumberGenerator.Fill(randomBytes);

            var clientIdBytes = Encoding.UTF8.GetBytes(clientId);
            var data = new byte[clientIdBytes.Length + randomBytes.Length + 1];
            data[0] = (byte)clientIdBytes.Length;
            Buffer.BlockCopy(clientIdBytes, 0, data, 1, clientIdBytes.Length);
            Buffer.BlockCopy(randomBytes, 0, data, 1 + clientIdBytes.Length, randomBytes.Length);

            var encryptedData = rsa.Encrypt(data, RSAEncryptionPadding.OaepSHA256);
            return Convert.ToBase64String(encryptedData);
        }


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