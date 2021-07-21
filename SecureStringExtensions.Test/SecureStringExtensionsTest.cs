using NUnit.Framework;
using System;
using SecureStringExtensions;
using System.Security;

namespace SecureStringExtensions.Test
{
    [TestFixture]
    public class SecureStringExtensionsTest
    {
        [Test]
        [TestCase(new char[] { 'G', 'O', 'A', 'T', '1', '2', '3' })]
        public void CopyCharArrayToSecureStringAndCopyBackToCharArrayReturnsExpected(char[] inputChars)
        {
            SecureString sec = inputChars.ToSecureString();
            var copiedFromSec = sec.FromSecureStringToCharArray();
            CollectionAssert.AreEqual(copiedFromSec, inputChars);                
        }

        [Test]
        [TestCase("GOAT456")]
        public void CopyStringToSecureStringAndCopyBackToUnsafeStringReturnsExpected(string inputString)
        {
            SecureString sec = inputString.ToSecureString();
            var copiedFromSec = sec.FromSecureStringToUnsafeString();
            Assert.AreEqual(copiedFromSec, inputString);
        }
    }
}
