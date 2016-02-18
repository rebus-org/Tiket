using System;
using System.Security.Cryptography;

namespace Reincheck
{
    class CryptoInitializer
    {
        public static RSACryptoServiceProvider GetFromKey(string keyXml)
        {
            if (string.IsNullOrWhiteSpace(keyXml))
            {
                throw NewCryptographicHelpException(null, @"KeyMan created with an empty key as argument.");
            }

            try
            {
                var provider = NewProvider();

                provider.FromXmlString(keyXml);

                return provider;
            }
            catch (Exception e)
            {
                throw NewCryptographicHelpException(e, @"KeyMan created with an invalid key as argument - you need to specify a key in order to use the KeyMan!");
            }
        }

        static CryptographicException NewCryptographicHelpException(Exception e, string problemDescription)
        {
            var suggestedKeyXml = GenerateNewKeyXml();

            return new CryptographicException(problemDescription + $@"

If you are not sure which key to use, you can have this new and fresh one:

{suggestedKeyXml}

To use the key, just copy/paste the full contents of the line above and start using it.

Please ensure that the key IS NOT SHARED WITH ANYONE!", e);
        }

        static string GenerateNewKeyXml()
        {
            using (var provider = NewProvider())
            {
                var xmlString = provider.ToXmlString(true);

                return xmlString;
            }
        }

        static RSACryptoServiceProvider NewProvider()
        {
            return new RSACryptoServiceProvider(2048)
            {
                PersistKeyInCsp = false
            };
        }
    }
}