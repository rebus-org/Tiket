using System;
using System.Collections.Generic;
using NUnit.Framework;
using Testy;

namespace Tiket.Tests.Areas
{
    [TestFixture]
    public class Expiration : FixtureBase
    {
        KeyMan _keyMan;

        protected override void SetUp()
        {
            _keyMan = new KeyMan(TestConfig.ValidKey);

            Using(_keyMan);
        }

        [Test]
        public void CanDetermineWhetherTokenIsExpired()
        {
            var expirationTimeInThePast = DateTimeOffset.Now.AddMinutes(-2).ToString("O");

            var properties = new Dictionary<string, string>
            {
                {KeyMan.Properties.ExpirationTime, expirationTimeInThePast}
            };

            var token = _keyMan.Encode(properties);

            var result = _keyMan.Decode(token);

            Assert.That(result.IsValid, Is.False);
            Assert.That(result.Details.HasValidSignature, Is.True);
            Assert.That(result.Details.IsExpired, Is.True);
        }

        [Test]
        public void CanDetermineWhetherTokenHasNotBecomeValidYet()
        {
            var validTimeInTheFuture = DateTimeOffset.Now.AddMinutes(2).ToString("O");

            var properties = new Dictionary<string, string>
            {
                {KeyMan.Properties.NotBefore, validTimeInTheFuture}
            };

            var token = _keyMan.Encode(properties);

            var result = _keyMan.Decode(token);

            Assert.That(result.IsValid, Is.False);
            Assert.That(result.Details.HasValidSignature, Is.True);
            Assert.That(result.Details.IsExpired, Is.False);
            Assert.That(result.Details.IsNotValidYet, Is.True);
        }
    }
}