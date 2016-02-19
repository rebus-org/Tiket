using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Reincheck.Tests.Areas
{
    [TestFixture]
    public class Signing : FixtureBase
    {
        [Test]
        public void CanRoundtripToken()
        {
            using (var keyMan = new KeyMan(ValidKey))
            {
                var token = keyMan.Encode(new Dictionary<string, string>
                {
                    {"username", "joe"}
                });

                var decodingResult = keyMan.Decode(token);

                Assert.That(decodingResult.IsValid, Is.True);

                var decodedProperties = decodingResult.Properties;

                Assert.That(decodedProperties.ContainsKey("username"));
                Assert.That(decodedProperties["username"], Is.EqualTo("joe"));
            }
        }

        [Test]
        public void CanRoundtripToken_TamperedTokenIsInvalid()
        {
            using (var keyMan = new KeyMan(ValidKey))
            {
                var token = keyMan.Encode(new Dictionary<string, string>
                {
                    {"username", "joe"}
                });

                var tamperedToken = TamperSomehow(token);

                Console.WriteLine($@"Token:

{token}

TAMPERED token:

{tamperedToken}
");

                var decodingResult = keyMan.Decode(tamperedToken);

                Assert.That(decodingResult.IsValid, Is.False);
            }
        }

        static string TamperSomehow(string token)
        {
            var firstPart = token.Substring(0, 10);
            var middlePart = "aa";
            var secondPart = token.Substring(12);

            return firstPart + middlePart + secondPart;
        }
    }
}