using System;
using System.Security.Cryptography;
using NUnit.Framework;
using Testy;

namespace Tiket.Tests.Assumptions
{
    [TestFixture]
    public class CheckAesHash : FixtureBase
    {
        const string masterKey = "ufyqTR9KgORFYxAQP0tpB4b/cnJFu4GmzNEKb4Kh9k8=|MPdDrRHAerQVc1BTV7xCkg==";

        [Test]
        public void GenerateHash()
        {
            var parts = masterKey.Split('|');

            var aes = Using(Aes.Create());

            aes.Key = Convert.FromBase64String(parts[0]);
            aes.IV = Convert.FromBase64String(parts[1]);


        }

        [Test]
        public void GenerateKey()
        {
            var aes = Using(Aes.Create());

            aes.KeySize = 256;

            aes.GenerateIV();
            aes.GenerateKey();

            var key = Convert.ToBase64String(aes.Key);
            var iv = Convert.ToBase64String(aes.IV);

            var finalKey = $"{key}|{iv}";

            Console.WriteLine(finalKey);

        }
    }
}