using System;
using System.Collections;
using NUnit.Framework;
using Testy;

namespace Tiket.Tests.Areas
{
    [TestFixture]
    public class DirtyInput : FixtureBase
    {
        [TestCaseSource(nameof(GetDirtyInput))]
        public void DoesNotDieOnInvalidClientKey(string dirtyInput)
        {
            var keyMan = Using(new KeyMan(KeyMan.GenerateKey()));

            var result = keyMan.Decode(dirtyInput);

            Assert.That(result.IsValid, Is.False);
            Assert.That(result.Details.HasValidSignature, Is.False);
            Assert.That(result.Details.IsCorrectFormat, Is.False);
            Assert.That(result.Details.IsExpired, Is.False);
            Assert.That(result.Details.IsNotValidYet, Is.False);
        }

        static IEnumerable GetDirtyInput()
        {
            yield return "this is definitely not a valid key";

            var random = new Random(DateTime.Now.GetHashCode());
            var iterations = random.Next(15);

            for (var counter = 0; counter < iterations; counter++)
            {
                var buffer = new byte[random.Next(1024)];

                random.NextBytes(buffer);

                var base64String = Convert.ToBase64String(buffer);

                yield return base64String;
            }

            yield return "this 15 4rb17r4ry";
        }
    }
}