using System;
using NUnit.Framework;
using Moq;
using PCLCrypto;

namespace PasswordGenerator.Core.Tests
{
    [TestFixture]
    public class HashCryptographerTests
    {
        [Test]
        public void Encrypt_ForEqualStrings_GenerateEqualCrypts()
        {
            //arrange
            var cryptographer = GetCryptographer();

            //act
            var crypt1 = cryptographer.Encrypt("SomeString");
            var crypt2 = cryptographer.Encrypt("SomeString");

            //assert
            Assert.AreEqual(crypt1, crypt2);
        }

        [Test]
        public void Constructor_ByDefault_CreateMd5Cryptographer()
        {
            //arrange
            var cryptographerByDefault = GetCryptographer();
            var md5Cryptographer = new HashCryptographer(HashAlgorithm.Md5);

            //act
            var cryptByDefault = cryptographerByDefault.Encrypt("test");
            var md5Crypt = md5Cryptographer.Encrypt("test");

            //assert
            Assert.AreEqual(cryptByDefault, md5Crypt);
        }

        private ICryptographer GetCryptographer()
        {
            return new HashCryptographer();
        }
    }
}