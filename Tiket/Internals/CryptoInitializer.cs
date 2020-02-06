using System;
using System.Security.Cryptography;

namespace Tiket.Internals
{
    class CryptoInitializer
    {
        public static Aes GetFromKey(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw NewCryptographicHelpException(null, @"KeyMan created with an empty key as argument.");
            }

            try
            {
                var provider = NewProvider();
                var parts = key.Split('|');

                provider.Key = Convert.FromBase64String(parts[0]);
                provider.IV = Convert.FromBase64String(parts[1]);

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
                provider.GenerateIV();
                provider.GenerateKey();

                var key = Convert.ToBase64String(provider.Key);
                var iv = Convert.ToBase64String(provider.IV);

                return $"{key}|{iv}";
            }
        }

        static AesCryptoServiceProvider NewProvider()
        {
            return new AesCryptoServiceProvider { KeySize = 256 };
        }
    }
}