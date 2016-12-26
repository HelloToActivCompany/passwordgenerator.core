using System;
using NUnit.Framework;
using Moq;

namespace PasswordGenerator.Tests
{
    [TestFixture]
    public class HashCryptographerTests
    {
        [Test]
        public void Check_md5_as_default_cryptographer_without_param()
        {
            //arrange
            ICryptographer hashCryptographer = new HashCryptographer();

            //act + assert
            Assert.That(hashCryptographer.Encrypt("lalala") == "9aa6e5f2256c17d2d430b100032b997c");
        }

        [Test]
        public void Check_md5_as_default_cryptographer_with_not_valid_hash_algorithm()
        {
            //arrange
            ICryptographer hashCryptographer = new HashCryptographer("gohqg");

            //act + assert
            Assert.That(hashCryptographer.Encrypt("lalala") == "9aa6e5f2256c17d2d430b100032b997c");
        }
    }
}
