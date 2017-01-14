using System;
using NUnit.Framework;
using Moq;
using PCLCrypto;

namespace PasswordGenerator.Tests
{
    [TestFixture]
    public class HashCryptographerTests
    {
        ICryptographer hashCryptographer;

        [OneTimeSetUp]
        public void Initialize()
        {
            //arrange
            hashCryptographer = new HashCryptographer();
        }

        [Test]
        public void Encrypt_for_not_null_or_empty_param_return_value_is_not_null_or_empty()
        {
            //act + assert
            Assert.That(!string.IsNullOrEmpty(hashCryptographer.Encrypt("default")));
        }

        [Test]
        public void Constructor_by_default_is_Md5()
        {
            //arrange
            ICryptographer md5Cryptographer = new HashCryptographer(HashAlgorithm.Md5);

            //act + assert
            Assert.That(hashCryptographer.Encrypt("") == md5Cryptographer.Encrypt(""));
        }
    }
}