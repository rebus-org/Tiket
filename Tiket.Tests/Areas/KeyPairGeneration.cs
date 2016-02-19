using System;
using System.Security.Cryptography;
using NUnit.Framework;

namespace Tiket.Tests.Areas
{
    [TestFixture]
    public class KeyPairGeneration : FixtureBase
    {
        [Test]
        public void AcceptsValidKey()
        {
            var keyMan = Using(new KeyMan(ValidKey));
        }

        [Test]
        public void SuggestsNewKeyWhenCreatingWithNull()
        {
            var cryptographicException = Assert.Throws<CryptographicException>(() =>
            {
                var keyMan = Using(new KeyMan(null));
            });

            Console.WriteLine(cryptographicException);

            Assert.That(cryptographicException.Message, Contains.Substring("you can have this new and fresh one"));
        }

        [Test]
        public void SuggestsNewKeyWhenCreatingWithInvalidKey()
        {
            var cryptographicException = Assert.Throws<CryptographicException>(() =>
            {
                var keyMan = Using(new KeyMan("this is not a valid key"));
            });

            Console.WriteLine(cryptographicException);

            Assert.That(cryptographicException.Message, Contains.Substring("you can have this new and fresh one"));
        }
    }
}