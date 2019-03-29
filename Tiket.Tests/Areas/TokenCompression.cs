using System;
using System.Linq;
using NUnit.Framework;
using Testy;

namespace Tiket.Tests.Areas
{
    [TestFixture]
    public class TokenCompression : FixtureBase
    {
        KeyMan _keyMan;

        protected override void SetUp()
        {
            _keyMan = new KeyMan(TestConfig.ValidKey);

            Using(_keyMan);
        }

        /*

        Initial result:

        #props/token length
            0: 349
            5: 501
           10: 653
           20: 973
           50: 1933
          100: 3533

        Zipping of ASCII-encoded token:

        #props/token length
            0: 420
            5: 532
           10: 548
           20: 588
           50: 692
          100: 860

        */

        [Test]
        public void CheckTokenSize()
        {
            Console.WriteLine("#props/token length");
            CheckSize(0);
            CheckSize(5);
            CheckSize(10);
            CheckSize(20);
            CheckSize(50);
            CheckSize(100);
        }

        void CheckSize(int numberOfProperties)
        {
            var properties = Enumerable.Range(0, numberOfProperties)
                .Select(i => $"key-{i}")
                .ToDictionary(a => a, a => "just a value");

            var token = _keyMan.Encode(properties);

            Console.WriteLine($"{numberOfProperties.ToString().PadLeft(5)}: {token.Length} - {token}");
        }
    }
}