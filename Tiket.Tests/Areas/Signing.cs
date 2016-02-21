using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Tiket.Internals;

namespace Tiket.Tests.Areas
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

        [Test, Ignore]
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
            var zipper = new Zipper();

            var zippedBytes = Convert.FromBase64String(token);

            var str = Encoding.ASCII.GetString(zipper.Unzip(zippedBytes));

            Console.WriteLine("Unzipped contents:");
            Console.WriteLine(str);
            Console.WriteLine();

            var parts = str.Split(new [] {"|"}, StringSplitOptions.RemoveEmptyEntries);

            var jsonText = Encoding.UTF8.GetString(Convert.FromBase64String(parts[0]));

            Console.WriteLine("First part as JSON:");
            Console.WriteLine(jsonText);
            Console.WriteLine();

            var tamperedJsonText = jsonText.Replace(@"""joe""", @"""moe""");

            Console.WriteLine("Tampered JSON:");
            Console.WriteLine(tamperedJsonText);
            Console.WriteLine();

            var newStr = Convert.ToBase64String(Encoding.UTF8.GetBytes(tamperedJsonText))
                         + "|"
                         + parts[1];

            Console.WriteLine("New unzipped contents:");
            Console.WriteLine(newStr);
            Console.WriteLine();

            var tamperedZippedBytes = zipper.Zip(Encoding.ASCII.GetBytes(newStr));

            return Convert.ToBase64String(tamperedZippedBytes);
        }
    }
}