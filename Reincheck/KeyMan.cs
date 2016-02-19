using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace Reincheck
{
    /// <summary>
    /// Use the <see cref="KeyMan"/> to generate keys and to encode/decode claims to/from secure tokens
    /// </summary>
    public class KeyMan : IDisposable
    {
        const string PartSeparatorChar = "|";

        /// <summary>
        /// Generates a new key, which includes both public and private keys
        /// </summary>
        public static string GenerateKey()
        {
            return CryptoInitializer.GenerateNewKey();
        }

        readonly RSACryptoServiceProvider _cryptoServiceProvider;
        readonly string _hashAlgoOid = CryptoConfig.MapNameToOID("SHA1");

        public KeyMan(string xmlEncodedRsaKeysIncludingThePrivateParts)
        {
            _cryptoServiceProvider = CryptoInitializer.GetFromKey(xmlEncodedRsaKeysIncludingThePrivateParts);
        }

        public void Dispose()
        {
            _cryptoServiceProvider.Dispose();
        }

        public string Encode(IDictionary<string, string> properties)
        {
            var jsonText = JsonConvert.SerializeObject(properties);
            var firstPart = Convert.ToBase64String(Encoding.UTF8.GetBytes(jsonText));

            return firstPart 
                + PartSeparatorChar 
                + GenerateSignature(firstPart);
        }

        public DecodingResult Decode(string token)
        {
            try
            {
                var parts = token.Split(new[] {PartSeparatorChar}, StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length != 2)
                {
                    throw new CryptographicException("Token did not contain the expected number of parts!");
                }

                var properties = Deserialize(parts[0]);
                var isValid = CanValidateSignature(parts[0], parts[1]);

                return new DecodingResult(token, properties, isValid);
            }
            catch (Exception exception)
            {
                throw new CryptographicException("Could not decode token", exception);
            }
        }

        static Dictionary<string, string> Deserialize(string tokenPart)
        {
            try
            {
                var jsonBytes = Convert.FromBase64String(tokenPart);
                var jsonText = Encoding.UTF8.GetString(jsonBytes);
                return JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonText);
            }
            catch
            {
                throw new CryptographicException("Could not deserialize properties");
            }
        }

        string GenerateSignature(string jsonText)
        {
            using (var hashish = SHA1.Create())
            {
                var jsonBytes = Encoding.UTF8.GetBytes(jsonText);
                using (var inputStream = new MemoryStream(jsonBytes))
                {
                    var hashBytes = hashish.ComputeHash(inputStream);
                    var digest = _cryptoServiceProvider.SignHash(hashBytes, _hashAlgoOid);
                    var signature = Convert.ToBase64String(digest);
                    return signature;
                }
            }
        }

        bool CanValidateSignature(string jsonText, string signature)
        {
            var generatedSignature = GenerateSignature(jsonText);

            return generatedSignature == signature;
        }
    }
}
