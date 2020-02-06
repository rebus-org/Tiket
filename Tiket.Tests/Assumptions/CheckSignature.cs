using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using NUnit.Framework;
using Testy;

namespace Tiket.Tests.Assumptions
{
    [TestFixture]
    public class CheckSignature : FixtureBase
    {
        const string masterKey = "ufyqTR9KgORFYxAQP0tpB4b/cnJFu4GmzNEKb4Kh9k8=|MPdDrRHAerQVc1BTV7xCkg==";

        [Test]
        public void PrintHash()
        {
            using (var hashish = SHA1.Create())
            {
                var bytes = Encoding.UTF8.GetBytes("constant-text");
                using (var inputStream = new MemoryStream(bytes))
                {
                    var hashBytes = hashish.ComputeHash(inputStream);
                    var digest = Convert.ToBase64String(hashBytes);
                    Console.WriteLine(digest);
                    Assert.That(digest, Is.EqualTo("A/0Cj5e7SQWCxSbj7ZPQiLZ2bAM="));
                }
            }
        }

        [Test]
        public void PrintEncryptedHash()
        {
            using (var hashish = SHA1.Create())
            {
                var bytes = Encoding.UTF8.GetBytes("constant-text");
                using (var inputStream = new MemoryStream(bytes))
                {
                    var hashBytes = hashish.ComputeHash(inputStream);
                    var digest = Convert.ToBase64String(hashBytes);
                    Console.WriteLine(digest);
                    Assert.That(digest, Is.EqualTo("A/0Cj5e7SQWCxSbj7ZPQiLZ2bAM="));
                }
            }
        }


    }
}