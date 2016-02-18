using System;
using System.Security.Cryptography;

namespace Reincheck
{
    public class KeyMan : IDisposable
    {
        readonly RSACryptoServiceProvider _cryptoServiceProvider;

        public KeyMan(string xmlEncodedRsaKeysIncludingThePrivateParts)
        {
            _cryptoServiceProvider = CryptoInitializer.GetFromKey(xmlEncodedRsaKeysIncludingThePrivateParts);
        }

        public void Dispose()
        {
            _cryptoServiceProvider.Dispose();
        }
    }
}
