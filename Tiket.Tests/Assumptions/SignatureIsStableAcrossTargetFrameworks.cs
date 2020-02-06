using System;
using System.Collections.Generic;
using NUnit.Framework;
using Testy;

namespace Tiket.Tests.Assumptions
{
    [TestFixture]
    public class SignatureIsStableAcrossTargetFrameworks : FixtureBase
    {
        const string MasterKey = "EccpTS2BpjLwPHSO5MFcoT+Tl337FpxyWFTwsx3n0xM=|AeizFLOVbNtg9zzNnyde0Q==";

        [Test]
        [Explicit]
        [Description("Not a real test - just outputs the encoded string as it looks when generated with this runtime")]
        public void PrintEncodedString()
        {
            var keyMan = Using(new KeyMan(MasterKey));
            var encodedString = keyMan.Encode(new Dictionary<string, string> { { "what", "test" } });
            Console.WriteLine(encodedString);
        }

        [Test]
        public void DecodedKnownEncodedString()
        {
            const string encodedString = "06g8ixCqJ7fTIUafgyp+83n6BE7wJmJ5o41ruKDg/ZIK0FlJA4qPofOszmct9uoPIWQIWBu/PJ2MFaQPoKWN6e8gUe9a00ijDItV5NHFjPFmVenq+RlEGUDPaAUbI5bTftCeA2rUA7/Tbhm/doN+Mi1csYjfG/KXpXhPTQSxATQ=";

            var keyMan = Using(new KeyMan(MasterKey));

            var result = keyMan.Decode(encodedString);

            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Properties, Contains.Key("what").And.ContainValue("test"));
        }
    }
}