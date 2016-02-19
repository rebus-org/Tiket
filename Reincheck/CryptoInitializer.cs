using System;
using System.Security.Cryptography;
using System.Text;

namespace Reincheck
{
    class CryptoInitializer
    {
        const int KeySize = 2048;

        public static RSACryptoServiceProvider GetFromKey(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw NewCryptographicHelpException(null, @"KeyMan created with an empty key as argument.");
            }

            try
            {
                var provider = NewProvider();
                var keyBytes = Convert.FromBase64String(key);
                var xml = Encoding.UTF8.GetString(keyBytes);

                provider.FromXmlString(xml);

                return provider;
            }
            catch (Exception e)
            {
                throw NewCryptographicHelpException(e, @"KeyMan created with an invalid key as argument - you need to specify a key in order to use the KeyMan!");
            }
        }

        static CryptographicException NewCryptographicHelpException(Exception e, string problemDescription)
        {
            var suggestedKey = GenerateNewKey();

            return new CryptographicException(problemDescription + $@"

If you are not sure which key to use, you can have this new and fresh one:

{suggestedKey}

To use the key, just copy/paste the full contents of the line above and start using it.

Please ensure that the key IS NOT SHARED WITH ANYONE!", e);
        }

        public static string GenerateNewKey()
        {
            using (var provider = NewProvider())
            {
                var xmlString = provider.ToXmlString(true);
                var keyBytes = Encoding.UTF8.GetBytes(xmlString);
                var key = Convert.ToBase64String(keyBytes);
                return key;
            }
        }

        const bool FuckNoWhyWouldYouDoThat = false;

        static RSACryptoServiceProvider NewProvider()
        {
            return new RSACryptoServiceProvider(KeySize)
            {
                PersistKeyInCsp = FuckNoWhyWouldYouDoThat
            };
        }
    }
}