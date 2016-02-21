using System;
using System.Linq;
using System.Security.Cryptography;
using NUnit.Framework;

namespace Tiket.Tests.Assumptions
{
    [TestFixture]
    public class EncryptionCheck
    {
        [Test]
        public void CheckAES()
        {
            var cryptoServiceProvider = new AesCryptoServiceProvider();

            Console.WriteLine(string.Join(Environment.NewLine, cryptoServiceProvider.LegalKeySizes.Select(k => $"{k.MinSize} - {k.MaxSize} / {k.SkipSize}")));
        }
    }
}