using System;
using NUnit.Framework;

namespace Tiket.Tests.Areas
{
    [TestFixture]
    public class MasterKeyGeneration
    {
        [Test]
        public void DoesNotGenerateSameKeyTwice()
        {
            var firstKey = KeyMan.GenerateKey();
            var secondKey = KeyMan.GenerateKey();

            Console.WriteLine(firstKey);
            Console.WriteLine();
            Console.WriteLine(secondKey);

            Assert.That(firstKey, Is.Not.EqualTo(secondKey));
        }
    }
}