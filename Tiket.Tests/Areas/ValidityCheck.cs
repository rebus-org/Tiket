using System;
using System.Collections.Generic;
using System.Security;
using NUnit.Framework;

namespace Tiket.Tests.Areas
{
    [TestFixture]
    public class ValidityCheck
    {
        [TestCase(true, false, false, true, true)]
        [TestCase(true, false, true, true, false)]
        [TestCase(true, true, false, true, false)]
        [TestCase(false, false, false, true, false)]

        [TestCase(true, false, false, false, false)]
        [TestCase(true, false, true, false, false)]
        [TestCase(true, true, false, false, false)]
        [TestCase(false, false, false, false, false)]
        public void CanCheckWhetherDecodingResultIsValue(bool hasVaidSignature, bool isNotValidYet, bool isExpired, bool isCorrectFormat, bool expectToBeValid)
        {
            var validResult = new DecodingResult(new Dictionary<string, string>(), new DecodingResultDetails(hasVaidSignature, isNotValidYet, isExpired, isCorrectFormat));

            if (expectToBeValid)
            {
                validResult.ThrowIfInvalid();
            }
            else
            {
                var securityException = Assert.Throws<SecurityException>(() => validResult.ThrowIfInvalid());

                Console.WriteLine(securityException);
            }
        }
    }
}