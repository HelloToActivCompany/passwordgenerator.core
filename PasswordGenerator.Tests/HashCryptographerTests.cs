using System;
using NUnit.Framework;
using Moq;

namespace PasswordGenerator.Tests
{
    [TestFixture]
    public class HashCryptographerTests
    {
        [Test]
        public void Encrypt_return_value_is_not_null_or_empty()
        {
            //arrange
            ICryptographer hashCryptographer = new HashCryptographer();
            
            //act + assert
            Assert.That(!string.IsNullOrEmpty(hashCryptographer.Encrypt("default")));
        }
    }
}