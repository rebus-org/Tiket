using System;
using System.Collections.Generic;
using NUnit.Framework;
using Testy;

namespace Tiket.Tests.Areas
{
    [TestFixture]
    public class AutoProperties : FixtureBase
    {
        KeyMan _keyMan;

        protected override void SetUp()
        {
            _keyMan = new KeyMan(TestConfig.ValidKey);
            Using(_keyMan);
        }

        [Test]
        public void AutomaticallyAssignsIssuedAt()
        {
            var token = _keyMan.Encode(new Dictionary<string,string>());

            var result = _keyMan.Decode(token);

            Assert.That(result.Properties.ContainsKey(KeyMan.Properties.IssuedAt));

            var issuedAt = DateTimeOffset.Parse(result.Properties[KeyMan.Properties.IssuedAt]);

            Assert.That(issuedAt, Is.GreaterThan(DateTimeOffset.Now.AddSeconds(-2)));
            Assert.That(issuedAt, Is.LessThan(DateTimeOffset.Now.AddSeconds(2)));
        }
    }
}